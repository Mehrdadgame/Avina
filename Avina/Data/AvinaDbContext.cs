using Avina.Models;
using Microsoft.EntityFrameworkCore;

namespace Avina.Data;

public class AvinaDbContext : DbContext
{
    public AvinaDbContext(DbContextOptions<AvinaDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<UserPurchase> UserPurchases { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    // New tables
    public DbSet<ContentMedia> Contents { get; set; } = null!;
    public DbSet<ContentPurchase> ContentPurchases { get; set; } = null!;
    public DbSet<CourseLesson> CourseLessons { get; set; } = null!;
    public DbSet<UserCourseEnrollment> CourseEnrollments { get; set; } = null!;
    public DbSet<Exam> Exams { get; set; } = null!;
    public DbSet<ExamQuestion> ExamQuestions { get; set; } = null!;
    public DbSet<UserExamResult> ExamResults { get; set; } = null!;
    public DbSet<Certificate> Certificates { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<UserNotificationState> UserNotificationStates { get; set; } = null!;
    public DbSet<HomeBanner> HomeBanners { get; set; } = null!;
    public DbSet<HomeFeaturedItem> HomeFeaturedItems { get; set; } = null!;
    public DbSet<SocialPost> SocialPosts { get; set; } = null!;
    public DbSet<SocialComment> SocialComments { get; set; } = null!;
    public DbSet<SocialPostLike> SocialPostLikes { get; set; } = null!;
    public DbSet<UserFollow> UserFollows { get; set; } = null!;
    public DbSet<DailyChallenge> DailyChallenges { get; set; } = null!;
    public DbSet<DailyChallengeQuestion> DailyChallengeQuestions { get; set; } = null!;
    public DbSet<UserDailyChallengeAttempt> UserDailyChallengeAttempts { get; set; } = null!;
    public DbSet<GrowthProfile> GrowthProfiles { get; set; } = null!;
    public DbSet<GrowthPath> GrowthPaths { get; set; } = null!;
    public DbSet<UserGrowthPath> UserGrowthPaths { get; set; } = null!;
    public DbSet<Skill> Skills { get; set; } = null!;
    public DbSet<SkillDependency> SkillDependencies { get; set; } = null!;
    public DbSet<UserSkillProgress> UserSkillProgresses { get; set; } = null!;
    public DbSet<Mission> Missions { get; set; } = null!;
    public DbSet<MissionSubmission> MissionSubmissions { get; set; } = null!;
    public DbSet<RewardTransaction> RewardTransactions { get; set; } = null!;
    public DbSet<OnboardingQuestion> OnboardingQuestions { get; set; } = null!;
    public DbSet<OnboardingOption> OnboardingOptions { get; set; } = null!;
    public DbSet<OnboardingAttempt> OnboardingAttempts { get; set; } = null!;
    public DbSet<OnboardingAnswer> OnboardingAnswers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Decimal precision
        modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(18, 2);
        modelBuilder.Entity<Course>().Property(c => c.Price).HasPrecision(18, 2);
        modelBuilder.Entity<UserPurchase>().Property(p => p.AmountSpent).HasPrecision(18, 2);

        // User - UserPurchase
        modelBuilder.Entity<UserPurchase>()
            .HasOne(up => up.User).WithMany(u => u.Purchases)
            .HasForeignKey(up => up.UserId).OnDelete(DeleteBehavior.Cascade);

        // Product - UserPurchase
        modelBuilder.Entity<UserPurchase>()
            .HasOne(up => up.Product).WithMany(p => p.UserPurchases)
            .HasForeignKey(up => up.ProductId).OnDelete(DeleteBehavior.Restrict);

        // User - RefreshToken
        modelBuilder.Entity<RefreshToken>()
            .HasOne(rt => rt.User).WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId).OnDelete(DeleteBehavior.Cascade);

        // ContentMedia - Uploader (optional)
        modelBuilder.Entity<ContentMedia>()
            .HasOne(c => c.Uploader).WithMany()
            .HasForeignKey(c => c.UploaderId).OnDelete(DeleteBehavior.SetNull);

        // ContentPurchase
        modelBuilder.Entity<ContentPurchase>()
            .HasOne(cp => cp.User).WithMany()
            .HasForeignKey(cp => cp.UserId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<ContentPurchase>()
            .HasOne(cp => cp.Content).WithMany(c => c.Purchases)
            .HasForeignKey(cp => cp.ContentId).OnDelete(DeleteBehavior.Cascade);

        // CourseLesson - Course
        modelBuilder.Entity<CourseLesson>()
            .HasOne(l => l.Course).WithMany(c => c.Lessons)
            .HasForeignKey(l => l.CourseId).OnDelete(DeleteBehavior.Cascade);

        // UserCourseEnrollment
        modelBuilder.Entity<UserCourseEnrollment>()
            .HasOne(e => e.User).WithMany()
            .HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserCourseEnrollment>()
            .HasOne(e => e.Course).WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserCourseEnrollment>()
            .HasIndex(e => new { e.UserId, e.CourseId }).IsUnique();

        // Exam - Course
        modelBuilder.Entity<Exam>()
            .HasOne(e => e.Course).WithMany(c => c.Exams)
            .HasForeignKey(e => e.CourseId).OnDelete(DeleteBehavior.Cascade);

        // ExamQuestion - Exam
        modelBuilder.Entity<ExamQuestion>()
            .HasOne(q => q.Exam).WithMany(e => e.Questions)
            .HasForeignKey(q => q.ExamId).OnDelete(DeleteBehavior.Cascade);

        // UserExamResult
        modelBuilder.Entity<UserExamResult>()
            .HasOne(r => r.User).WithMany()
            .HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserExamResult>()
            .HasOne(r => r.Exam).WithMany(e => e.Results)
            .HasForeignKey(r => r.ExamId).OnDelete(DeleteBehavior.Cascade);

        // Certificate
        modelBuilder.Entity<Certificate>()
            .HasOne(c => c.User).WithMany()
            .HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Certificate>()
            .HasOne(c => c.Course).WithMany(co => co.Certificates)
            .HasForeignKey(c => c.CourseId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Certificate>()
            .HasIndex(c => c.CertificateNumber).IsUnique();

        // Notifications
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.TargetUser).WithMany()
            .HasForeignKey(n => n.TargetUserId).OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<UserNotificationState>()
            .HasOne(s => s.User).WithMany()
            .HasForeignKey(s => s.UserId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserNotificationState>()
            .HasOne(s => s.Notification).WithMany(n => n.UserStates)
            .HasForeignKey(s => s.NotificationId).OnDelete(DeleteBehavior.Cascade);

        // Indices
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<RefreshToken>().HasIndex(rt => rt.Token).IsUnique();
        modelBuilder.Entity<UserNotificationState>().HasIndex(s => new { s.UserId, s.NotificationId }).IsUnique();
        modelBuilder.Entity<Notification>().HasIndex(n => new { n.IsActive, n.PublishAt, n.ExpireAt });
        modelBuilder.Entity<HomeBanner>().HasIndex(b => new { b.IsActive, b.DisplayOrder });
        modelBuilder.Entity<HomeFeaturedItem>().HasIndex(f => new { f.SectionKey, f.DisplayOrder, f.IsActive });
        modelBuilder.Entity<SocialPost>().Property(p => p.Content).HasMaxLength(4000);
        modelBuilder.Entity<SocialComment>().Property(c => c.Content).HasMaxLength(1000);

        modelBuilder.Entity<SocialPost>()
            .HasOne(p => p.User).WithMany(u => u.SocialPosts)
            .HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SocialComment>()
            .HasOne(c => c.Post).WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<SocialComment>()
            .HasOne(c => c.User).WithMany(u => u.SocialComments)
            .HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SocialPostLike>()
            .HasOne(l => l.Post).WithMany(p => p.Likes)
            .HasForeignKey(l => l.PostId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<SocialPostLike>()
            .HasOne(l => l.User).WithMany(u => u.SocialPostLikes)
            .HasForeignKey(l => l.UserId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<SocialPostLike>()
            .HasIndex(l => new { l.PostId, l.UserId }).IsUnique();
        modelBuilder.Entity<SocialPost>()
            .HasIndex(p => p.CreatedAt);
        modelBuilder.Entity<SocialComment>()
            .HasIndex(c => new { c.PostId, c.CreatedAt });

        modelBuilder.Entity<UserFollow>()
            .HasOne(f => f.Follower).WithMany(u => u.FollowingMap)
            .HasForeignKey(f => f.FollowerId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<UserFollow>()
            .HasOne(f => f.Following).WithMany(u => u.FollowersMap)
            .HasForeignKey(f => f.FollowingId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<UserFollow>()
            .HasIndex(f => new { f.FollowerId, f.FollowingId }).IsUnique();

        modelBuilder.Entity<DailyChallenge>()
            .Property(c => c.Title).HasMaxLength(200);
        modelBuilder.Entity<DailyChallengeQuestion>()
            .Property(q => q.QuestionText).HasMaxLength(1000);
        modelBuilder.Entity<DailyChallenge>()
            .HasIndex(c => new { c.IsActive, c.PublishAt });
        modelBuilder.Entity<DailyChallengeQuestion>()
            .HasOne(q => q.DailyChallenge).WithMany(c => c.Questions)
            .HasForeignKey(q => q.DailyChallengeId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserDailyChallengeAttempt>()
            .HasOne(a => a.User).WithMany()
            .HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserDailyChallengeAttempt>()
            .HasOne(a => a.DailyChallenge).WithMany(c => c.Attempts)
            .HasForeignKey(a => a.DailyChallengeId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserDailyChallengeAttempt>()
            .HasIndex(a => new { a.UserId, a.DailyChallengeId, a.AttemptNumber }).IsUnique();

        modelBuilder.Entity<GrowthProfile>()
            .HasOne(g => g.User)
            .WithOne(u => u.GrowthProfile)
            .HasForeignKey<GrowthProfile>(g => g.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserGrowthPath>()
            .HasOne(up => up.User)
            .WithMany(u => u.GrowthPaths)
            .HasForeignKey(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserGrowthPath>()
            .HasOne(up => up.GrowthPath)
            .WithMany(p => p.UserPaths)
            .HasForeignKey(up => up.GrowthPathId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserGrowthPath>()
            .HasIndex(up => new { up.UserId, up.GrowthPathId })
            .IsUnique();

        modelBuilder.Entity<Skill>()
            .HasOne(s => s.Path)
            .WithMany(p => p.Skills)
            .HasForeignKey(s => s.PathId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Skill>()
            .HasIndex(s => new { s.PathId, s.Order })
            .IsUnique();

        modelBuilder.Entity<SkillDependency>()
            .HasOne(sd => sd.Skill)
            .WithMany(s => s.Dependencies)
            .HasForeignKey(sd => sd.SkillId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<SkillDependency>()
            .HasOne(sd => sd.RequiredSkill)
            .WithMany(s => s.RequiredBy)
            .HasForeignKey(sd => sd.RequiredSkillId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<SkillDependency>()
            .HasIndex(sd => new { sd.SkillId, sd.RequiredSkillId })
            .IsUnique();

        modelBuilder.Entity<UserSkillProgress>()
            .HasOne(sp => sp.User)
            .WithMany(u => u.SkillProgresses)
            .HasForeignKey(sp => sp.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserSkillProgress>()
            .HasOne(sp => sp.Skill)
            .WithMany(s => s.UserProgresses)
            .HasForeignKey(sp => sp.SkillId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserSkillProgress>()
            .HasIndex(sp => new { sp.UserId, sp.SkillId })
            .IsUnique();

        modelBuilder.Entity<Mission>()
            .HasOne(m => m.Path)
            .WithMany(p => p.Missions)
            .HasForeignKey(m => m.PathId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Mission>()
            .HasOne(m => m.Skill)
            .WithMany(s => s.Missions)
            .HasForeignKey(m => m.SkillId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Mission>()
            .HasIndex(m => new { m.PathId, m.SkillId, m.Difficulty, m.IsActive });

        modelBuilder.Entity<MissionSubmission>()
            .HasOne(ms => ms.User)
            .WithMany(u => u.MissionSubmissions)
            .HasForeignKey(ms => ms.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<MissionSubmission>()
            .HasOne(ms => ms.Mission)
            .WithMany(m => m.Submissions)
            .HasForeignKey(ms => ms.MissionId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<MissionSubmission>()
            .HasIndex(ms => new { ms.UserId, ms.SubmittedAt });

        modelBuilder.Entity<RewardTransaction>()
            .HasOne(rt => rt.User)
            .WithMany(u => u.RewardTransactions)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<RewardTransaction>()
            .HasIndex(rt => new { rt.UserId, rt.SourceType, rt.SourceId });

        modelBuilder.Entity<OnboardingQuestion>()
            .HasIndex(q => new { q.IsActive, q.DisplayOrder });
        modelBuilder.Entity<OnboardingOption>()
            .HasOne(o => o.Question)
            .WithMany(q => q.Options)
            .HasForeignKey(o => o.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<OnboardingAttempt>()
            .HasOne(a => a.User)
            .WithMany(u => u.OnboardingAttempts)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<OnboardingAttempt>()
            .HasOne(a => a.RecommendedPath)
            .WithMany()
            .HasForeignKey(a => a.RecommendedPathId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<OnboardingAnswer>()
            .HasOne(a => a.Attempt)
            .WithMany(a => a.Answers)
            .HasForeignKey(a => a.AttemptId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<OnboardingAnswer>()
            .HasOne(a => a.Question)
            .WithMany()
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<OnboardingAnswer>()
            .HasOne(a => a.Option)
            .WithMany()
            .HasForeignKey(a => a.OptionId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<OnboardingAnswer>()
            .HasIndex(a => new { a.AttemptId, a.QuestionId })
            .IsUnique();

        SeedData(modelBuilder);
        SeedGrowthEngineData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        var now = new DateTime(2026, 4, 18, 12, 0, 0);

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "کتاب: شروع برنامه‌نویسی", Description = "یک کتاب جامع برای یادگیری برنامه‌نویسی از صفر", Category = "کتاب", Type = "Digital", CoinPrice = 500, Price = 15, PreviewPages = 2, FilePath = "/uploads/books/programming-intro.pdf", IsNew = true, Stock = 9999, CreatedAt = now },
            new Product { Id = 2, Name = "پادکست: مسیریابی شغلی", Description = "پادکست هفتگی درباره انتخاب شغل و رشد حرفه‌ای", Category = "پادکست", Type = "Podcast", CoinPrice = 100, Price = 5, FilePath = "/uploads/podcasts/career-guidance.mp3", IsNew = true, Stock = 9999, CreatedAt = now },
            new Product { Id = 3, Name = "کیت مهارت: ساخت رباتیک", Description = "کیت فیزیکی برای یادگیری رباتیک و الکترونیک", Category = "کیت مهارتی", Type = "Physical", Price = 50, CoinPrice = 1000, IsNew = true, Stock = 25, CreatedAt = now }
        );

        modelBuilder.Entity<Course>().HasData(
            new Course { Id = 1, Title = "دوره برنامه‌نویسی با پایتون", Description = "یادگیری پایتون از صفر تا پیشرفته با پروژه‌های واقعی", Instructor = "علی محمدی", Category = "برنامه‌نویسی", Level = "مقدماتی", CoinPrice = 800, DurationMinutes = 600, StudentCount = 1240, Rating = 4.8, RatingCount = 320, IsPublished = true, CreatedAt = now, WhatYouLearn = "مفاهیم پایه پایتون|کار با داده‌ها|پروژه‌های عملی", Requirements = "هیچ پیش‌نیازی لازم نیست" },
            new Course { Id = 2, Title = "طراحی UI/UX حرفه‌ای", Description = "اصول طراحی رابط کاربری و تجربه کاربری با Figma", Instructor = "سارا کریمی", Category = "طراحی", Level = "متوسط", CoinPrice = 600, DurationMinutes = 480, StudentCount = 856, Rating = 4.6, RatingCount = 210, IsPublished = true, CreatedAt = now, WhatYouLearn = "اصول طراحی|کار با Figma|پروتوتایپ", Requirements = "آشنایی با کامپیوتر" },
            new Course { Id = 3, Title = "مدیریت کسب و کار دیجیتال", Description = "راه‌اندازی و مدیریت کسب و کار در فضای دیجیتال", Instructor = "رضا احمدی", Category = "کسب و کار", Level = "مقدماتی", IsFree = true, DurationMinutes = 300, StudentCount = 2100, Rating = 4.5, RatingCount = 450, IsPublished = true, CreatedAt = now, WhatYouLearn = "استراتژی دیجیتال|بازاریابی آنلاین|مدیریت تیم", Requirements = "علاقه به کسب و کار" }
        );

        modelBuilder.Entity<CourseLesson>().HasData(
            new CourseLesson { Id = 1, CourseId = 1, Title = "معرفی پایتون و نصب محیط", Order = 1, IsFreePreview = true, DurationSeconds = 1200, CreatedAt = now },
            new CourseLesson { Id = 2, CourseId = 1, Title = "متغیرها و انواع داده", Order = 2, IsFreePreview = true, DurationSeconds = 1800, CreatedAt = now },
            new CourseLesson { Id = 3, CourseId = 1, Title = "شرط‌ها و حلقه‌ها", Order = 3, IsFreePreview = false, DurationSeconds = 2400, CreatedAt = now },
            new CourseLesson { Id = 4, CourseId = 1, Title = "توابع و ماژول‌ها", Order = 4, IsFreePreview = false, DurationSeconds = 2100, CreatedAt = now },
            new CourseLesson { Id = 5, CourseId = 2, Title = "اصول اولیه طراحی", Order = 1, IsFreePreview = true, DurationSeconds = 1500, CreatedAt = now },
            new CourseLesson { Id = 6, CourseId = 2, Title = "آشنایی با Figma", Order = 2, IsFreePreview = true, DurationSeconds = 1800, CreatedAt = now }
        );

        modelBuilder.Entity<Exam>().HasData(
            new Exam { Id = 1, CourseId = 1, Title = "آزمون پایانی پایتون", PassingScore = 70, TimeLimitMinutes = 30, CreatedAt = now },
            new Exam { Id = 2, CourseId = 2, Title = "آزمون UI/UX", PassingScore = 65, TimeLimitMinutes = 25, CreatedAt = now }
        );

        modelBuilder.Entity<ExamQuestion>().HasData(
            new ExamQuestion { Id = 1, ExamId = 1, Order = 1, QuestionText = "کدام دستور برای چاپ در پایتون استفاده می‌شود؟", OptionA = "echo()", OptionB = "print()", OptionC = "console.log()", OptionD = "write()", CorrectAnswer = "B", Points = 1 },
            new ExamQuestion { Id = 2, ExamId = 1, Order = 2, QuestionText = "در پایتون، برای تعریف یک تابع از کدام کلمه کلیدی استفاده می‌شود؟", OptionA = "function", OptionB = "func", OptionC = "def", OptionD = "fn", CorrectAnswer = "C", Points = 1 },
            new ExamQuestion { Id = 3, ExamId = 1, Order = 3, QuestionText = "کدام نوع داده در پایتون برای ذخیره متن استفاده می‌شود؟", OptionA = "int", OptionB = "float", OptionC = "bool", OptionD = "str", CorrectAnswer = "D", Points = 1 },
            new ExamQuestion { Id = 4, ExamId = 2, Order = 1, QuestionText = "UX مخفف چیست؟", OptionA = "User Experience", OptionB = "User Extension", OptionC = "Unique Experience", OptionD = "Universal Exchange", CorrectAnswer = "A", Points = 1 },
            new ExamQuestion { Id = 5, ExamId = 2, Order = 2, QuestionText = "در طراحی UI، کدام اصل به سادگی رابط کاربری اشاره دارد؟", OptionA = "Complexity", OptionB = "KISS", OptionC = "BOLD", OptionD = "DEEP", CorrectAnswer = "B", Points = 1 }
        );

        modelBuilder.Entity<ContentMedia>().HasData(
            new ContentMedia { Id = 1, Title = "مقدمه‌ای بر هوش مصنوعی", Description = "آشنایی با مفاهیم پایه هوش مصنوعی و یادگیری ماشین", Type = ContentMediaType.Video, FilePath = "/uploads/videos/ai-intro.mp4", ThumbnailPath = "/uploads/thumbs/ai-intro.jpg", IsFree = true, Category = "فناوری", Tags = "هوش مصنوعی,یادگیری ماشین", ViewCount = 1520, MonthlyViews = 340, WeeklyViews = 85, IsPublished = true, CreatedAt = now, UpdatedAt = now },
            new ContentMedia { Id = 2, Title = "راهنمای مطالعه مؤثر", Description = "تکنیک‌های علمی برای مطالعه بهتر و به خاطر سپردن بیشتر", Type = ContentMediaType.PDF, FilePath = "/uploads/pdfs/study-guide.pdf", ThumbnailPath = "/uploads/thumbs/study.jpg", IsFree = false, CoinPrice = 200, Category = "مهارت‌های زندگی", Tags = "مطالعه,یادگیری", ViewCount = 890, MonthlyViews = 210, WeeklyViews = 52, PageCount = 45, IsPublished = true, CreatedAt = now, UpdatedAt = now },
            new ContentMedia { Id = 3, Title = "پادکست: مدیریت زمان", Description = "اصول و تکنیک‌های مدیریت زمان برای افراد پرمشغله", Type = ContentMediaType.Audio, FilePath = "/uploads/audio/time-management.mp3", IsFree = false, CoinPrice = 150, Category = "مهارت‌های زندگی", Tags = "مدیریت زمان,بهره‌وری", ViewCount = 650, MonthlyViews = 180, WeeklyViews = 41, DurationSeconds = 2700, IsPublished = true, CreatedAt = now, UpdatedAt = now },
            new ContentMedia { Id = 4, Title = "اینفوگرافیک: تغذیه سالم", Description = "راهنمای تصویری برای داشتن تغذیه سالم و متعادل", Type = ContentMediaType.Image, FilePath = "/uploads/images/healthy-food.jpg", IsFree = true, Category = "سلامت", Tags = "تغذیه,سلامت", ViewCount = 2100, MonthlyViews = 520, WeeklyViews = 130, IsPublished = true, CreatedAt = now, UpdatedAt = now },
            new ContentMedia { Id = 5, Title = "دوره فتوشاپ پیشرفته", Description = "آموزش کامل ابزارهای حرفه‌ای فتوشاپ", Type = ContentMediaType.Video, FilePath = "/uploads/videos/photoshop-pro.mp4", ThumbnailPath = "/uploads/thumbs/photoshop.jpg", IsFree = false, CoinPrice = 500, Category = "طراحی", Tags = "فتوشاپ,طراحی گرافیک", ViewCount = 1230, PurchaseCount = 180, MonthlyViews = 290, WeeklyViews = 68, IsPublished = true, CreatedAt = now.AddDays(-2), UpdatedAt = now },
            new ContentMedia { Id = 6, Title = "آموزش CSS Grid و Flexbox", Type = ContentMediaType.Video, FilePath = "/uploads/videos/css-layout.mp4", ThumbnailPath = "/uploads/thumbs/css.jpg", IsFree = true, Category = "برنامه‌نویسی", Tags = "CSS,طراحی وب", ViewCount = 890, MonthlyViews = 220, WeeklyViews = 55, IsPublished = true, CreatedAt = now, UpdatedAt = now },
            new ContentMedia { Id = 7, Title = "کتاب: اصول اقتصاد خرد", Type = ContentMediaType.PDF, FilePath = "/uploads/pdfs/micro-econ.pdf", IsFree = false, CoinPrice = 300, Category = "علوم اجتماعی", Tags = "اقتصاد,دانشگاه", ViewCount = 340, MonthlyViews = 90, WeeklyViews = 22, PageCount = 120, IsPublished = true, CreatedAt = now, UpdatedAt = now },
            new ContentMedia { Id = 8, Title = "پادکست: تاریخ ایران باستان", Type = ContentMediaType.Audio, FilePath = "/uploads/audio/iran-history.mp3", IsFree = true, Category = "تاریخ", Tags = "ایران,تاریخ", ViewCount = 1200, MonthlyViews = 310, WeeklyViews = 78, DurationSeconds = 3600, IsPublished = true, CreatedAt = now, UpdatedAt = now },
            new ContentMedia { Id = 9, Title = "اینفوگرافیک: سیستم شمسی", Type = ContentMediaType.Image, FilePath = "/uploads/images/solar-system.jpg", IsFree = true, Category = "علوم", Tags = "نجوم,علوم", ViewCount = 3200, MonthlyViews = 800, WeeklyViews = 190, IsPublished = true, CreatedAt = now, UpdatedAt = now },
            new ContentMedia { Id = 10, Title = "آموزش Python برای مبتدیان", Type = ContentMediaType.Video, FilePath = "/uploads/videos/python-basics.mp4", ThumbnailPath = "/uploads/thumbs/python.jpg", IsFree = false, CoinPrice = 400, Category = "برنامه‌نویسی", Tags = "پایتون,کدنویسی", ViewCount = 2100, PurchaseCount = 320, MonthlyViews = 530, WeeklyViews = 125, IsPublished = true, CreatedAt = now, UpdatedAt = now },
            new ContentMedia { Id = 11, Title = "راهنمای یادگیری زبان انگلیسی", Type = ContentMediaType.PDF, FilePath = "/uploads/pdfs/english-guide.pdf", IsFree = false, CoinPrice = 150, Category = "زبان", Tags = "انگلیسی,یادگیری", ViewCount = 670, PurchaseCount = 95, MonthlyViews = 175, WeeklyViews = 42, PageCount = 68, IsPublished = true, CreatedAt = now, UpdatedAt = now },
            new ContentMedia { Id = 12, Title = "موسیقی آرامش‌بخش برای مطالعه", Type = ContentMediaType.Audio, FilePath = "/uploads/audio/study-music.mp3", IsFree = true, Category = "موسیقی", Tags = "موسیقی,مطالعه", ViewCount = 4500, MonthlyViews = 1200, WeeklyViews = 290, DurationSeconds = 5400, IsPublished = true, CreatedAt = now, UpdatedAt = now },
            new ContentMedia { Id = 13, Title = "نقشه ذهنی: مدیریت پروژه", Type = ContentMediaType.Image, FilePath = "/uploads/images/project-mgmt.jpg", IsFree = false, CoinPrice = 100, Category = "کسب و کار", Tags = "مدیریت,پروژه", ViewCount = 780, PurchaseCount = 55, MonthlyViews = 198, WeeklyViews = 48, IsPublished = true, CreatedAt = now, UpdatedAt = now },
            new ContentMedia { Id = 14, Title = "دوره کامل JavaScript ES2024", Type = ContentMediaType.Video, FilePath = "/uploads/videos/js-advanced.mp4", ThumbnailPath = "/uploads/thumbs/js.jpg", IsFree = false, CoinPrice = 600, Category = "برنامه‌نویسی", Tags = "جاوااسکریپت,وب", ViewCount = 1560, PurchaseCount = 240, MonthlyViews = 390, WeeklyViews = 95, IsPublished = true, CreatedAt = now, UpdatedAt = now },
            new ContentMedia { Id = 15, Title = "تمرینات یوگا برای مبتدیان", Type = ContentMediaType.Video, FilePath = "/uploads/videos/yoga-beginner.mp4", ThumbnailPath = "/uploads/thumbs/yoga.jpg", IsFree = true, Category = "سلامت", Tags = "یوگا,ورزش", ViewCount = 2800, MonthlyViews = 700, WeeklyViews = 175, IsPublished = true, CreatedAt = now, UpdatedAt = now },
            new ContentMedia { Id = 16, Title = "کتاب: داستان کوتاه فارسی", Type = ContentMediaType.PDF, FilePath = "/uploads/pdfs/persian-stories.pdf", IsFree = true, Category = "ادبیات", Tags = "داستان,ادبیات فارسی", ViewCount = 1100, MonthlyViews = 280, WeeklyViews = 68, PageCount = 85, IsPublished = true, CreatedAt = now, UpdatedAt = now },
            new ContentMedia { Id = 17, Title = "پادکست: کارآفرینی در ایران", Type = ContentMediaType.Audio, FilePath = "/uploads/audio/entrepreneurship.mp3", IsFree = false, CoinPrice = 180, Category = "کسب و کار", Tags = "کارآفرینی,بیزنس", ViewCount = 920, PurchaseCount = 78, MonthlyViews = 230, WeeklyViews = 58, DurationSeconds = 2400, IsPublished = true, CreatedAt = now, UpdatedAt = now },
            new ContentMedia { Id = 18, Title = "آموزش طراحی لوگو با Figma", Type = ContentMediaType.Video, FilePath = "/uploads/videos/logo-design.mp4", ThumbnailPath = "/uploads/thumbs/figma.jpg", IsFree = false, CoinPrice = 350, Category = "طراحی", Tags = "فیگما,لوگو,طراحی گرافیک", ViewCount = 1340, PurchaseCount = 168, MonthlyViews = 335, WeeklyViews = 84, IsPublished = true, CreatedAt = now, UpdatedAt = now }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 4, Name = "دوره ویدیویی: ریاضی کنکور", Description = "آموزش کامل ریاضی کنکور با تمرین‌های حل‌شده", Category = "دوره ویدیویی", Type = "Digital", CoinPrice = 800, Price = 25, IsNew = true, Stock = 9999, FilePath = "/uploads/videos/konkoor-math.mp4", CreatedAt = now },
            new Product { Id = 5, Name = "بازی فکری: شطرنج دیجیتال", Description = "شطرنج تعاملی با ۵۰۰ پازل و آموزش استراتژی", Category = "بازی", Type = "Digital", CoinPrice = 200, Price = 8, IsNew = false, Stock = 9999, CreatedAt = now },
            new Product { Id = 6, Name = "کتاب: فیزیک پایه دانشگاه", Description = "کتاب فیزیک عمومی با مثال‌های حل‌شده و آزمون", Category = "کتاب", Type = "Digital", CoinPrice = 400, Price = 12, IsNew = false, Stock = 9999, FilePath = "/uploads/pdfs/physics-uni.pdf", CreatedAt = now },
            new Product { Id = 7, Name = "کیت رباتیک پیشرفته", Description = "کیت ساخت ربات‌های هوشمند با آموزش برنامه‌نویسی Arduino", Category = "کیت مهارتی", Type = "Physical", CoinPrice = 1500, Price = 75, IsNew = false, Stock = 20, CreatedAt = now },
            new Product { Id = 8, Name = "پک آموزشی زبان انگلیسی", Description = "مجموعه کامل آموزش زبان انگلیسی از مقدماتی تا پیشرفته", Category = "پادکست", Type = "Digital", CoinPrice = 600, Price = 18, IsNew = false, Stock = 9999, CreatedAt = now }
        );

        modelBuilder.Entity<Course>().HasData(
            new Course { Id = 4, Title = "زبان انگلیسی کاربردی", Description = "آموزش مکالمه و گرامر زبان انگلیسی برای زندگی روزمره", Instructor = "مریم حسینی", Category = "زبان", Level = "مقدماتی", CoinPrice = 500, DurationMinutes = 420, StudentCount = 1680, Rating = 4.7, RatingCount = 380, IsPublished = true, CreatedAt = now, WhatYouLearn = "گرامر پایه|مکالمه روزمره|دستور زبان|لغات کاربردی", Requirements = "هیچ پیش‌نیازی لازم نیست" },
            new Course { Id = 5, Title = "هنر نقاشی دیجیتال", Description = "آموزش کامل نقاشی دیجیتال با Procreate و Adobe Fresco", Instructor = "علی رضاپور", Category = "هنر", Level = "متوسط", CoinPrice = 700, DurationMinutes = 360, StudentCount = 920, Rating = 4.9, RatingCount = 185, IsPublished = true, CreatedAt = now, WhatYouLearn = "ابزارهای Procreate|تکنیک‌های رنگ‌آمیزی|طراحی کاراکتر|صادرات فایل", Requirements = "آشنایی پایه با تبلت" },
            new Course { Id = 6, Title = "بازاریابی دیجیتال مدرن", Description = "استراتژی‌های بازاریابی آنلاین، SEO و شبکه‌های اجتماعی", Instructor = "نیلوفر کاظمی", Category = "کسب و کار", Level = "متوسط", IsFree = false, CoinPrice = 900, DurationMinutes = 540, StudentCount = 2340, Rating = 4.6, RatingCount = 512, IsPublished = true, CreatedAt = now, WhatYouLearn = "استراتژی محتوا|SEO و سئو|اینستاگرام مارکتینگ|تبلیغات گوگل", Requirements = "آشنایی با اینترنت" }
        );

        modelBuilder.Entity<CourseLesson>().HasData(
            new CourseLesson { Id = 7, CourseId = 4, Title = "معرفی دوره و اهداف", Order = 1, IsFreePreview = true, DurationSeconds = 900, CreatedAt = now },
            new CourseLesson { Id = 8, CourseId = 4, Title = "گرامر پایه: فعل To Be", Order = 2, IsFreePreview = true, DurationSeconds = 1800, CreatedAt = now },
            new CourseLesson { Id = 9, CourseId = 4, Title = "مکالمه روزمره: معرفی خود", Order = 3, IsFreePreview = false, DurationSeconds = 2100, CreatedAt = now },
            new CourseLesson { Id = 10, CourseId = 5, Title = "آشنایی با Procreate", Order = 1, IsFreePreview = true, DurationSeconds = 1500, CreatedAt = now },
            new CourseLesson { Id = 11, CourseId = 5, Title = "لایه‌بندی و ابزارها", Order = 2, IsFreePreview = false, DurationSeconds = 2400, CreatedAt = now },
            new CourseLesson { Id = 12, CourseId = 5, Title = "طراحی اول کاراکتر", Order = 3, IsFreePreview = false, DurationSeconds = 3000, CreatedAt = now }
        );

        modelBuilder.Entity<Exam>().HasData(
            new Exam { Id = 3, CourseId = 4, Title = "آزمون زبان انگلیسی", PassingScore = 60, TimeLimitMinutes = 20, CreatedAt = now }
        );

        modelBuilder.Entity<ExamQuestion>().HasData(
            new ExamQuestion { Id = 6, ExamId = 3, Order = 1, QuestionText = "فعل 'To Be' در جمله 'She ___ a teacher' کدام است؟", OptionA = "am", OptionB = "is", OptionC = "are", OptionD = "be", CorrectAnswer = "B", Points = 1 },
            new ExamQuestion { Id = 7, ExamId = 3, Order = 2, QuestionText = "ترجمه صحیح 'Good morning' چیست؟", OptionA = "شب بخیر", OptionB = "عصر بخیر", OptionC = "صبح بخیر", OptionD = "خداحافظ", CorrectAnswer = "C", Points = 1 },
            new ExamQuestion { Id = 8, ExamId = 3, Order = 3, QuestionText = "کدام جمله صحیح است؟", OptionA = "I am go to school", OptionB = "I goes to school", OptionC = "I go to school", OptionD = "I going to school", CorrectAnswer = "C", Points = 1 }
        );

        modelBuilder.Entity<Notification>().HasData(
            new Notification
            {
                Id = 1,
                Title = "تخفیف ویژه فروشگاه",
                Message = "امروز تمام محصولات آموزشی فروشگاه تا ۳۰٪ تخفیف دارند.",
                Type = NotificationType.Discount,
                Icon = "🛍️",
                LinkUrl = "/store",
                LinkLabel = "مشاهده فروشگاه",
                IsBroadcast = true,
                IsActive = true,
                PublishAt = now,
                CreatedAt = now
            },
            new Notification
            {
                Id = 2,
                Title = "دوره جدید در مدرسه آنلاین",
                Message = "دوره جدید «بازاریابی دیجیتال مدرن» منتشر شد. همین حالا شروع کن.",
                Type = NotificationType.School,
                Icon = "🎓",
                LinkUrl = "/school",
                LinkLabel = "رفتن به مدرسه",
                IsBroadcast = true,
                IsActive = true,
                PublishAt = now.AddMinutes(10),
                CreatedAt = now.AddMinutes(10)
            },
            new Notification
            {
                Id = 3,
                Title = "ماموریت روزانه آماده است",
                Message = "چالش امروزت فعال شد؛ با انجامش ۵۰ امتیاز و ۲۰ کوین بگیر.",
                Type = NotificationType.System,
                Icon = "⚡",
                LinkUrl = "/",
                LinkLabel = "مشاهده ماموریت",
                IsBroadcast = true,
                IsActive = true,
                PublishAt = now.AddMinutes(20),
                CreatedAt = now.AddMinutes(20)
            }
        );

        modelBuilder.Entity<HomeBanner>().HasData(
            new HomeBanner
            {
                Id = 1,
                Badge = "🎓 مدرسه آنلاین",
                Title = "ماموریت امروز: راز کتیبه‌های باستانی",
                Description = "با حل این معما ۲۰ امتیاز و ۵ کوین دریافت کن.",
                ImageUrl = "https://images.unsplash.com/photo-1503676260728-1c00da094a0b?w=1600&q=80",
                PrimaryLabel = "شروع ماموریت",
                PrimaryLink = "/school",
                SecondaryLabel = "پروفایل من",
                SecondaryLink = "/profile",
                Stat1 = "1.2M",
                Stat1Label = "دانش‌آموز",
                Stat2 = "42K",
                Stat2Label = "دوره فعال",
                DisplayOrder = 1,
                IsActive = true,
                SelectionMode = HomeBannerSelectionMode.Manual,
                PublishAt = now
            },
            new HomeBanner
            {
                Id = 2,
                Badge = "🔥 خودکار: برترین هفتگی",
                Title = "",
                Description = "",
                ImageUrl = "",
                PrimaryLabel = "مشاهده محتوا",
                PrimaryLink = "/school",
                SecondaryLabel = "جزئیات",
                SecondaryLink = "/",
                DisplayOrder = 2,
                IsActive = true,
                SelectionMode = HomeBannerSelectionMode.Auto,
                AutoSourceType = HomeBannerSourceType.Content,
                AutoStrategy = HomeBannerAutoStrategy.TopWeek,
                PublishAt = now
            },
            new HomeBanner
            {
                Id = 3,
                Badge = "🛍️ فروشگاه",
                Title = "ابزار واقعی برای تمرین مسیر زندگی",
                Description = "محصولات فیزیکی را تهیه کن و با کد QR کوین بگیر.",
                ImageUrl = "https://images.unsplash.com/photo-1522202176988-66273c2fd55f?w=1600&q=80",
                PrimaryLabel = "مشاهده فروشگاه",
                PrimaryLink = "/store",
                SecondaryLabel = "مشاهده محصولات منتخب",
                SecondaryLink = "/store",
                Stat1 = "300+",
                Stat1Label = "محصول",
                Stat2 = "100K+",
                Stat2Label = "کاربر فعال",
                DisplayOrder = 3,
                IsActive = true,
                SelectionMode = HomeBannerSelectionMode.Manual,
                PublishAt = now
            }
        );

        modelBuilder.Entity<HomeFeaturedItem>().HasData(
            new HomeFeaturedItem
            {
                Id = 1,
                SectionKey = "home_curated",
                EntityType = FeaturedEntityType.Content,
                EntityId = 12,
                Badge = "🎧 منتخب امروز",
                DisplayOrder = 1,
                IsActive = true,
                PublishAt = now
            },
            new HomeFeaturedItem
            {
                Id = 2,
                SectionKey = "home_curated",
                EntityType = FeaturedEntityType.Course,
                EntityId = 1,
                Badge = "🎓 کلاس ویژه",
                DisplayOrder = 2,
                IsActive = true,
                PublishAt = now
            },
            new HomeFeaturedItem
            {
                Id = 3,
                SectionKey = "home_curated",
                EntityType = FeaturedEntityType.Product,
                EntityId = 4,
                Badge = "🛍️ پیشنهاد فروشگاه",
                DisplayOrder = 3,
                IsActive = true,
                PublishAt = now
            }
        );

        SeedDailyChallenges(modelBuilder, now);
    }

    private static void SeedGrowthEngineData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GrowthPath>().HasData(
            new GrowthPath { Id = 1, Title = "مسیر تحلیلی", Description = "رشد در منطق، حل مسئله، تحلیل داده و تصمیم‌گیری.", PathType = PathType.Analytical, Icon = "analytics", IsActive = true },
            new GrowthPath { Id = 2, Title = "مسیر خلاق", Description = "رشد در ایده‌پردازی، طراحی، داستان‌گویی و خلق محتوا.", PathType = PathType.Creative, Icon = "palette", IsActive = true },
            new GrowthPath { Id = 3, Title = "مسیر رهبری اجتماعی", Description = "رشد در ارتباط، گفت‌وگو، کار تیمی و رهبری.", PathType = PathType.SocialLeadership, Icon = "groups", IsActive = true },
            new GrowthPath { Id = 4, Title = "مسیر سازنده عملی", Description = "رشد با ساخت پروژه‌های واقعی و تجربه عملی.", PathType = PathType.MakerPractical, Icon = "build", IsActive = true },
            new GrowthPath { Id = 5, Title = "مسیر معنا و مراقبت", Description = "رشد در همدلی، مسئولیت‌پذیری و اثرگذاری اجتماعی.", PathType = PathType.CareMeaning, Icon = "volunteer_activism", IsActive = true }
        );

        modelBuilder.Entity<Skill>().HasData(
            new Skill { Id = 1, PathId = 1, Title = "تمرکز", Description = "حفظ توجه روی یک کار مشخص.", Order = 1, Level = 1, IsCoreSkill = true },
            new Skill { Id = 2, PathId = 1, Title = "مشاهده", Description = "دیدن الگوها و جزئیات مهم.", Order = 2, Level = 1, IsCoreSkill = true },
            new Skill { Id = 3, PathId = 1, Title = "تفکر منطقی", Description = "استدلال منظم و تحلیل علت و معلول.", Order = 3, Level = 2, IsCoreSkill = true },
            new Skill { Id = 4, PathId = 1, Title = "حل مسئله", Description = "شکستن مسائل واقعی و رسیدن به راه‌حل.", Order = 4, Level = 2, IsCoreSkill = true },

            new Skill { Id = 5, PathId = 2, Title = "ایده‌پردازی", Description = "تولید چندین ایده خلاق برای یک مسئله.", Order = 1, Level = 1, IsCoreSkill = true },
            new Skill { Id = 6, PathId = 2, Title = "داستان‌گویی", Description = "بیان ایده با روایت جذاب.", Order = 2, Level = 1, IsCoreSkill = true },
            new Skill { Id = 7, PathId = 2, Title = "اجرای خلاق", Description = "تبدیل ایده به خروجی واقعی.", Order = 3, Level = 2, IsCoreSkill = true },

            new Skill { Id = 8, PathId = 3, Title = "شنیدن فعال", Description = "گوش‌دادن دقیق و بازتاب درست.", Order = 1, Level = 1, IsCoreSkill = true },
            new Skill { Id = 9, PathId = 3, Title = "میانجی‌گری تعارض", Description = "مدیریت اختلاف با گفت‌وگوی امن.", Order = 2, Level = 2, IsCoreSkill = true },
            new Skill { Id = 10, PathId = 3, Title = "رهبری تیم", Description = "هدایت تیم با وضوح و همدلی.", Order = 3, Level = 3, IsCoreSkill = true },

            new Skill { Id = 11, PathId = 4, Title = "ساخت عملی", Description = "ساخت پروژه‌های کوچک کاربردی.", Order = 1, Level = 1, IsCoreSkill = true },
            new Skill { Id = 12, PathId = 4, Title = "تکرار و بهبود", Description = "بهبود خروجی براساس بازخورد.", Order = 2, Level = 2, IsCoreSkill = true },

            new Skill { Id = 13, PathId = 5, Title = "همدلی", Description = "درک احساس و نگاه دیگران.", Order = 1, Level = 1, IsCoreSkill = true },
            new Skill { Id = 14, PathId = 5, Title = "مسئولیت‌پذیری", Description = "پذیرفتن مسئولیت اثر رفتار خود.", Order = 2, Level = 2, IsCoreSkill = true },
            new Skill { Id = 15, PathId = 5, Title = "حمایت اجتماعی", Description = "کمک موثر به جامعه و اطرافیان.", Order = 3, Level = 3, IsCoreSkill = true }
        );

        modelBuilder.Entity<SkillDependency>().HasData(
            new SkillDependency { Id = 1, SkillId = 2, RequiredSkillId = 1 },
            new SkillDependency { Id = 2, SkillId = 3, RequiredSkillId = 2 },
            new SkillDependency { Id = 3, SkillId = 4, RequiredSkillId = 3 },
            new SkillDependency { Id = 4, SkillId = 6, RequiredSkillId = 5 },
            new SkillDependency { Id = 5, SkillId = 7, RequiredSkillId = 6 },
            new SkillDependency { Id = 6, SkillId = 9, RequiredSkillId = 8 },
            new SkillDependency { Id = 7, SkillId = 10, RequiredSkillId = 9 },
            new SkillDependency { Id = 8, SkillId = 12, RequiredSkillId = 11 },
            new SkillDependency { Id = 9, SkillId = 14, RequiredSkillId = 13 },
            new SkillDependency { Id = 10, SkillId = 15, RequiredSkillId = 14 }
        );

        modelBuilder.Entity<Mission>().HasData(
            new Mission { Id = 1, Title = "تمرین تمرکز عمیق", Description = "۲۰ دقیقه بدون موبایل فقط روی یک کار مشخص تمرکز کن.", Goal = "افزایش توان تمرکز", PathId = 1, SkillId = 1, MissionType = MissionType.Practice, Difficulty = 1, EstimatedMinutes = 20, RequiredOutputType = RequiredOutputType.Checkbox, ValidationType = MissionValidationType.SelfReport, RewardXP = 30, RewardCoin = 12, SkillProgressGain = 18, IsActive = true },
            new Mission { Id = 2, Title = "شکار الگو", Description = "در یک کار روزمره سه الگوی تکرارشونده پیدا کن و بنویس.", Goal = "تقویت مشاهده تحلیلی", PathId = 1, SkillId = 2, MissionType = MissionType.Educational, Difficulty = 2, EstimatedMinutes = 15, RequiredOutputType = RequiredOutputType.Text, ValidationType = MissionValidationType.SelfReport, RewardXP = 25, RewardCoin = 8, SkillProgressGain = 15, IsActive = true },
            new Mission { Id = 3, Title = "مرور معمای منطقی", Description = "یک معمای منطقی حل کن و روش حل خودت را توضیح بده.", Goal = "تقویت استدلال منطقی", PathId = 1, SkillId = 3, MissionType = MissionType.Practice, Difficulty = 3, EstimatedMinutes = 25, RequiredOutputType = RequiredOutputType.Text, ValidationType = MissionValidationType.EvidenceRequired, RewardXP = 40, RewardCoin = 18, SkillProgressGain = 20, IsActive = true },
            new Mission { Id = 4, Title = "حل یک مسئله واقعی", Description = "یک مشکل کوچک واقعی انتخاب کن و دو راه‌حل عملی پیشنهاد بده.", Goal = "حل مسئله در دنیای واقعی", PathId = 1, SkillId = 4, MissionType = MissionType.RealWorld, Difficulty = 4, EstimatedMinutes = 35, RequiredOutputType = RequiredOutputType.Text, ValidationType = MissionValidationType.AdminReview, RewardXP = 70, RewardCoin = 35, SkillProgressGain = 28, IsActive = true },

            new Mission { Id = 5, Title = "چالش ۱۰ ایده", Description = "برای یک موضوع ساده، ۱۰ ایده متفاوت بنویس.", Goal = "گسترش خلاقیت", PathId = 2, SkillId = 5, MissionType = MissionType.Creative, Difficulty = 1, EstimatedMinutes = 15, RequiredOutputType = RequiredOutputType.Text, ValidationType = MissionValidationType.SelfReport, RewardXP = 32, RewardCoin = 14, SkillProgressGain = 18, IsActive = true },
            new Mission { Id = 6, Title = "داستان ۶ قاب", Description = "یک داستان کوتاه در ۶ قاب (متن یا تصویر) بساز.", Goal = "تمرین داستان‌گویی", PathId = 2, SkillId = 6, MissionType = MissionType.Creative, Difficulty = 2, EstimatedMinutes = 30, RequiredOutputType = RequiredOutputType.Image, ValidationType = MissionValidationType.EvidenceRequired, RewardXP = 45, RewardCoin = 22, SkillProgressGain = 22, IsActive = true },
            new Mission { Id = 7, Title = "انتشار خروجی خلاق", Description = "یک خروجی خلاق نهایی منتشر کن و بازخورد بگیر.", Goal = "تبدیل ایده به اجرا", PathId = 2, SkillId = 7, MissionType = MissionType.RealWorld, Difficulty = 4, EstimatedMinutes = 40, RequiredOutputType = RequiredOutputType.Image, ValidationType = MissionValidationType.AdminReview, RewardXP = 75, RewardCoin = 36, SkillProgressGain = 30, IsActive = true },

            new Mission { Id = 8, Title = "تمرین شنیدن فعال", Description = "۱۰ دقیقه به حرف یک نفر گوش بده و ۳ نکته کلیدی بنویس.", Goal = "تقویت شنیدن و همدلی", PathId = 3, SkillId = 8, MissionType = MissionType.Social, Difficulty = 1, EstimatedMinutes = 15, RequiredOutputType = RequiredOutputType.Text, ValidationType = MissionValidationType.SelfReport, RewardXP = 35, RewardCoin = 16, SkillProgressGain = 18, IsActive = true },
            new Mission { Id = 9, Title = "نقش‌آفرینی تعارض", Description = "یک موقعیت اختلاف را شبیه‌سازی کن و روش میانجی‌گری‌ات را ثبت کن.", Goal = "تمرین مدیریت تعارض", PathId = 3, SkillId = 9, MissionType = MissionType.Social, Difficulty = 3, EstimatedMinutes = 25, RequiredOutputType = RequiredOutputType.Text, ValidationType = MissionValidationType.EvidenceRequired, RewardXP = 50, RewardCoin = 24, SkillProgressGain = 23, IsActive = true },
            new Mission { Id = 10, Title = "هدایت جلسه کوتاه", Description = "یک گفت‌وگوی ۲۰ دقیقه‌ای تیمی را هدایت کن و یک بازخورد ثبت کن.", Goal = "تقویت رهبری تیم", PathId = 3, SkillId = 10, MissionType = MissionType.RealWorld, Difficulty = 4, EstimatedMinutes = 30, RequiredOutputType = RequiredOutputType.Video, ValidationType = MissionValidationType.AdminReview, RewardXP = 80, RewardCoin = 40, SkillProgressGain = 32, IsActive = true },

            new Mission { Id = 11, Title = "ساخت ابزار کوچک", Description = "یک خروجی عملی کوچک با ابزارهای موجود بساز.", Goal = "اجرای عملی", PathId = 4, SkillId = 11, MissionType = MissionType.Practice, Difficulty = 2, EstimatedMinutes = 35, RequiredOutputType = RequiredOutputType.Image, ValidationType = MissionValidationType.EvidenceRequired, RewardXP = 48, RewardCoin = 20, SkillProgressGain = 22, IsActive = true },
            new Mission { Id = 12, Title = "حلقه بهبود", Description = "خروجی دیروز را براساس یک بازخورد بهبود بده.", Goal = "یادگیری بهبود تدریجی", PathId = 4, SkillId = 12, MissionType = MissionType.LongTerm, Difficulty = 3, EstimatedMinutes = 25, RequiredOutputType = RequiredOutputType.Text, ValidationType = MissionValidationType.AdminReview, RewardXP = 65, RewardCoin = 28, SkillProgressGain = 26, IsActive = true },

            new Mission { Id = 13, Title = "گفت‌وگوی همدلانه", Description = "با یک نفر درباره تجربه سختش گفت‌وگو کن و برداشتت را بنویس.", Goal = "رشد همدلی", PathId = 5, SkillId = 13, MissionType = MissionType.Social, Difficulty = 2, EstimatedMinutes = 20, RequiredOutputType = RequiredOutputType.Text, ValidationType = MissionValidationType.SelfReport, RewardXP = 38, RewardCoin = 18, SkillProgressGain = 20, IsActive = true },
            new Mission { Id = 14, Title = "قول مسئولیت", Description = "یک تعهد مشخص تعریف کن و تا پایان روز انجامش بده.", Goal = "تقویت مسئولیت‌پذیری", PathId = 5, SkillId = 14, MissionType = MissionType.RealWorld, Difficulty = 3, EstimatedMinutes = 20, RequiredOutputType = RequiredOutputType.Text, ValidationType = MissionValidationType.EvidenceRequired, RewardXP = 58, RewardCoin = 27, SkillProgressGain = 24, IsActive = true },
            new Mission { Id = 15, Title = "اقدام حمایت اجتماعی", Description = "یک اقدام واقعی برای کمک به اطرافیان انجام بده و مدرک ثبت کن.", Goal = "اثرگذاری معنادار", PathId = 5, SkillId = 15, MissionType = MissionType.RealWorld, Difficulty = 5, EstimatedMinutes = 45, RequiredOutputType = RequiredOutputType.Image, ValidationType = MissionValidationType.AdminReview, RewardXP = 95, RewardCoin = 45, SkillProgressGain = 35, IsActive = true }
        );

        modelBuilder.Entity<OnboardingQuestion>().HasData(
            new OnboardingQuestion { Id = 1, DisplayOrder = 1, QuestionText = "اگر در یک پروژه گروهی اختلاف پیش بیاید، معمولاً اول چه کاری می‌کنی؟", IsActive = true },
            new OnboardingQuestion { Id = 2, DisplayOrder = 2, QuestionText = "اگر یک بعدازظهر آزاد داشته باشی، کدام فعالیت برایت جذاب‌تر است؟", IsActive = true },
            new OnboardingQuestion { Id = 3, DisplayOrder = 3, QuestionText = "وقتی یک کار سخت می‌شود، واکنش پیش‌فرض تو چیست؟", IsActive = true },
            new OnboardingQuestion { Id = 4, DisplayOrder = 4, QuestionText = "وقتی می‌خواهی موضوع جدیدی یاد بگیری، از کجا شروع می‌کنی؟", IsActive = true },
            new OnboardingQuestion { Id = 5, DisplayOrder = 5, QuestionText = "کدام نتیجه بیشتر باعث احساس افتخار در تو می‌شود؟", IsActive = true },
            new OnboardingQuestion { Id = 6, DisplayOrder = 6, QuestionText = "وقتی دیگران به کمک نیاز دارند، معمولاً چه برخوردی داری؟", IsActive = true },
            new OnboardingQuestion { Id = 7, DisplayOrder = 7, QuestionText = "حواست‌پرتی‌ها را چطور مدیریت می‌کنی؟", IsActive = true },
            new OnboardingQuestion { Id = 8, DisplayOrder = 8, QuestionText = "کدام نوع چالش برای تو انگیزه‌بخش‌تر است؟", IsActive = true }
        );

        modelBuilder.Entity<OnboardingOption>().HasData(
            new OnboardingOption { Id = 1, QuestionId = 1, OptionKey = "A", OptionText = "دلیل اصلی اختلاف را تحلیل می‌کنم", AnalyticalScoreDelta = 3, CuriosityScoreDelta = 1 },
            new OnboardingOption { Id = 2, QuestionId = 1, OptionKey = "B", OptionText = "یک راه‌حل خلاق پیشنهاد می‌دهم", CreativityScoreDelta = 3, ConfidenceScoreDelta = 1 },
            new OnboardingOption { Id = 3, QuestionId = 1, OptionKey = "C", OptionText = "بین اعضا میانجی‌گری می‌کنم", SocialScoreDelta = 3, ConfidenceScoreDelta = 1 },
            new OnboardingOption { Id = 4, QuestionId = 1, OptionKey = "D", OptionText = "کار را تقسیم و مسئولیت‌ها را مشخص می‌کنم", ResponsibilityScoreDelta = 2, DisciplineScoreDelta = 1 },

            new OnboardingOption { Id = 5, QuestionId = 2, OptionKey = "A", OptionText = "حل معما و چالش منطقی", AnalyticalScoreDelta = 2, FocusScoreDelta = 1 },
            new OnboardingOption { Id = 6, QuestionId = 2, OptionKey = "B", OptionText = "طراحی، نوشتن یا تولید محتوای خلاق", CreativityScoreDelta = 3, CuriosityScoreDelta = 1 },
            new OnboardingOption { Id = 7, QuestionId = 2, OptionKey = "C", OptionText = "شرکت در فعالیت گروهی", SocialScoreDelta = 2, ConfidenceScoreDelta = 1 },
            new OnboardingOption { Id = 8, QuestionId = 2, OptionKey = "D", OptionText = "ساخت یک خروجی عملی", ResponsibilityScoreDelta = 2, ResilienceScoreDelta = 1 },

            new OnboardingOption { Id = 9, QuestionId = 3, OptionKey = "A", OptionText = "کار را به قدم‌های کوچک‌تر تقسیم می‌کنم", AnalyticalScoreDelta = 2, DisciplineScoreDelta = 1 },
            new OnboardingOption { Id = 10, QuestionId = 3, OptionKey = "B", OptionText = "روش جدید و متفاوت امتحان می‌کنم", CreativityScoreDelta = 2, CuriosityScoreDelta = 2 },
            new OnboardingOption { Id = 11, QuestionId = 3, OptionKey = "C", OptionText = "از یک دوست یا هم‌تیمی کمک می‌گیرم", SocialScoreDelta = 2, ConfidenceScoreDelta = 1 },
            new OnboardingOption { Id = 12, QuestionId = 3, OptionKey = "D", OptionText = "با استمرار ادامه می‌دهم تا حل شود", ResilienceScoreDelta = 2, DisciplineScoreDelta = 1 },

            new OnboardingOption { Id = 13, QuestionId = 4, OptionKey = "A", OptionText = "اول ساختار و مفاهیم را می‌خوانم", AnalyticalScoreDelta = 3 },
            new OnboardingOption { Id = 14, QuestionId = 4, OptionKey = "B", OptionText = "نمونه‌ها و ایده‌ها را بررسی می‌کنم", CreativityScoreDelta = 2, CuriosityScoreDelta = 1 },
            new OnboardingOption { Id = 15, QuestionId = 4, OptionKey = "C", OptionText = "با دیگران گفت‌وگو می‌کنم", SocialScoreDelta = 2, ConfidenceScoreDelta = 1 },
            new OnboardingOption { Id = 16, QuestionId = 4, OptionKey = "D", OptionText = "یک پروژه عملی کوچک می‌زنم", ResponsibilityScoreDelta = 2, FocusScoreDelta = 1 },

            new OnboardingOption { Id = 17, QuestionId = 5, OptionKey = "A", OptionText = "گرفتن یک تصمیم هوشمندانه", AnalyticalScoreDelta = 2, ConfidenceScoreDelta = 1 },
            new OnboardingOption { Id = 18, QuestionId = 5, OptionKey = "B", OptionText = "خلق یک خروجی متفاوت", CreativityScoreDelta = 3 },
            new OnboardingOption { Id = 19, QuestionId = 5, OptionKey = "C", OptionText = "موفق شدن یک تیم با کمک من", SocialScoreDelta = 2, ResponsibilityScoreDelta = 1 },
            new OnboardingOption { Id = 20, QuestionId = 5, OptionKey = "D", OptionText = "تمام کردن یک تعهد سخت", DisciplineScoreDelta = 2, ResilienceScoreDelta = 1 },

            new OnboardingOption { Id = 21, QuestionId = 6, OptionKey = "A", OptionText = "با تحلیل واضح راهنمایی می‌کنم", AnalyticalScoreDelta = 2, ResponsibilityScoreDelta = 1 },
            new OnboardingOption { Id = 22, QuestionId = 6, OptionKey = "B", OptionText = "ایده خلاق امیدبخش می‌دهم", CreativityScoreDelta = 2, ConfidenceScoreDelta = 1 },
            new OnboardingOption { Id = 23, QuestionId = 6, OptionKey = "C", OptionText = "با دقت گوش می‌دهم و همدلی می‌کنم", SocialScoreDelta = 2, ConfidenceScoreDelta = 1 },
            new OnboardingOption { Id = 24, QuestionId = 6, OptionKey = "D", OptionText = "خودم مسئولیت کمک را می‌گیرم", ResponsibilityScoreDelta = 2, ResilienceScoreDelta = 1 },

            new OnboardingOption { Id = 25, QuestionId = 7, OptionKey = "A", OptionText = "با چک‌لیست و برنامه منظم جلو می‌روم", DisciplineScoreDelta = 2, FocusScoreDelta = 1 },
            new OnboardingOption { Id = 26, QuestionId = 7, OptionKey = "B", OptionText = "محیط را عوض می‌کنم و دوباره شروع می‌کنم", CreativityScoreDelta = 1, ResilienceScoreDelta = 1, FocusScoreDelta = 1 },
            new OnboardingOption { Id = 27, QuestionId = 7, OptionKey = "C", OptionText = "با شریک مطالعه و تعهد مشترک پیش می‌روم", SocialScoreDelta = 1, DisciplineScoreDelta = 1, ConfidenceScoreDelta = 1 },
            new OnboardingOption { Id = 28, QuestionId = 7, OptionKey = "D", OptionText = "تایمر می‌گذارم و تا پایان می‌روم", FocusScoreDelta = 2, ResilienceScoreDelta = 1 },

            new OnboardingOption { Id = 29, QuestionId = 8, OptionKey = "A", OptionText = "چالش داده‌محور و تحلیلی", AnalyticalScoreDelta = 3 },
            new OnboardingOption { Id = 30, QuestionId = 8, OptionKey = "B", OptionText = "چالش پروژه خلاق", CreativityScoreDelta = 3 },
            new OnboardingOption { Id = 31, QuestionId = 8, OptionKey = "C", OptionText = "چالش رهبری اجتماعی", SocialScoreDelta = 3, ConfidenceScoreDelta = 1 },
            new OnboardingOption { Id = 32, QuestionId = 8, OptionKey = "D", OptionText = "چالش عملی دنیای واقعی", ResponsibilityScoreDelta = 2, ResilienceScoreDelta = 1, DisciplineScoreDelta = 1 }
        );
    }

    private static void SeedDailyChallenges(ModelBuilder modelBuilder, DateTime now)
    {
        var challenges = new[]
        {
            new DailyChallenge
            {
                Id = 1,
                Title = "استاد محاسبات",
                Description = "۲۰ سوال ریاضی سطح متوسط را حل کن و اگر بیشتر از ۵۰ درصد درست بزنی، به همان نسبت امتیاز بگیر.",
                Subject = "ریاضی",
                Icon = "🧮",
                BorderColor = "#ffb68c",
                RewardPoints = 20,
                PassingPercentage = 50,
                TimeLimitMinutes = 20,
                IsActive = true,
                PublishAt = now
            },
            new DailyChallenge
            {
                Id = 2,
                Title = "سفیر زبان",
                Description = "۲۰ سوال واژگان و گرامر را جواب بده و مهارت زبانت را به مسیر رشدت اضافه کن.",
                Subject = "زبان",
                Icon = "🌍",
                BorderColor = "#afc7f7",
                RewardPoints = 30,
                PassingPercentage = 50,
                TimeLimitMinutes = 18,
                IsActive = true,
                PublishAt = now
            },
            new DailyChallenge
            {
                Id = 3,
                Title = "دانشمند کوچک",
                Description = "۲۰ سوال علوم و آزمایشگاهی را پاسخ بده و امتیاز علمی جمع کن.",
                Subject = "علوم",
                Icon = "🔬",
                BorderColor = "#aac7ff",
                RewardPoints = 80,
                PassingPercentage = 50,
                TimeLimitMinutes = 22,
                IsActive = true,
                PublishAt = now
            },
            new DailyChallenge
            {
                Id = 4,
                Title = "خواننده ماهر",
                Description = "در ۲۰ سوال ادبیات و درک مطلب شرکت کن و برای مسیرت امتیاز بگیر.",
                Subject = "ادبیات",
                Icon = "📚",
                BorderColor = "#7dd3a8",
                RewardPoints = 50,
                PassingPercentage = 50,
                TimeLimitMinutes = 20,
                IsActive = true,
                PublishAt = now
            }
        };

        modelBuilder.Entity<DailyChallenge>().HasData(challenges);
        modelBuilder.Entity<DailyChallengeQuestion>().HasData(BuildDailyChallengeQuestions());
    }

    private static List<DailyChallengeQuestion> BuildDailyChallengeQuestions()
    {
        var questions = new List<DailyChallengeQuestion>();
        questions.AddRange(BuildChallengeQuestions(
            challengeId: 1,
            idStart: 1,
            subjectLabel: "ریاضی",
            optionA: "۲",
            optionB: "۴",
            optionC: "۶",
            optionD: "۸",
            correctAnswer: "B",
            stems: Enumerable.Range(1, 20).Select(i => $"سوال {i} ریاضی: حاصل ۲ + ۲ کدام است؟")));

        questions.AddRange(BuildChallengeQuestions(
            challengeId: 2,
            idStart: 21,
            subjectLabel: "زبان",
            optionA: "Good morning",
            optionB: "Good night",
            optionC: "Goodbye",
            optionD: "Please sit",
            correctAnswer: "A",
            stems: Enumerable.Range(1, 20).Select(i => $"سوال {i} زبان: ترجمه درست «صبح بخیر» کدام است؟")));

        questions.AddRange(BuildChallengeQuestions(
            challengeId: 3,
            idStart: 41,
            subjectLabel: "علوم",
            optionA: "اکسیژن",
            optionB: "هیدروژن",
            optionC: "نیتروژن",
            optionD: "دی‌اکسید کربن",
            correctAnswer: "A",
            stems: Enumerable.Range(1, 20).Select(i => $"سوال {i} علوم: گازی که برای تنفس انسان لازم است کدام است؟")));

        questions.AddRange(BuildChallengeQuestions(
            challengeId: 4,
            idStart: 61,
            subjectLabel: "ادبیات",
            optionA: "اسم",
            optionB: "فعل",
            optionC: "صفت",
            optionD: "قید",
            correctAnswer: "B",
            stems: Enumerable.Range(1, 20).Select(i => $"سوال {i} ادبیات: در جمله «او نوشت» واژه «نوشت» چه نقشی دارد؟")));

        return questions;
    }

    private static IEnumerable<DailyChallengeQuestion> BuildChallengeQuestions(
        int challengeId,
        int idStart,
        string subjectLabel,
        string optionA,
        string optionB,
        string optionC,
        string optionD,
        string correctAnswer,
        IEnumerable<string> stems)
    {
        var id = idStart;
        var order = 1;
        foreach (var stem in stems)
        {
            yield return new DailyChallengeQuestion
            {
                Id = id++,
                DailyChallengeId = challengeId,
                Order = order++,
                QuestionText = stem,
                OptionA = optionA,
                OptionB = optionB,
                OptionC = optionC,
                OptionD = optionD,
                CorrectAnswer = correctAnswer,
                Points = 1
            };
        }
    }
}
