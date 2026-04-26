namespace Avina.Models;

public enum PathType
{
    Analytical = 1,
    Creative = 2,
    SocialLeadership = 3,
    MakerPractical = 4,
    CareMeaning = 5
}

public enum UserSkillStatus
{
    Locked = 1,
    Unlocked = 2,
    InProgress = 3,
    Completed = 4
}

public enum MissionType
{
    Educational = 1,
    Practice = 2,
    RealWorld = 3,
    Social = 4,
    Creative = 5,
    LongTerm = 6
}

public enum RequiredOutputType
{
    None = 1,
    Text = 2,
    Image = 3,
    Video = 4,
    Checkbox = 5,
    QuizAnswer = 6
}

public enum MissionValidationType
{
    SelfReport = 1,
    EvidenceRequired = 2,
    AdminReview = 3
}

public enum MissionSubmissionStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3,
    AutoApproved = 4
}

public enum RewardSourceType
{
    MissionSubmission = 1,
    OnboardingBonus = 2,
    AdminAdjustment = 3
}

public class GrowthProfile
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int AnalyticalScore { get; set; }
    public int CreativityScore { get; set; }
    public int SocialScore { get; set; }
    public int DisciplineScore { get; set; }
    public int ResilienceScore { get; set; }
    public int FocusScore { get; set; }
    public int ConfidenceScore { get; set; }
    public int ResponsibilityScore { get; set; }
    public int CuriosityScore { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
}

public class GrowthPath
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public PathType PathType { get; set; }
    public string Icon { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public ICollection<Skill> Skills { get; set; } = new List<Skill>();
    public ICollection<Mission> Missions { get; set; } = new List<Mission>();
    public ICollection<UserGrowthPath> UserPaths { get; set; } = new List<UserGrowthPath>();
}

public class UserGrowthPath
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int GrowthPathId { get; set; }
    public bool IsActive { get; set; }
    public bool IsPrimary { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public GrowthPath GrowthPath { get; set; } = null!;
}

public class Skill
{
    public int Id { get; set; }
    public int PathId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Order { get; set; }
    public int Level { get; set; } = 1;
    public bool IsCoreSkill { get; set; } = true;

    public GrowthPath Path { get; set; } = null!;
    public ICollection<SkillDependency> Dependencies { get; set; } = new List<SkillDependency>();
    public ICollection<SkillDependency> RequiredBy { get; set; } = new List<SkillDependency>();
    public ICollection<UserSkillProgress> UserProgresses { get; set; } = new List<UserSkillProgress>();
    public ICollection<Mission> Missions { get; set; } = new List<Mission>();
}

public class SkillDependency
{
    public int Id { get; set; }
    public int SkillId { get; set; }
    public int RequiredSkillId { get; set; }

    public Skill Skill { get; set; } = null!;
    public Skill RequiredSkill { get; set; } = null!;
}

public class UserSkillProgress
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int SkillId { get; set; }
    public int ProgressPercent { get; set; }
    public int CurrentLevel { get; set; } = 1;
    public UserSkillStatus Status { get; set; } = UserSkillStatus.Locked;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public Skill Skill { get; set; } = null!;
}

public class Mission
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Goal { get; set; } = string.Empty;
    public int PathId { get; set; }
    public int SkillId { get; set; }
    public MissionType MissionType { get; set; }
    public int Difficulty { get; set; } = 1;
    public int EstimatedMinutes { get; set; } = 10;
    public RequiredOutputType RequiredOutputType { get; set; } = RequiredOutputType.None;
    public MissionValidationType ValidationType { get; set; } = MissionValidationType.SelfReport;
    public int RewardXP { get; set; } = 10;
    public int RewardCoin { get; set; } = 5;
    public int SkillProgressGain { get; set; } = 20;
    public bool IsActive { get; set; } = true;

    public GrowthPath Path { get; set; } = null!;
    public Skill Skill { get; set; } = null!;
    public ICollection<MissionSubmission> Submissions { get; set; } = new List<MissionSubmission>();
}

public class MissionSubmission
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int MissionId { get; set; }
    public string? TextAnswer { get; set; }
    public string? MediaUrl { get; set; }
    public MissionSubmissionStatus Status { get; set; } = MissionSubmissionStatus.Pending;
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReviewedAt { get; set; }
    public string? AdminFeedback { get; set; }
    public int RewardXPGranted { get; set; }
    public int RewardCoinGranted { get; set; }
    public bool IsRewardGranted { get; set; }

    public User User { get; set; } = null!;
    public Mission Mission { get; set; } = null!;
}

public class RewardTransaction
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public RewardSourceType SourceType { get; set; } = RewardSourceType.MissionSubmission;
    public int SourceId { get; set; }
    public int XPAmount { get; set; }
    public int CoinAmount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
}

public class OnboardingQuestion
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<OnboardingOption> Options { get; set; } = new List<OnboardingOption>();
}

public class OnboardingOption
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public string OptionKey { get; set; } = string.Empty;
    public string OptionText { get; set; } = string.Empty;
    public int AnalyticalScoreDelta { get; set; }
    public int CreativityScoreDelta { get; set; }
    public int SocialScoreDelta { get; set; }
    public int DisciplineScoreDelta { get; set; }
    public int ResilienceScoreDelta { get; set; }
    public int FocusScoreDelta { get; set; }
    public int ConfidenceScoreDelta { get; set; }
    public int ResponsibilityScoreDelta { get; set; }
    public int CuriosityScoreDelta { get; set; }

    public OnboardingQuestion Question { get; set; } = null!;
}

public class OnboardingAttempt
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RecommendedPathId { get; set; }
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public GrowthPath RecommendedPath { get; set; } = null!;
    public ICollection<OnboardingAnswer> Answers { get; set; } = new List<OnboardingAnswer>();
}

public class OnboardingAnswer
{
    public int Id { get; set; }
    public int AttemptId { get; set; }
    public int QuestionId { get; set; }
    public int OptionId { get; set; }

    public OnboardingAttempt Attempt { get; set; } = null!;
    public OnboardingQuestion Question { get; set; } = null!;
    public OnboardingOption Option { get; set; } = null!;
}

public sealed class TodayMissionDto
{
    public int MissionId { get; set; }
    public string MissionTitle { get; set; } = string.Empty;
    public string MissionDescription { get; set; } = string.Empty;
    public int EstimatedTime { get; set; }
    public int Difficulty { get; set; }
    public int RewardXP { get; set; }
    public int RewardCoin { get; set; }
    public string ConnectedSkill { get; set; } = string.Empty;
    public string ConnectedPath { get; set; } = string.Empty;
    public string ReasonForRecommendation { get; set; } = string.Empty;
}

public sealed class DashboardDto
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public bool NeedsOnboarding { get; set; }
    public string ActivePath { get; set; } = string.Empty;
    public string CurrentSkill { get; set; } = string.Empty;
    public int Level { get; set; }
    public int XP { get; set; }
    public int XPToNextLevel { get; set; }
    public int Coin { get; set; }
    public int DailyStreak { get; set; }
    public TodayMissionDto? TodayMission { get; set; }
}

public sealed class MissionSubmitRequest
{
    public int UserId { get; set; }
    public string? TextAnswer { get; set; }
    public string? MediaUrl { get; set; }
}

public sealed class MissionSubmitResultDto
{
    public int SubmissionId { get; set; }
    public MissionSubmissionStatus Status { get; set; }
    public int GrantedXP { get; set; }
    public int GrantedCoin { get; set; }
    public string Message { get; set; } = string.Empty;
}

public sealed class OnboardingQuestionDto
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public List<OnboardingOptionDto> Options { get; set; } = new();
}

public sealed class OnboardingOptionDto
{
    public int Id { get; set; }
    public string OptionKey { get; set; } = string.Empty;
    public string OptionText { get; set; } = string.Empty;
}

public sealed class OnboardingAnswerRequest
{
    public int QuestionId { get; set; }
    public int OptionId { get; set; }
}

public sealed class OnboardingSubmitRequest
{
    public int UserId { get; set; }
    public int? Age { get; set; }
    public string? FullName { get; set; }
    public List<OnboardingAnswerRequest> Answers { get; set; } = new();
}

public sealed class OnboardingResultDto
{
    public int UserId { get; set; }
    public int GrowthProfileId { get; set; }
    public int RecommendedPathId { get; set; }
    public string RecommendedPathTitle { get; set; } = string.Empty;
    public Dictionary<string, int> Scores { get; set; } = new();
}

public sealed class UserPathSelectionRequest
{
    public int UserId { get; set; }
    public int PathId { get; set; }
    public bool MakePrimary { get; set; }
}

public sealed class GrowthPathUpsertRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public PathType PathType { get; set; }
    public string Icon { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

public sealed class SkillUpsertRequest
{
    public int PathId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Order { get; set; }
    public int Level { get; set; } = 1;
    public bool IsCoreSkill { get; set; } = true;
}

public sealed class MissionUpsertRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Goal { get; set; } = string.Empty;
    public int PathId { get; set; }
    public int SkillId { get; set; }
    public MissionType MissionType { get; set; } = MissionType.Practice;
    public int Difficulty { get; set; } = 1;
    public int EstimatedMinutes { get; set; } = 10;
    public RequiredOutputType RequiredOutputType { get; set; } = RequiredOutputType.None;
    public MissionValidationType ValidationType { get; set; } = MissionValidationType.SelfReport;
    public int RewardXP { get; set; } = 10;
    public int RewardCoin { get; set; } = 5;
    public int SkillProgressGain { get; set; } = 20;
    public bool IsActive { get; set; } = true;
}

public sealed class ReviewMissionSubmissionRequest
{
    public bool Approve { get; set; }
    public string? AdminFeedback { get; set; }
}

