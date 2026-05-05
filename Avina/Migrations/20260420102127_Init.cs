using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Avina.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HomeBanners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Badge = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecondaryLabel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondaryLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stat1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stat1Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stat2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stat2Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PublishAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpireAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SelectionMode = table.Column<int>(type: "int", nullable: false),
                    AutoSourceType = table.Column<int>(type: "int", nullable: true),
                    AutoStrategy = table.Column<int>(type: "int", nullable: true),
                    AutoEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeBanners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomeFeaturedItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EntityType = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    TitleOverride = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubtitleOverride = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Badge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrlOverride = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PublishAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpireAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeFeaturedItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    LinkUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkLabel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsBroadcast = table.Column<bool>(type: "bit", nullable: false),
                    TargetUserId = table.Column<int>(type: "int", nullable: true),
                    TargetRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PublishAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpireAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_TargetUserId",
                        column: x => x.TargetUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "UserNotificationStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotificationStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserNotificationStates_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserNotificationStates_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Contents",
                columns: new[] { "Id", "Category", "CoinPrice", "CreatedAt", "Description", "DurationSeconds", "FilePath", "FileSizeBytes", "IsFree", "IsPublished", "MonthlyViews", "PageCount", "PurchaseCount", "Tags", "ThumbnailPath", "Title", "Type", "UpdatedAt", "UploaderId", "ViewCount", "WeeklyViews" },
                values: new object[,]
                {
                    { 6, "برنامه‌نویسی", 0, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "", 0, "/uploads/videos/css-layout.mp4", 0L, true, true, 220, 0, 0, "CSS,طراحی وب", "/uploads/thumbs/css.jpg", "آموزش CSS Grid و Flexbox", 3, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 890, 55 },
                    { 7, "علوم اجتماعی", 300, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "", 0, "/uploads/pdfs/micro-econ.pdf", 0L, false, true, 90, 120, 0, "اقتصاد,دانشگاه", null, "کتاب: اصول اقتصاد خرد", 1, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 340, 22 },
                    { 8, "تاریخ", 0, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "", 3600, "/uploads/audio/iran-history.mp3", 0L, true, true, 310, 0, 0, "ایران,تاریخ", null, "پادکست: تاریخ ایران باستان", 2, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1200, 78 },
                    { 9, "علوم", 0, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "", 0, "/uploads/images/solar-system.jpg", 0L, true, true, 800, 0, 0, "نجوم,علوم", null, "اینفوگرافیک: سیستم شمسی", 0, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3200, 190 },
                    { 10, "برنامه‌نویسی", 400, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "", 0, "/uploads/videos/python-basics.mp4", 0L, false, true, 530, 0, 320, "پایتون,کدنویسی", "/uploads/thumbs/python.jpg", "آموزش Python برای مبتدیان", 3, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2100, 125 },
                    { 11, "زبان", 150, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "", 0, "/uploads/pdfs/english-guide.pdf", 0L, false, true, 175, 68, 95, "انگلیسی,یادگیری", null, "راهنمای یادگیری زبان انگلیسی", 1, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 670, 42 },
                    { 12, "موسیقی", 0, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "", 5400, "/uploads/audio/study-music.mp3", 0L, true, true, 1200, 0, 0, "موسیقی,مطالعه", null, "موسیقی آرامش‌بخش برای مطالعه", 2, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 4500, 290 },
                    { 13, "کسب و کار", 100, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "", 0, "/uploads/images/project-mgmt.jpg", 0L, false, true, 198, 0, 55, "مدیریت,پروژه", null, "نقشه ذهنی: مدیریت پروژه", 0, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 780, 48 },
                    { 14, "برنامه‌نویسی", 600, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "", 0, "/uploads/videos/js-advanced.mp4", 0L, false, true, 390, 0, 240, "جاوااسکریپت,وب", "/uploads/thumbs/js.jpg", "دوره کامل JavaScript ES2024", 3, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1560, 95 },
                    { 15, "سلامت", 0, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "", 0, "/uploads/videos/yoga-beginner.mp4", 0L, true, true, 700, 0, 0, "یوگا,ورزش", "/uploads/thumbs/yoga.jpg", "تمرینات یوگا برای مبتدیان", 3, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2800, 175 },
                    { 16, "ادبیات", 0, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "", 0, "/uploads/pdfs/persian-stories.pdf", 0L, true, true, 280, 85, 0, "داستان,ادبیات فارسی", null, "کتاب: داستان کوتاه فارسی", 1, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1100, 68 },
                    { 17, "کسب و کار", 180, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "", 2400, "/uploads/audio/entrepreneurship.mp3", 0L, false, true, 230, 0, 78, "کارآفرینی,بیزنس", null, "پادکست: کارآفرینی در ایران", 2, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 920, 58 },
                    { 18, "طراحی", 350, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "", 0, "/uploads/videos/logo-design.mp4", 0L, false, true, 335, 0, 168, "فیگما,لوگو,طراحی گرافیک", "/uploads/thumbs/figma.jpg", "آموزش طراحی لوگو با Figma", 3, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1340, 84 }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Category", "CoinPrice", "CreatedAt", "Description", "DurationMinutes", "Instructor", "IsFree", "IsPublished", "Language", "Level", "PreviewVideoPath", "Price", "Rating", "RatingCount", "Requirements", "StudentCount", "ThumbnailImage", "Title", "WhatYouLearn" },
                values: new object[,]
                {
                    { 4, "زبان", 500, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "آموزش مکالمه و گرامر زبان انگلیسی برای زندگی روزمره", 420, "مریم حسینی", false, true, "فارسی", "مقدماتی", null, 0m, 4.7000000000000002, 380, "هیچ پیش‌نیازی لازم نیست", 1680, null, "زبان انگلیسی کاربردی", "گرامر پایه|مکالمه روزمره|دستور زبان|لغات کاربردی" },
                    { 5, "هنر", 700, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "آموزش کامل نقاشی دیجیتال با Procreate و Adobe Fresco", 360, "علی رضاپور", false, true, "فارسی", "متوسط", null, 0m, 4.9000000000000004, 185, "آشنایی پایه با تبلت", 920, null, "هنر نقاشی دیجیتال", "ابزارهای Procreate|تکنیک‌های رنگ‌آمیزی|طراحی کاراکتر|صادرات فایل" },
                    { 6, "کسب و کار", 900, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "استراتژی‌های بازاریابی آنلاین، SEO و شبکه‌های اجتماعی", 540, "نیلوفر کاظمی", false, true, "فارسی", "متوسط", null, 0m, 4.5999999999999996, 512, "آشنایی با اینترنت", 2340, null, "بازاریابی دیجیتال مدرن", "استراتژی محتوا|SEO و سئو|اینستاگرام مارکتینگ|تبلیغات گوگل" }
                });

            migrationBuilder.InsertData(
                table: "HomeBanners",
                columns: new[] { "Id", "AutoEntityId", "AutoSourceType", "AutoStrategy", "Badge", "Description", "DisplayOrder", "ExpireAt", "ImageUrl", "IsActive", "PrimaryLabel", "PrimaryLink", "PublishAt", "SecondaryLabel", "SecondaryLink", "SelectionMode", "Stat1", "Stat1Label", "Stat2", "Stat2Label", "Title" },
                values: new object[,]
                {
                    { 1, null, null, null, "🎓 مدرسه آنلاین", "با حل این معما ۲۰ امتیاز و ۵ کوین دریافت کن.", 1, null, "https://images.unsplash.com/photo-1503676260728-1c00da094a0b?w=1600&q=80", true, "شروع ماموریت", "/school", new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "پروفایل من", "/profile", 0, "1.2M", "دانش‌آموز", "42K", "دوره فعال", "ماموریت امروز: راز کتیبه‌های باستانی" },
                    { 2, null, 1, 1, "🔥 خودکار: برترین هفتگی", "", 2, null, "", true, "مشاهده محتوا", "/school", new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "جزئیات", "/", 1, null, null, null, null, "" },
                    { 3, null, null, null, "🛍️ فروشگاه", "محصولات فیزیکی را تهیه کن و با کد QR کوین بگیر.", 3, null, "https://images.unsplash.com/photo-1522202176988-66273c2fd55f?w=1600&q=80", true, "مشاهده فروشگاه", "/store", new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "مشاهده محصولات منتخب", "/store", 0, "300+", "محصول", "100K+", "کاربر فعال", "ابزار واقعی برای تمرین مسیر زندگی" }
                });

            migrationBuilder.InsertData(
                table: "HomeFeaturedItems",
                columns: new[] { "Id", "Badge", "DisplayOrder", "EntityId", "EntityType", "ExpireAt", "ImageUrlOverride", "IsActive", "PublishAt", "SectionKey", "SubtitleOverride", "TitleOverride" },
                values: new object[,]
                {
                    { 1, "🎧 منتخب امروز", 1, 12, 1, null, null, true, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "home_curated", null, null },
                    { 2, "🎓 کلاس ویژه", 2, 1, 2, null, null, true, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "home_curated", null, null },
                    { 3, "🛍️ پیشنهاد فروشگاه", 3, 4, 3, null, null, true, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "home_curated", null, null }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "CreatedAt", "ExpireAt", "Icon", "IsActive", "IsBroadcast", "LinkLabel", "LinkUrl", "Message", "PublishAt", "TargetRole", "TargetUserId", "Title", "Type" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "🛍️", true, true, "مشاهده فروشگاه", "/store", "امروز تمام محصولات آموزشی فروشگاه تا ۳۰٪ تخفیف دارند.", new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, null, "تخفیف ویژه فروشگاه", 1 },
                    { 2, new DateTime(2026, 4, 18, 12, 10, 0, 0, DateTimeKind.Unspecified), null, "🎓", true, true, "رفتن به مدرسه", "/school", "دوره جدید «بازاریابی دیجیتال مدرن» منتشر شد. همین حالا شروع کن.", new DateTime(2026, 4, 18, 12, 10, 0, 0, DateTimeKind.Unspecified), null, null, "دوره جدید در مدرسه آنلاین", 3 },
                    { 3, new DateTime(2026, 4, 18, 12, 20, 0, 0, DateTimeKind.Unspecified), null, "⚡", true, true, "مشاهده ماموریت", "/", "چالش امروزت فعال شد؛ با انجامش ۵۰ امتیاز و ۲۰ کوین بگیر.", new DateTime(2026, 4, 18, 12, 20, 0, 0, DateTimeKind.Unspecified), null, null, "ماموریت روزانه آماده است", 4 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "CoinPrice", "CreatedAt", "Description", "FilePath", "Image", "IsActive", "IsNew", "Name", "PreviewPages", "Price", "Stock", "Type" },
                values: new object[,]
                {
                    { 4, "دوره ویدیویی", 800, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "آموزش کامل ریاضی کنکور با تمرین‌های حل‌شده", "/uploads/videos/konkoor-math.mp4", null, true, true, "دوره ویدیویی: ریاضی کنکور", 0, 25m, 9999, "Digital" },
                    { 5, "بازی", 200, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "شطرنج تعاملی با ۵۰۰ پازل و آموزش استراتژی", null, null, true, false, "بازی فکری: شطرنج دیجیتال", 0, 8m, 9999, "Digital" },
                    { 6, "کتاب", 400, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "کتاب فیزیک عمومی با مثال‌های حل‌شده و آزمون", "/uploads/pdfs/physics-uni.pdf", null, true, false, "کتاب: فیزیک پایه دانشگاه", 0, 12m, 9999, "Digital" },
                    { 7, "کیت مهارتی", 1500, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "کیت ساخت ربات‌های هوشمند با آموزش برنامه‌نویسی Arduino", null, null, true, false, "کیت رباتیک پیشرفته", 0, 75m, 20, "Physical" },
                    { 8, "پادکست", 600, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "مجموعه کامل آموزش زبان انگلیسی از مقدماتی تا پیشرفته", null, null, true, false, "پک آموزشی زبان انگلیسی", 0, 18m, 9999, "Digital" }
                });

            migrationBuilder.InsertData(
                table: "CourseLessons",
                columns: new[] { "Id", "CourseId", "CreatedAt", "Description", "DurationSeconds", "IsFreePreview", "Order", "ThumbnailPath", "Title", "VideoPath" },
                values: new object[,]
                {
                    { 7, 4, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 900, true, 1, null, "معرفی دوره و اهداف", null },
                    { 8, 4, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1800, true, 2, null, "گرامر پایه: فعل To Be", null },
                    { 9, 4, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2100, false, 3, null, "مکالمه روزمره: معرفی خود", null },
                    { 10, 5, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1500, true, 1, null, "آشنایی با Procreate", null },
                    { 11, 5, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2400, false, 2, null, "لایه‌بندی و ابزارها", null },
                    { 12, 5, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 3000, false, 3, null, "طراحی اول کاراکتر", null }
                });

            migrationBuilder.InsertData(
                table: "Exams",
                columns: new[] { "Id", "CourseId", "CreatedAt", "PassingScore", "TimeLimitMinutes", "Title" },
                values: new object[] { 3, 4, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), 60, 20, "آزمون زبان انگلیسی" });

            migrationBuilder.InsertData(
                table: "ExamQuestions",
                columns: new[] { "Id", "CorrectAnswer", "ExamId", "OptionA", "OptionB", "OptionC", "OptionD", "Order", "Points", "QuestionText" },
                values: new object[,]
                {
                    { 6, "B", 3, "am", "is", "are", "be", 1, 1, "فعل 'To Be' در جمله 'She ___ a teacher' کدام است؟" },
                    { 7, "C", 3, "شب بخیر", "عصر بخیر", "صبح بخیر", "خداحافظ", 2, 1, "ترجمه صحیح 'Good morning' چیست؟" },
                    { 8, "C", 3, "I am go to school", "I goes to school", "I go to school", "I going to school", 3, 1, "کدام جمله صحیح است؟" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_HomeBanners_IsActive_DisplayOrder",
                table: "HomeBanners",
                columns: new[] { "IsActive", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_HomeFeaturedItems_SectionKey_DisplayOrder_IsActive",
                table: "HomeFeaturedItems",
                columns: new[] { "SectionKey", "DisplayOrder", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_IsActive_PublishAt_ExpireAt",
                table: "Notifications",
                columns: new[] { "IsActive", "PublishAt", "ExpireAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_TargetUserId",
                table: "Notifications",
                column: "TargetUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotificationStates_NotificationId",
                table: "UserNotificationStates",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotificationStates_UserId_NotificationId",
                table: "UserNotificationStates",
                columns: new[] { "UserId", "NotificationId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomeBanners");

            migrationBuilder.DropTable(
                name: "HomeFeaturedItems");

            migrationBuilder.DropTable(
                name: "UserNotificationStates");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DeleteData(
                table: "Contents",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Contents",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Contents",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Contents",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Contents",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Contents",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Contents",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Contents",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Contents",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Contents",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Contents",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Contents",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Contents",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "CourseLessons",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "CourseLessons",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "CourseLessons",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "CourseLessons",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "CourseLessons",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "CourseLessons",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ExamQuestions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ExamQuestions",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ExamQuestions",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Exams",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
