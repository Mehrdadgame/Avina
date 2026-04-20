using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Avina.Migrations
{
    /// <inheritdoc />
    public partial class AddContentAndCourseSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "Courses",
                newName: "RatingCount");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Instructor",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CoinPrice",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsFree",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Level",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PreviewVideoPath",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Requirements",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhatYouLearn",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    ExamScore = table.Column<int>(type: "int", nullable: false),
                    CertificateNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Certificates_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Certificates_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThumbnailPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFree = table.Column<bool>(type: "bit", nullable: false),
                    CoinPrice = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    DurationSeconds = table.Column<int>(type: "int", nullable: false),
                    PageCount = table.Column<int>(type: "int", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    PurchaseCount = table.Column<int>(type: "int", nullable: false),
                    WeeklyViews = table.Column<int>(type: "int", nullable: false),
                    MonthlyViews = table.Column<int>(type: "int", nullable: false),
                    UploaderId = table.Column<int>(type: "int", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contents_Users_UploaderId",
                        column: x => x.UploaderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CourseEnrollments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    CoinSpent = table.Column<int>(type: "int", nullable: false),
                    ProgressPercent = table.Column<int>(type: "int", nullable: false),
                    EnrolledAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseEnrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseEnrollments_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseEnrollments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseLessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThumbnailPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DurationSeconds = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsFreePreview = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseLessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseLessons_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PassingScore = table.Column<int>(type: "int", nullable: false),
                    TimeLimitMinutes = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exams_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentPurchases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ContentId = table.Column<int>(type: "int", nullable: false),
                    CoinSpent = table.Column<int>(type: "int", nullable: false),
                    PurchasedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentPurchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentPurchases_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentPurchases_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamId = table.Column<int>(type: "int", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamQuestions_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ExamId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Passed = table.Column<bool>(type: "bit", nullable: false),
                    AttemptNumber = table.Column<int>(type: "int", nullable: false),
                    TakenAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamResults_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamResults_Users_UserId",
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
                    { 1, "فناوری", 0, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "آشنایی با مفاهیم پایه هوش مصنوعی و یادگیری ماشین", 0, "/uploads/videos/ai-intro.mp4", 0L, true, true, 340, 0, 0, "هوش مصنوعی,یادگیری ماشین", "/uploads/thumbs/ai-intro.jpg", "مقدمه‌ای بر هوش مصنوعی", 3, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1520, 85 },
                    { 2, "مهارت‌های زندگی", 200, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "تکنیک‌های علمی برای مطالعه بهتر و به خاطر سپردن بیشتر", 0, "/uploads/pdfs/study-guide.pdf", 0L, false, true, 210, 45, 0, "مطالعه,یادگیری", "/uploads/thumbs/study.jpg", "راهنمای مطالعه مؤثر", 1, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 890, 52 },
                    { 3, "مهارت‌های زندگی", 150, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "اصول و تکنیک‌های مدیریت زمان برای افراد پرمشغله", 2700, "/uploads/audio/time-management.mp3", 0L, false, true, 180, 0, 0, "مدیریت زمان,بهره‌وری", null, "پادکست: مدیریت زمان", 2, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 650, 41 },
                    { 4, "سلامت", 0, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "راهنمای تصویری برای داشتن تغذیه سالم و متعادل", 0, "/uploads/images/healthy-food.jpg", 0L, true, true, 520, 0, 0, "تغذیه,سلامت", null, "اینفوگرافیک: تغذیه سالم", 0, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2100, 130 },
                    { 5, "طراحی", 500, new DateTime(2026, 4, 16, 12, 0, 0, 0, DateTimeKind.Unspecified), "آموزش کامل ابزارهای حرفه‌ای فتوشاپ", 0, "/uploads/videos/photoshop-pro.mp4", 0L, false, true, 290, 0, 180, "فتوشاپ,طراحی گرافیک", "/uploads/thumbs/photoshop.jpg", "دوره فتوشاپ پیشرفته", 3, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1230, 68 }
                });

            migrationBuilder.InsertData(
                table: "CourseLessons",
                columns: new[] { "Id", "CourseId", "CreatedAt", "Description", "DurationSeconds", "IsFreePreview", "Order", "ThumbnailPath", "Title", "VideoPath" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1200, true, 1, null, "معرفی پایتون و نصب محیط", null },
                    { 2, 1, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1800, true, 2, null, "متغیرها و انواع داده", null },
                    { 3, 1, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2400, false, 3, null, "شرط‌ها و حلقه‌ها", null },
                    { 4, 1, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2100, false, 4, null, "توابع و ماژول‌ها", null }
                });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Category", "CoinPrice", "CreatedAt", "Description", "DurationMinutes", "IsFree", "IsPublished", "Language", "Level", "PreviewVideoPath", "Rating", "RatingCount", "Requirements", "StudentCount", "Title", "WhatYouLearn" },
                values: new object[] { "برنامه‌نویسی", 800, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "یادگیری پایتون از صفر تا پیشرفته با پروژه‌های واقعی", 600, false, true, "فارسی", "مقدماتی", null, 4.7999999999999998, 320, "هیچ پیش‌نیازی لازم نیست", 1240, "دوره برنامه‌نویسی با پایتون", "مفاهیم پایه پایتون|کار با داده‌ها|پروژه‌های عملی" });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Category", "CoinPrice", "CreatedAt", "Description", "DurationMinutes", "Instructor", "IsFree", "IsPublished", "Language", "Level", "PreviewVideoPath", "Price", "Rating", "RatingCount", "Requirements", "StudentCount", "ThumbnailImage", "Title", "WhatYouLearn" },
                values: new object[,]
                {
                    { 2, "طراحی", 600, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "اصول طراحی رابط کاربری و تجربه کاربری با Figma", 480, "سارا کریمی", false, true, "فارسی", "متوسط", null, 0m, 4.5999999999999996, 210, "آشنایی با کامپیوتر", 856, null, "طراحی UI/UX حرفه‌ای", "اصول طراحی|کار با Figma|پروتوتایپ" },
                    { 3, "کسب و کار", 0, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), "راه‌اندازی و مدیریت کسب و کار در فضای دیجیتال", 300, "رضا احمدی", true, true, "فارسی", "مقدماتی", null, 0m, 4.5, 450, "علاقه به کسب و کار", 2100, null, "مدیریت کسب و کار دیجیتال", "استراتژی دیجیتال|بازاریابی آنلاین|مدیریت تیم" }
                });

            migrationBuilder.InsertData(
                table: "Exams",
                columns: new[] { "Id", "CourseId", "CreatedAt", "PassingScore", "TimeLimitMinutes", "Title" },
                values: new object[] { 1, 1, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), 70, 30, "آزمون پایانی پایتون" });

            migrationBuilder.InsertData(
                table: "CourseLessons",
                columns: new[] { "Id", "CourseId", "CreatedAt", "Description", "DurationSeconds", "IsFreePreview", "Order", "ThumbnailPath", "Title", "VideoPath" },
                values: new object[,]
                {
                    { 5, 2, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1500, true, 1, null, "اصول اولیه طراحی", null },
                    { 6, 2, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 1800, true, 2, null, "آشنایی با Figma", null }
                });

            migrationBuilder.InsertData(
                table: "ExamQuestions",
                columns: new[] { "Id", "CorrectAnswer", "ExamId", "OptionA", "OptionB", "OptionC", "OptionD", "Order", "Points", "QuestionText" },
                values: new object[,]
                {
                    { 1, "B", 1, "echo()", "print()", "console.log()", "write()", 1, 1, "کدام دستور برای چاپ در پایتون استفاده می‌شود؟" },
                    { 2, "C", 1, "function", "func", "def", "fn", 2, 1, "در پایتون، برای تعریف یک تابع از کدام کلمه کلیدی استفاده می‌شود؟" },
                    { 3, "D", 1, "int", "float", "bool", "str", 3, 1, "کدام نوع داده در پایتون برای ذخیره متن استفاده می‌شود؟" }
                });

            migrationBuilder.InsertData(
                table: "Exams",
                columns: new[] { "Id", "CourseId", "CreatedAt", "PassingScore", "TimeLimitMinutes", "Title" },
                values: new object[] { 2, 2, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), 65, 25, "آزمون UI/UX" });

            migrationBuilder.InsertData(
                table: "ExamQuestions",
                columns: new[] { "Id", "CorrectAnswer", "ExamId", "OptionA", "OptionB", "OptionC", "OptionD", "Order", "Points", "QuestionText" },
                values: new object[,]
                {
                    { 4, "A", 2, "User Experience", "User Extension", "Unique Experience", "Universal Exchange", 1, 1, "UX مخفف چیست؟" },
                    { 5, "B", 2, "Complexity", "KISS", "BOLD", "DEEP", 2, 1, "در طراحی UI، کدام اصل به سادگی رابط کاربری اشاره دارد؟" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_CertificateNumber",
                table: "Certificates",
                column: "CertificateNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_CourseId",
                table: "Certificates",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_UserId",
                table: "Certificates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentPurchases_ContentId",
                table: "ContentPurchases",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentPurchases_UserId",
                table: "ContentPurchases",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_UploaderId",
                table: "Contents",
                column: "UploaderId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollments_CourseId",
                table: "CourseEnrollments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollments_UserId_CourseId",
                table: "CourseEnrollments",
                columns: new[] { "UserId", "CourseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseLessons_CourseId",
                table: "CourseLessons",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamQuestions_ExamId",
                table: "ExamQuestions",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_ExamId",
                table: "ExamResults",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_UserId",
                table: "ExamResults",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_CourseId",
                table: "Exams",
                column: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DropTable(
                name: "ContentPurchases");

            migrationBuilder.DropTable(
                name: "CourseEnrollments");

            migrationBuilder.DropTable(
                name: "CourseLessons");

            migrationBuilder.DropTable(
                name: "ExamQuestions");

            migrationBuilder.DropTable(
                name: "ExamResults");

            migrationBuilder.DropTable(
                name: "Contents");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "CoinPrice",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "IsFree",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "PreviewVideoPath",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Requirements",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "WhatYouLearn",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "RatingCount",
                table: "Courses",
                newName: "Duration");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Instructor",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Category", "Description", "Duration", "Rating", "StudentCount", "Title" },
                values: new object[] { "مقدماتی", "یک دوره مقدماتی برای شروع یادگیری تکنولوژی", 0, 0.0, 0, "دوره شروع به فن‌آوری" });
        }
    }
}
