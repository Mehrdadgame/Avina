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

        SeedData(modelBuilder);
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
