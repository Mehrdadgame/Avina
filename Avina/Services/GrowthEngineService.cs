using Avina.Data;
using Avina.Models;
using Microsoft.EntityFrameworkCore;

namespace Avina.Services;

public interface IGrowthEngineService
{
    Task<DashboardDto> GetDashboardAsync(int userId, CancellationToken cancellationToken = default);
    Task<TodayMissionDto?> GetTodayMissionAsync(int userId, CancellationToken cancellationToken = default);
    Task<MissionSubmitResultDto> SubmitMissionAsync(int userId, int missionId, string? textAnswer, string? mediaUrl, CancellationToken cancellationToken = default);
    Task<List<OnboardingQuestionDto>> GetOnboardingQuestionsAsync(CancellationToken cancellationToken = default);
    Task<OnboardingResultDto> SubmitOnboardingAsync(OnboardingSubmitRequest request, CancellationToken cancellationToken = default);
    Task SetUserPathAsync(UserPathSelectionRequest request, CancellationToken cancellationToken = default);

    Task<List<GrowthPath>> GetPathsAsync(CancellationToken cancellationToken = default);
    Task<GrowthPath> CreatePathAsync(GrowthPathUpsertRequest request, CancellationToken cancellationToken = default);
    Task<GrowthPath?> UpdatePathAsync(int pathId, GrowthPathUpsertRequest request, CancellationToken cancellationToken = default);

    Task<List<Skill>> GetSkillsAsync(int? pathId = null, CancellationToken cancellationToken = default);
    Task<Skill> CreateSkillAsync(SkillUpsertRequest request, CancellationToken cancellationToken = default);
    Task<Skill?> UpdateSkillAsync(int skillId, SkillUpsertRequest request, CancellationToken cancellationToken = default);

    Task<List<Mission>> GetMissionsAsync(int? pathId = null, int? skillId = null, CancellationToken cancellationToken = default);
    Task<Mission> CreateMissionAsync(MissionUpsertRequest request, CancellationToken cancellationToken = default);
    Task<Mission?> UpdateMissionAsync(int missionId, MissionUpsertRequest request, CancellationToken cancellationToken = default);

    Task<List<MissionSubmission>> GetPendingSubmissionsAsync(CancellationToken cancellationToken = default);
    Task<MissionSubmission?> ReviewSubmissionAsync(int submissionId, bool approve, string? adminFeedback, CancellationToken cancellationToken = default);
}

public sealed class GrowthEngineService(IDbContextFactory<AvinaDbContext> dbFactory) : IGrowthEngineService
{
    private const string ScoreAnalytical = "Analytical";
    private const string ScoreCreativity = "Creativity";
    private const string ScoreSocial = "Social";
    private const string ScoreDiscipline = "Discipline";
    private const string ScoreResilience = "Resilience";
    private const string ScoreFocus = "Focus";
    private const string ScoreConfidence = "Confidence";
    private const string ScoreResponsibility = "Responsibility";
    private const string ScoreCuriosity = "Curiosity";

    public async Task<DashboardDto> GetDashboardAsync(int userId, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var user = await db.Users
            .Include(u => u.GrowthProfile)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
            ?? throw new InvalidOperationException("کاربر پیدا نشد.");

        var needsOnboarding = user.GrowthProfile is null;
        var todayMission = needsOnboarding ? null : await GetTodayMissionInternalAsync(db, userId, cancellationToken);
        var activePath = await GetActivePathAsync(db, userId, cancellationToken);
        var currentSkill = await GetCurrentSkillAsync(db, userId, activePath?.Id, cancellationToken);
        var streak = await CalculateStreakAsync(db, userId, cancellationToken);
        var targetForNextLevel = Math.Max(300, user.Level * 300);

        return new DashboardDto
        {
            UserId = user.Id,
            UserName = !string.IsNullOrWhiteSpace(user.FullName) ? user.FullName : user.Name,
            NeedsOnboarding = needsOnboarding,
            ActivePath = activePath?.Title ?? "مسیر فعالی انتخاب نشده",
            CurrentSkill = currentSkill?.Skill.Title ?? "هنوز شروع نشده",
            Level = user.Level,
            XP = user.Experience,
            XPToNextLevel = Math.Max(0, targetForNextLevel - user.Experience),
            Coin = user.Coin,
            DailyStreak = streak,
            TodayMission = todayMission
        };
    }

    public async Task<TodayMissionDto?> GetTodayMissionAsync(int userId, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        return await GetTodayMissionInternalAsync(db, userId, cancellationToken);
    }

    public async Task<MissionSubmitResultDto> SubmitMissionAsync(int userId, int missionId, string? textAnswer, string? mediaUrl, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
            ?? throw new InvalidOperationException("کاربر پیدا نشد.");

        var mission = await db.Missions
            .Include(m => m.Path)
            .Include(m => m.Skill)
            .FirstOrDefaultAsync(m => m.Id == missionId && m.IsActive, cancellationToken)
            ?? throw new InvalidOperationException("ماموریت پیدا نشد.");

        var hasPendingSubmission = await db.MissionSubmissions
            .AnyAsync(
                s => s.UserId == userId &&
                     s.MissionId == missionId &&
                     s.Status == MissionSubmissionStatus.Pending,
                cancellationToken);

        if (hasPendingSubmission)
        {
            throw new InvalidOperationException("ثبت قبلی این ماموریت هنوز در انتظار بررسی است.");
        }

        ValidateMissionOutput(mission, textAnswer, mediaUrl);

        var submission = new MissionSubmission
        {
            UserId = userId,
            MissionId = missionId,
            TextAnswer = textAnswer?.Trim(),
            MediaUrl = mediaUrl?.Trim(),
            SubmittedAt = DateTime.UtcNow,
            Status = mission.ValidationType == MissionValidationType.SelfReport
                ? MissionSubmissionStatus.AutoApproved
                : MissionSubmissionStatus.Pending
        };

        db.MissionSubmissions.Add(submission);
        await db.SaveChangesAsync(cancellationToken);

        if (submission.Status == MissionSubmissionStatus.AutoApproved)
        {
            submission.ReviewedAt = DateTime.UtcNow;
            await ApplyRewardAndProgressAsync(db, user, mission, submission, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);

            return new MissionSubmitResultDto
            {
                SubmissionId = submission.Id,
                Status = submission.Status,
                GrantedXP = submission.RewardXPGranted,
                GrantedCoin = submission.RewardCoinGranted,
                Message = "ماموریت به‌صورت خودکار تایید شد و پاداش اعمال شد."
            };
        }

        return new MissionSubmitResultDto
        {
            SubmissionId = submission.Id,
            Status = submission.Status,
            GrantedXP = 0,
            GrantedCoin = 0,
            Message = mission.ValidationType == MissionValidationType.EvidenceRequired
                ? "ثبت انجام شد و در انتظار بررسی مستندات است."
                : "ثبت انجام شد و در انتظار بررسی ادمین است."
        };
    }

    public async Task<List<OnboardingQuestionDto>> GetOnboardingQuestionsAsync(CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var questions = await db.OnboardingQuestions
            .AsNoTracking()
            .Where(q => q.IsActive)
            .Include(q => q.Options)
            .OrderBy(q => q.DisplayOrder)
            .ToListAsync(cancellationToken);

        return questions.Select(q => new OnboardingQuestionDto
        {
            Id = q.Id,
            QuestionText = q.QuestionText,
            DisplayOrder = q.DisplayOrder,
            Options = q.Options
                .OrderBy(o => o.OptionKey)
                .Select(o => new OnboardingOptionDto
                {
                    Id = o.Id,
                    OptionKey = o.OptionKey,
                    OptionText = o.OptionText
                })
                .ToList()
        }).ToList();
    }

    public async Task<OnboardingResultDto> SubmitOnboardingAsync(OnboardingSubmitRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Answers.Count < 8)
        {
            throw new InvalidOperationException("حداقل باید به 8 سوال پاسخ داده شود.");
        }

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken)
            ?? throw new InvalidOperationException("کاربر پیدا نشد.");

        var optionIds = request.Answers.Select(a => a.OptionId).Distinct().ToList();
        var options = await db.OnboardingOptions
            .Where(o => optionIds.Contains(o.Id))
            .ToListAsync(cancellationToken);

        if (options.Count != optionIds.Count)
        {
            throw new InvalidOperationException("بخشی از گزینه‌های آزمون معتبر نیستند.");
        }

        var answeredQuestions = request.Answers.Select(a => a.QuestionId).Distinct().Count();
        if (answeredQuestions != request.Answers.Count)
        {
            throw new InvalidOperationException("برای هر سوال فقط یک پاسخ مجاز است.");
        }

        var scores = BuildScoreMap();
        foreach (var answer in request.Answers)
        {
            var option = options.First(o => o.Id == answer.OptionId);
            if (option.QuestionId != answer.QuestionId)
            {
                throw new InvalidOperationException("گزینه انتخاب‌شده مربوط به این سوال نیست.");
            }

            scores[ScoreAnalytical] += option.AnalyticalScoreDelta;
            scores[ScoreCreativity] += option.CreativityScoreDelta;
            scores[ScoreSocial] += option.SocialScoreDelta;
            scores[ScoreDiscipline] += option.DisciplineScoreDelta;
            scores[ScoreResilience] += option.ResilienceScoreDelta;
            scores[ScoreFocus] += option.FocusScoreDelta;
            scores[ScoreConfidence] += option.ConfidenceScoreDelta;
            scores[ScoreResponsibility] += option.ResponsibilityScoreDelta;
            scores[ScoreCuriosity] += option.CuriosityScoreDelta;
        }

        var growthProfile = await db.GrowthProfiles.FirstOrDefaultAsync(g => g.UserId == request.UserId, cancellationToken);
        if (growthProfile is null)
        {
            growthProfile = new GrowthProfile { UserId = request.UserId };
            db.GrowthProfiles.Add(growthProfile);
        }

        growthProfile.AnalyticalScore = scores[ScoreAnalytical];
        growthProfile.CreativityScore = scores[ScoreCreativity];
        growthProfile.SocialScore = scores[ScoreSocial];
        growthProfile.DisciplineScore = scores[ScoreDiscipline];
        growthProfile.ResilienceScore = scores[ScoreResilience];
        growthProfile.FocusScore = scores[ScoreFocus];
        growthProfile.ConfidenceScore = scores[ScoreConfidence];
        growthProfile.ResponsibilityScore = scores[ScoreResponsibility];
        growthProfile.CuriosityScore = scores[ScoreCuriosity];
        growthProfile.UpdatedAt = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(request.FullName))
        {
            user.FullName = request.FullName.Trim();
            if (string.IsNullOrWhiteSpace(user.Name))
            {
                user.Name = user.FullName;
            }
        }

        if (request.Age.HasValue && request.Age.Value > 0)
        {
            user.Age = request.Age.Value;
        }

        var recommendedPathId = RecommendPrimaryPath(scores);
        var secondPathId = RecommendSecondaryPath(scores, recommendedPathId);

        await SetPrimaryPathInternalAsync(db, request.UserId, recommendedPathId, cancellationToken);
        if (secondPathId.HasValue)
        {
            await EnsureUserPathAsync(db, request.UserId, secondPathId.Value, isActive: true, isPrimary: false, cancellationToken);
        }

        await EnsureSkillProgressForPathAsync(db, request.UserId, recommendedPathId, cancellationToken);

        var attempt = new OnboardingAttempt
        {
            UserId = request.UserId,
            RecommendedPathId = recommendedPathId,
            CompletedAt = DateTime.UtcNow
        };
        db.OnboardingAttempts.Add(attempt);
        await db.SaveChangesAsync(cancellationToken);

        var answers = request.Answers.Select(a => new OnboardingAnswer
        {
            AttemptId = attempt.Id,
            QuestionId = a.QuestionId,
            OptionId = a.OptionId
        });
        db.OnboardingAnswers.AddRange(answers);

        user.Experience += 20;
        user.Coin += 10;
        user.Level = Math.Max(1, 1 + user.Experience / 300);
        db.RewardTransactions.Add(new RewardTransaction
        {
            UserId = user.Id,
            SourceType = RewardSourceType.OnboardingBonus,
            SourceId = attempt.Id,
            XPAmount = 20,
            CoinAmount = 10,
            CreatedAt = DateTime.UtcNow
        });

        await db.SaveChangesAsync(cancellationToken);

        var path = await db.GrowthPaths.AsNoTracking().FirstAsync(p => p.Id == recommendedPathId, cancellationToken);
        return new OnboardingResultDto
        {
            UserId = request.UserId,
            GrowthProfileId = growthProfile.Id,
            RecommendedPathId = recommendedPathId,
            RecommendedPathTitle = path.Title,
            Scores = ToScoreMapFa(scores)
        };
    }

    public async Task SetUserPathAsync(UserPathSelectionRequest request, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var pathExists = await db.GrowthPaths.AnyAsync(p => p.Id == request.PathId && p.IsActive, cancellationToken);
        if (!pathExists)
        {
            throw new InvalidOperationException("مسیر رشد پیدا نشد.");
        }

        if (request.MakePrimary)
        {
            await SetPrimaryPathInternalAsync(db, request.UserId, request.PathId, cancellationToken);
        }
        else
        {
            await EnsureUserPathAsync(db, request.UserId, request.PathId, isActive: true, isPrimary: false, cancellationToken);
        }

        await EnsureSkillProgressForPathAsync(db, request.UserId, request.PathId, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<GrowthPath>> GetPathsAsync(CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        return await db.GrowthPaths.AsNoTracking().OrderBy(p => p.Id).ToListAsync(cancellationToken);
    }

    public async Task<GrowthPath> CreatePathAsync(GrowthPathUpsertRequest request, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var path = new GrowthPath
        {
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            PathType = request.PathType,
            Icon = request.Icon.Trim(),
            IsActive = request.IsActive
        };
        db.GrowthPaths.Add(path);
        await db.SaveChangesAsync(cancellationToken);
        return path;
    }

    public async Task<GrowthPath?> UpdatePathAsync(int pathId, GrowthPathUpsertRequest request, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var path = await db.GrowthPaths.FirstOrDefaultAsync(p => p.Id == pathId, cancellationToken);
        if (path is null)
        {
            return null;
        }

        path.Title = request.Title.Trim();
        path.Description = request.Description.Trim();
        path.PathType = request.PathType;
        path.Icon = request.Icon.Trim();
        path.IsActive = request.IsActive;
        await db.SaveChangesAsync(cancellationToken);
        return path;
    }

    public async Task<List<Skill>> GetSkillsAsync(int? pathId = null, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var query = db.Skills.AsNoTracking().AsQueryable();
        if (pathId.HasValue)
        {
            query = query.Where(s => s.PathId == pathId.Value);
        }

        return await query.OrderBy(s => s.PathId).ThenBy(s => s.Order).ToListAsync(cancellationToken);
    }

    public async Task<Skill> CreateSkillAsync(SkillUpsertRequest request, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var skill = new Skill
        {
            PathId = request.PathId,
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Order = request.Order,
            Level = request.Level,
            IsCoreSkill = request.IsCoreSkill
        };
        db.Skills.Add(skill);
        await db.SaveChangesAsync(cancellationToken);
        return skill;
    }

    public async Task<Skill?> UpdateSkillAsync(int skillId, SkillUpsertRequest request, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var skill = await db.Skills.FirstOrDefaultAsync(s => s.Id == skillId, cancellationToken);
        if (skill is null)
        {
            return null;
        }

        skill.PathId = request.PathId;
        skill.Title = request.Title.Trim();
        skill.Description = request.Description.Trim();
        skill.Order = request.Order;
        skill.Level = request.Level;
        skill.IsCoreSkill = request.IsCoreSkill;
        await db.SaveChangesAsync(cancellationToken);
        return skill;
    }

    public async Task<List<Mission>> GetMissionsAsync(int? pathId = null, int? skillId = null, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var query = db.Missions.AsNoTracking().AsQueryable();
        if (pathId.HasValue)
        {
            query = query.Where(m => m.PathId == pathId.Value);
        }

        if (skillId.HasValue)
        {
            query = query.Where(m => m.SkillId == skillId.Value);
        }

        return await query.OrderBy(m => m.PathId).ThenBy(m => m.SkillId).ThenBy(m => m.Difficulty).ToListAsync(cancellationToken);
    }

    public async Task<Mission> CreateMissionAsync(MissionUpsertRequest request, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var mission = new Mission
        {
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Goal = request.Goal.Trim(),
            PathId = request.PathId,
            SkillId = request.SkillId,
            MissionType = request.MissionType,
            Difficulty = Math.Clamp(request.Difficulty, 1, 5),
            EstimatedMinutes = Math.Max(5, request.EstimatedMinutes),
            RequiredOutputType = request.RequiredOutputType,
            ValidationType = request.ValidationType,
            RewardXP = Math.Max(1, request.RewardXP),
            RewardCoin = Math.Max(0, request.RewardCoin),
            SkillProgressGain = Math.Clamp(request.SkillProgressGain, 5, 100),
            IsActive = request.IsActive
        };
        db.Missions.Add(mission);
        await db.SaveChangesAsync(cancellationToken);
        return mission;
    }

    public async Task<Mission?> UpdateMissionAsync(int missionId, MissionUpsertRequest request, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var mission = await db.Missions.FirstOrDefaultAsync(m => m.Id == missionId, cancellationToken);
        if (mission is null)
        {
            return null;
        }

        mission.Title = request.Title.Trim();
        mission.Description = request.Description.Trim();
        mission.Goal = request.Goal.Trim();
        mission.PathId = request.PathId;
        mission.SkillId = request.SkillId;
        mission.MissionType = request.MissionType;
        mission.Difficulty = Math.Clamp(request.Difficulty, 1, 5);
        mission.EstimatedMinutes = Math.Max(5, request.EstimatedMinutes);
        mission.RequiredOutputType = request.RequiredOutputType;
        mission.ValidationType = request.ValidationType;
        mission.RewardXP = Math.Max(1, request.RewardXP);
        mission.RewardCoin = Math.Max(0, request.RewardCoin);
        mission.SkillProgressGain = Math.Clamp(request.SkillProgressGain, 5, 100);
        mission.IsActive = request.IsActive;
        await db.SaveChangesAsync(cancellationToken);
        return mission;
    }

    public async Task<List<MissionSubmission>> GetPendingSubmissionsAsync(CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        return await db.MissionSubmissions
            .AsNoTracking()
            .Include(s => s.User)
            .Include(s => s.Mission)
                .ThenInclude(m => m.Path)
            .Where(s => s.Status == MissionSubmissionStatus.Pending)
            .OrderBy(s => s.SubmittedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<MissionSubmission?> ReviewSubmissionAsync(int submissionId, bool approve, string? adminFeedback, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);
        var submission = await db.MissionSubmissions
            .Include(s => s.User)
            .Include(s => s.Mission)
                .ThenInclude(m => m.Path)
            .Include(s => s.Mission)
                .ThenInclude(m => m.Skill)
            .FirstOrDefaultAsync(s => s.Id == submissionId, cancellationToken);

        if (submission is null)
        {
            return null;
        }

        if (submission.Status != MissionSubmissionStatus.Pending)
        {
            return submission;
        }

        submission.ReviewedAt = DateTime.UtcNow;
        submission.AdminFeedback = adminFeedback?.Trim();
        submission.Status = approve ? MissionSubmissionStatus.Approved : MissionSubmissionStatus.Rejected;

        if (approve)
        {
            await ApplyRewardAndProgressAsync(db, submission.User, submission.Mission, submission, cancellationToken);
        }

        await db.SaveChangesAsync(cancellationToken);
        return submission;
    }

    private static void ValidateMissionOutput(Mission mission, string? textAnswer, string? mediaUrl)
    {
        if (mission.RequiredOutputType == RequiredOutputType.Text && string.IsNullOrWhiteSpace(textAnswer))
        {
            throw new InvalidOperationException("برای این ماموریت باید پاسخ متنی وارد شود.");
        }

        if ((mission.RequiredOutputType == RequiredOutputType.Image || mission.RequiredOutputType == RequiredOutputType.Video)
            && string.IsNullOrWhiteSpace(mediaUrl))
        {
            throw new InvalidOperationException("برای این ماموریت باید مدرک رسانه‌ای ارسال شود.");
        }
    }

    private async Task<TodayMissionDto?> GetTodayMissionInternalAsync(AvinaDbContext db, int userId, CancellationToken cancellationToken)
    {
        var profileExists = await db.GrowthProfiles.AnyAsync(g => g.UserId == userId, cancellationToken);
        if (!profileExists)
        {
            return null;
        }

        var path = await GetActivePathAsync(db, userId, cancellationToken);
        if (path is null)
        {
            var profile = await db.GrowthProfiles.FirstAsync(g => g.UserId == userId, cancellationToken);
            var scores = new Dictionary<string, int>
            {
                [ScoreAnalytical] = profile.AnalyticalScore,
                [ScoreCreativity] = profile.CreativityScore,
                [ScoreSocial] = profile.SocialScore,
                [ScoreDiscipline] = profile.DisciplineScore,
                [ScoreResilience] = profile.ResilienceScore,
                [ScoreFocus] = profile.FocusScore,
                [ScoreConfidence] = profile.ConfidenceScore,
                [ScoreResponsibility] = profile.ResponsibilityScore,
                [ScoreCuriosity] = profile.CuriosityScore
            };
            var recommendedPathId = RecommendPrimaryPath(scores);
            await SetPrimaryPathInternalAsync(db, userId, recommendedPathId, cancellationToken);
            await EnsureSkillProgressForPathAsync(db, userId, recommendedPathId, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
            path = await db.GrowthPaths.AsNoTracking().FirstAsync(p => p.Id == recommendedPathId, cancellationToken);
        }

        var currentSkill = await GetCurrentSkillAsync(db, userId, path.Id, cancellationToken);
        if (currentSkill is null)
        {
            await EnsureSkillProgressForPathAsync(db, userId, path.Id, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
            currentSkill = await GetCurrentSkillAsync(db, userId, path.Id, cancellationToken);
        }

        if (currentSkill is null)
        {
            return null;
        }

        var blockedMissionIds = await GetBlockedMissionIdsAsync(db, userId, cancellationToken);
        var targetDifficulty = await CalculateAdaptiveDifficultyAsync(db, userId, cancellationToken);
        var candidateMissions = await db.Missions
            .AsNoTracking()
            .Include(m => m.Skill)
            .Where(m => m.IsActive && m.PathId == path.Id && m.SkillId == currentSkill.SkillId && !blockedMissionIds.Contains(m.Id))
            .ToListAsync(cancellationToken);

        if (candidateMissions.Count == 0)
        {
            candidateMissions = await db.Missions
                .AsNoTracking()
                .Include(m => m.Skill)
                .Where(m => m.IsActive && m.PathId == path.Id && !blockedMissionIds.Contains(m.Id))
                .ToListAsync(cancellationToken);
        }

        var selected = candidateMissions
            .OrderBy(m => m.SkillId == currentSkill.SkillId ? 0 : 1)
            .ThenBy(m => m.Skill.Order)
            .ThenBy(m => Math.Abs(m.Difficulty - targetDifficulty))
            .ThenByDescending(m => m.MissionType == MissionType.RealWorld)
            .ThenBy(m => m.Id)
            .FirstOrDefault();

        if (selected is null)
        {
            return null;
        }

        var reason = BuildRecommendationReason(selected, currentSkill.Status, targetDifficulty);
        return new TodayMissionDto
        {
            MissionId = selected.Id,
            MissionTitle = selected.Title,
            MissionDescription = selected.Description,
            EstimatedTime = selected.EstimatedMinutes,
            Difficulty = selected.Difficulty,
            RewardXP = selected.RewardXP,
            RewardCoin = selected.RewardCoin,
            ConnectedSkill = selected.Skill.Title,
            ConnectedPath = path.Title,
            ReasonForRecommendation = reason
        };
    }

    private static async Task<HashSet<int>> GetBlockedMissionIdsAsync(
        AvinaDbContext db,
        int userId,
        CancellationToken cancellationToken)
    {
        var missionIds = await db.MissionSubmissions
            .AsNoTracking()
            .Where(s =>
                s.UserId == userId &&
                s.Status != MissionSubmissionStatus.Rejected)
            .Select(s => s.MissionId)
            .Distinct()
            .ToListAsync(cancellationToken);

        return missionIds.ToHashSet();
    }

    private async Task<GrowthPath?> GetActivePathAsync(AvinaDbContext db, int userId, CancellationToken cancellationToken)
    {
        return await db.UserGrowthPaths
            .AsNoTracking()
            .Include(p => p.GrowthPath)
            .Where(p => p.UserId == userId && p.IsActive)
            .OrderByDescending(p => p.IsPrimary)
            .ThenBy(p => p.CreatedAt)
            .Select(p => p.GrowthPath)
            .FirstOrDefaultAsync(cancellationToken);
    }

    private async Task<UserSkillProgress?> GetCurrentSkillAsync(AvinaDbContext db, int userId, int? pathId, CancellationToken cancellationToken)
    {
        if (!pathId.HasValue)
        {
            return null;
        }

        return await db.UserSkillProgresses
            .Include(s => s.Skill)
            .Where(s => s.UserId == userId && s.Skill.PathId == pathId.Value && s.Status != UserSkillStatus.Completed)
            .OrderByDescending(s => s.Status == UserSkillStatus.InProgress)
            .ThenByDescending(s => s.Status == UserSkillStatus.Unlocked)
            .ThenBy(s => s.Skill.Order)
            .FirstOrDefaultAsync(cancellationToken);
    }

    private async Task<int> CalculateAdaptiveDifficultyAsync(AvinaDbContext db, int userId, CancellationToken cancellationToken)
    {
        var recent = await db.MissionSubmissions
            .AsNoTracking()
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.SubmittedAt)
            .Take(6)
            .ToListAsync(cancellationToken);

        if (recent.Count == 0)
        {
            return 2;
        }

        var successful = recent.Count(s => s.Status == MissionSubmissionStatus.Approved || s.Status == MissionSubmissionStatus.AutoApproved);
        var successRate = successful / (double)recent.Count;

        if (successRate >= 0.8)
        {
            return 4;
        }

        if (successRate <= 0.35)
        {
            return 1;
        }

        return 3;
    }

    private static string BuildRecommendationReason(Mission mission, UserSkillStatus skillStatus, int targetDifficulty)
    {
        var statusReason = skillStatus switch
        {
            UserSkillStatus.InProgress => "این مهارت در حال پیشرفت شماست.",
            UserSkillStatus.Unlocked => "این مهارت، قدم بازشده بعدی در مسیر شماست.",
            _ => "این ماموریت شما را در مسیر فعلی رشد جلو می‌برد."
        };

        return $"{statusReason} سطح سختی هدف امروز {targetDifficulty} است و نوع ماموریت {ToMissionTypeFa(mission.MissionType)} می‌باشد.";
    }

    private static string ToMissionTypeFa(MissionType missionType) => missionType switch
    {
        MissionType.Educational => "آموزشی",
        MissionType.Practice => "تمرینی",
        MissionType.RealWorld => "دنیای واقعی",
        MissionType.Social => "اجتماعی",
        MissionType.Creative => "خلاق",
        MissionType.LongTerm => "بلندمدت",
        _ => "نامشخص"
    };

    private static Dictionary<string, int> BuildScoreMap() => new()
    {
        [ScoreAnalytical] = 0,
        [ScoreCreativity] = 0,
        [ScoreSocial] = 0,
        [ScoreDiscipline] = 0,
        [ScoreResilience] = 0,
        [ScoreFocus] = 0,
        [ScoreConfidence] = 0,
        [ScoreResponsibility] = 0,
        [ScoreCuriosity] = 0
    };

    private static Dictionary<string, int> ToScoreMapFa(Dictionary<string, int> scores) => new()
    {
        ["تحلیلی"] = scores[ScoreAnalytical],
        ["خلاقیت"] = scores[ScoreCreativity],
        ["اجتماعی"] = scores[ScoreSocial],
        ["انضباط"] = scores[ScoreDiscipline],
        ["تاب‌آوری"] = scores[ScoreResilience],
        ["تمرکز"] = scores[ScoreFocus],
        ["اعتمادبه‌نفس"] = scores[ScoreConfidence],
        ["مسئولیت‌پذیری"] = scores[ScoreResponsibility],
        ["کنجکاوی"] = scores[ScoreCuriosity]
    };

    private static int RecommendPrimaryPath(Dictionary<string, int> scores)
    {
        var weighted = new Dictionary<int, int>
        {
            [1] = scores[ScoreAnalytical] + scores[ScoreFocus] + scores[ScoreCuriosity],
            [2] = scores[ScoreCreativity] + scores[ScoreCuriosity] + scores[ScoreConfidence],
            [3] = scores[ScoreSocial] + scores[ScoreConfidence] + scores[ScoreResponsibility],
            [4] = scores[ScoreResponsibility] + scores[ScoreDiscipline] + scores[ScoreResilience],
            [5] = scores[ScoreSocial] + scores[ScoreResponsibility] + scores[ScoreResilience]
        };

        return weighted.OrderByDescending(x => x.Value).ThenBy(x => x.Key).First().Key;
    }

    private static int? RecommendSecondaryPath(Dictionary<string, int> scores, int primaryPathId)
    {
        var weighted = new Dictionary<int, int>
        {
            [1] = scores[ScoreAnalytical] + scores[ScoreFocus] + scores[ScoreCuriosity],
            [2] = scores[ScoreCreativity] + scores[ScoreCuriosity] + scores[ScoreConfidence],
            [3] = scores[ScoreSocial] + scores[ScoreConfidence] + scores[ScoreResponsibility],
            [4] = scores[ScoreResponsibility] + scores[ScoreDiscipline] + scores[ScoreResilience],
            [5] = scores[ScoreSocial] + scores[ScoreResponsibility] + scores[ScoreResilience]
        };

        return weighted
            .Where(x => x.Key != primaryPathId)
            .OrderByDescending(x => x.Value)
            .ThenBy(x => x.Key)
            .Select(x => (int?)x.Key)
            .FirstOrDefault();
    }

    private async Task SetPrimaryPathInternalAsync(AvinaDbContext db, int userId, int pathId, CancellationToken cancellationToken)
    {
        var paths = await db.UserGrowthPaths.Where(p => p.UserId == userId).ToListAsync(cancellationToken);
        foreach (var item in paths)
        {
            item.IsPrimary = false;
            if (item.GrowthPathId == pathId)
            {
                item.IsActive = true;
            }
        }

        var existing = paths.FirstOrDefault(p => p.GrowthPathId == pathId);
        if (existing is null)
        {
            db.UserGrowthPaths.Add(new UserGrowthPath
            {
                UserId = userId,
                GrowthPathId = pathId,
                IsPrimary = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });
        }
        else
        {
            existing.IsPrimary = true;
            existing.IsActive = true;
        }
    }

    private async Task EnsureUserPathAsync(AvinaDbContext db, int userId, int pathId, bool isActive, bool isPrimary, CancellationToken cancellationToken)
    {
        var existing = await db.UserGrowthPaths
            .FirstOrDefaultAsync(p => p.UserId == userId && p.GrowthPathId == pathId, cancellationToken);

        if (existing is null)
        {
            db.UserGrowthPaths.Add(new UserGrowthPath
            {
                UserId = userId,
                GrowthPathId = pathId,
                IsActive = isActive,
                IsPrimary = isPrimary,
                CreatedAt = DateTime.UtcNow
            });
            return;
        }

        existing.IsActive = isActive || existing.IsActive;
        existing.IsPrimary = isPrimary;
    }

    private async Task EnsureSkillProgressForPathAsync(AvinaDbContext db, int userId, int pathId, CancellationToken cancellationToken)
    {
        var skills = await db.Skills
            .AsNoTracking()
            .Where(s => s.PathId == pathId)
            .OrderBy(s => s.Order)
            .ToListAsync(cancellationToken);

        if (skills.Count == 0)
        {
            return;
        }

        var existing = await db.UserSkillProgresses
            .Where(p => p.UserId == userId && skills.Select(s => s.Id).Contains(p.SkillId))
            .ToListAsync(cancellationToken);

        foreach (var skill in skills)
        {
            if (existing.Any(e => e.SkillId == skill.Id))
            {
                continue;
            }

            db.UserSkillProgresses.Add(new UserSkillProgress
            {
                UserId = userId,
                SkillId = skill.Id,
                ProgressPercent = 0,
                CurrentLevel = skill.Level,
                Status = skill.Order == 1 ? UserSkillStatus.Unlocked : UserSkillStatus.Locked,
                UpdatedAt = DateTime.UtcNow
            });
        }
    }

    private async Task ApplyRewardAndProgressAsync(
        AvinaDbContext db,
        User user,
        Mission mission,
        MissionSubmission submission,
        CancellationToken cancellationToken)
    {
        if (submission.IsRewardGranted)
        {
            return;
        }

        var priorSameTypeCount = await db.MissionSubmissions
            .AsNoTracking()
            .Include(s => s.Mission)
            .CountAsync(s =>
                s.UserId == user.Id &&
                s.Id != submission.Id &&
                s.IsRewardGranted &&
                s.Status != MissionSubmissionStatus.Rejected &&
                s.Mission.MissionType == mission.MissionType,
                cancellationToken);

        var typeMultiplier = mission.MissionType switch
        {
            MissionType.RealWorld => 1.35,
            MissionType.LongTerm => 1.25,
            MissionType.Social => 1.15,
            MissionType.Creative => 1.10,
            MissionType.Practice => 1.0,
            MissionType.Educational => 0.80,
            _ => 1.0
        };

        var repeatXpMultiplier = priorSameTypeCount switch
        {
            0 => 1.0,
            1 => 0.7,
            2 => 0.4,
            _ => 0.2
        };

        var repeatCoinMultiplier = priorSameTypeCount switch
        {
            0 => 1.0,
            1 => 0.7,
            2 => 0.4,
            _ => 0.0
        };

        var xp = Math.Max(1, (int)Math.Round(mission.RewardXP * typeMultiplier * repeatXpMultiplier));
        var coin = Math.Max(0, (int)Math.Round(mission.RewardCoin * typeMultiplier * repeatCoinMultiplier));

        submission.IsRewardGranted = true;
        submission.RewardXPGranted = xp;
        submission.RewardCoinGranted = coin;

        user.Experience += xp;
        user.Coin += coin;
        user.Level = Math.Max(1, 1 + user.Experience / 300);

        db.RewardTransactions.Add(new RewardTransaction
        {
            UserId = user.Id,
            SourceType = RewardSourceType.MissionSubmission,
            SourceId = submission.Id,
            XPAmount = xp,
            CoinAmount = coin,
            CreatedAt = DateTime.UtcNow
        });

        var skillProgress = await db.UserSkillProgresses
            .FirstOrDefaultAsync(s => s.UserId == user.Id && s.SkillId == mission.SkillId, cancellationToken);

        if (skillProgress is null)
        {
            skillProgress = new UserSkillProgress
            {
                UserId = user.Id,
                SkillId = mission.SkillId,
                ProgressPercent = 0,
                CurrentLevel = 1,
                Status = UserSkillStatus.Unlocked,
                UpdatedAt = DateTime.UtcNow
            };
            db.UserSkillProgresses.Add(skillProgress);
        }

        if (skillProgress.Status == UserSkillStatus.Locked)
        {
            skillProgress.Status = UserSkillStatus.Unlocked;
        }

        if (skillProgress.Status != UserSkillStatus.Completed)
        {
            skillProgress.Status = UserSkillStatus.InProgress;
            skillProgress.ProgressPercent = Math.Min(100, skillProgress.ProgressPercent + mission.SkillProgressGain);
            skillProgress.UpdatedAt = DateTime.UtcNow;

            if (skillProgress.ProgressPercent >= 100)
            {
                skillProgress.Status = UserSkillStatus.Completed;
                skillProgress.CurrentLevel += 1;
                await UnlockDependentSkillsAsync(db, user.Id, mission.SkillId, cancellationToken);
            }
        }

        await UpdateGrowthProfileByMissionAsync(db, user.Id, mission, cancellationToken);
    }

    private async Task UnlockDependentSkillsAsync(AvinaDbContext db, int userId, int skillId, CancellationToken cancellationToken)
    {
        var dependencies = await db.SkillDependencies
            .AsNoTracking()
            .Where(d => d.RequiredSkillId == skillId)
            .Select(d => d.SkillId)
            .ToListAsync(cancellationToken);

        if (dependencies.Count == 0)
        {
            return;
        }

        foreach (var dependentSkillId in dependencies)
        {
            var progress = await db.UserSkillProgresses
                .FirstOrDefaultAsync(p => p.UserId == userId && p.SkillId == dependentSkillId, cancellationToken);

            if (progress is null)
            {
                db.UserSkillProgresses.Add(new UserSkillProgress
                {
                    UserId = userId,
                    SkillId = dependentSkillId,
                    ProgressPercent = 0,
                    CurrentLevel = 1,
                    Status = UserSkillStatus.Unlocked,
                    UpdatedAt = DateTime.UtcNow
                });
            }
            else if (progress.Status == UserSkillStatus.Locked)
            {
                progress.Status = UserSkillStatus.Unlocked;
                progress.UpdatedAt = DateTime.UtcNow;
            }
        }
    }

    private async Task UpdateGrowthProfileByMissionAsync(AvinaDbContext db, int userId, Mission mission, CancellationToken cancellationToken)
    {
        var profile = await db.GrowthProfiles.FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
        if (profile is null)
        {
            profile = new GrowthProfile { UserId = userId };
            db.GrowthProfiles.Add(profile);
        }

        switch (mission.Path.PathType)
        {
            case PathType.Analytical:
                profile.AnalyticalScore += 2;
                break;
            case PathType.Creative:
                profile.CreativityScore += 2;
                break;
            case PathType.SocialLeadership:
                profile.SocialScore += 2;
                profile.ConfidenceScore += 1;
                break;
            case PathType.MakerPractical:
                profile.ResponsibilityScore += 1;
                profile.DisciplineScore += 1;
                profile.ResilienceScore += 1;
                break;
            case PathType.CareMeaning:
                profile.SocialScore += 1;
                profile.ResponsibilityScore += 2;
                break;
        }

        switch (mission.MissionType)
        {
            case MissionType.Educational:
                profile.AnalyticalScore += 1;
                profile.CuriosityScore += 1;
                break;
            case MissionType.Practice:
                profile.FocusScore += 1;
                profile.DisciplineScore += 1;
                break;
            case MissionType.RealWorld:
                profile.ResponsibilityScore += 2;
                profile.ResilienceScore += 1;
                break;
            case MissionType.Social:
                profile.SocialScore += 2;
                profile.ConfidenceScore += 1;
                break;
            case MissionType.Creative:
                profile.CreativityScore += 2;
                profile.CuriosityScore += 1;
                break;
            case MissionType.LongTerm:
                profile.ResilienceScore += 2;
                profile.DisciplineScore += 1;
                break;
        }

        // مهارت‌های هسته‌ای که تقویت امتیاز اضافه دارند (براساس داده اولیه MVP).
        if (mission.SkillId == 1)
        {
            profile.FocusScore += 2;
            profile.DisciplineScore += 1;
        }

        if (mission.SkillId == 3)
        {
            profile.AnalyticalScore += 2;
        }

        if (mission.SkillId == 8)
        {
            profile.SocialScore += 2;
            profile.ConfidenceScore += 1;
        }

        profile.UpdatedAt = DateTime.UtcNow;
    }

    private async Task<int> CalculateStreakAsync(AvinaDbContext db, int userId, CancellationToken cancellationToken)
    {
        var completedDates = await db.MissionSubmissions
            .AsNoTracking()
            .Where(s =>
                s.UserId == userId &&
                (s.Status == MissionSubmissionStatus.AutoApproved || s.Status == MissionSubmissionStatus.Approved))
            .Select(s => s.SubmittedAt.Date)
            .Distinct()
            .OrderByDescending(d => d)
            .ToListAsync(cancellationToken);

        if (completedDates.Count == 0)
        {
            return 0;
        }

        var streak = 0;
        var cursor = DateTime.UtcNow.Date;
        foreach (var date in completedDates)
        {
            if (date == cursor)
            {
                streak++;
                cursor = cursor.AddDays(-1);
                continue;
            }

            if (date > cursor)
            {
                continue;
            }

            break;
        }

        return streak;
    }
}
