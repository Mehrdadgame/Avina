using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Avina.Migrations
{
    /// <inheritdoc />
    public partial class AddDailyChallengesAndSocialVideo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "SocialPosts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DailyChallenges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BorderColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RewardPoints = table.Column<int>(type: "int", nullable: false),
                    PassingPercentage = table.Column<int>(type: "int", nullable: false),
                    TimeLimitMinutes = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PublishAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyChallenges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyChallengeQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DailyChallengeId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    OptionA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyChallengeQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyChallengeQuestions_DailyChallenges_DailyChallengeId",
                        column: x => x.DailyChallengeId,
                        principalTable: "DailyChallenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDailyChallengeAttempts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DailyChallengeId = table.Column<int>(type: "int", nullable: false),
                    CorrectAnswers = table.Column<int>(type: "int", nullable: false),
                    TotalQuestions = table.Column<int>(type: "int", nullable: false),
                    ScorePercent = table.Column<int>(type: "int", nullable: false),
                    RewardEarned = table.Column<int>(type: "int", nullable: false),
                    Passed = table.Column<bool>(type: "bit", nullable: false),
                    AttemptNumber = table.Column<int>(type: "int", nullable: false),
                    TakenAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDailyChallengeAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDailyChallengeAttempts_DailyChallenges_DailyChallengeId",
                        column: x => x.DailyChallengeId,
                        principalTable: "DailyChallenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDailyChallengeAttempts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DailyChallenges",
                columns: new[] { "Id", "BorderColor", "Description", "Icon", "IsActive", "PassingPercentage", "PublishAt", "RewardPoints", "Subject", "TimeLimitMinutes", "Title" },
                values: new object[,]
                {
                    { 1, "#ffb68c", "۲۰ سوال ریاضی سطح متوسط را حل کن و اگر بیشتر از ۵۰ درصد درست بزنی، به همان نسبت امتیاز بگیر.", "🧮", true, 50, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), 20, "ریاضی", 20, "استاد محاسبات" },
                    { 2, "#afc7f7", "۲۰ سوال واژگان و گرامر را جواب بده و مهارت زبانت را به مسیر رشدت اضافه کن.", "🌍", true, 50, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), 30, "زبان", 18, "سفیر زبان" },
                    { 3, "#aac7ff", "۲۰ سوال علوم و آزمایشگاهی را پاسخ بده و امتیاز علمی جمع کن.", "🔬", true, 50, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), 80, "علوم", 22, "دانشمند کوچک" },
                    { 4, "#7dd3a8", "در ۲۰ سوال ادبیات و درک مطلب شرکت کن و برای مسیرت امتیاز بگیر.", "📚", true, 50, new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), 50, "ادبیات", 20, "خواننده ماهر" }
                });

            migrationBuilder.InsertData(
                table: "DailyChallengeQuestions",
                columns: new[] { "Id", "CorrectAnswer", "DailyChallengeId", "OptionA", "OptionB", "OptionC", "OptionD", "Order", "Points", "QuestionText" },
                values: new object[,]
                {
                    { 1, "B", 1, "۲", "۴", "۶", "۸", 1, 1, "سوال 1 ریاضی: حاصل ۲ + ۲ کدام است؟" },
                    { 2, "B", 1, "۲", "۴", "۶", "۸", 2, 1, "سوال 2 ریاضی: حاصل ۲ + ۲ کدام است؟" },
                    { 3, "B", 1, "۲", "۴", "۶", "۸", 3, 1, "سوال 3 ریاضی: حاصل ۲ + ۲ کدام است؟" },
                    { 4, "B", 1, "۲", "۴", "۶", "۸", 4, 1, "سوال 4 ریاضی: حاصل ۲ + ۲ کدام است؟" },
                    { 5, "B", 1, "۲", "۴", "۶", "۸", 5, 1, "سوال 5 ریاضی: حاصل ۲ + ۲ کدام است؟" },
                    { 6, "B", 1, "۲", "۴", "۶", "۸", 6, 1, "سوال 6 ریاضی: حاصل ۲ + ۲ کدام است؟" },
                    { 7, "B", 1, "۲", "۴", "۶", "۸", 7, 1, "سوال 7 ریاضی: حاصل ۲ + ۲ کدام است؟" },
                    { 8, "B", 1, "۲", "۴", "۶", "۸", 8, 1, "سوال 8 ریاضی: حاصل ۲ + ۲ کدام است؟" },
                    { 9, "B", 1, "۲", "۴", "۶", "۸", 9, 1, "سوال 9 ریاضی: حاصل ۲ + ۲ کدام است؟" },
                    { 10, "B", 1, "۲", "۴", "۶", "۸", 10, 1, "سوال 10 ریاضی: حاصل ۲ + ۲ کدام است؟" },
                    { 11, "B", 1, "۲", "۴", "۶", "۸", 11, 1, "سوال 11 ریاضی: حاصل ۲ + ۲ کدام است؟" },
                    { 12, "B", 1, "۲", "۴", "۶", "۸", 12, 1, "سوال 12 ریاضی: حاصل ۲ + ۲ کدام است؟" },
                    { 13, "B", 1, "۲", "۴", "۶", "۸", 13, 1, "سوال 13 ریاضی: حاصل ۲ + ۲ کدام است؟" },
                    { 14, "B", 1, "۲", "۴", "۶", "۸", 14, 1, "سوال 14 ریاضی: حاصل ۲ + ۲ کدام است؟" },
                    { 15, "B", 1, "۲", "۴", "۶", "۸", 15, 1, "سوال 15 ریاضی: حاصل ۲ + ۲ کدام است؟" },
                    { 16, "B", 1, "۲", "۴", "۶", "۸", 16, 1, "سوال 16 ریاضی: حاصل ۲ + ۲ کدام است؟" },
                    { 17, "B", 1, "۲", "۴", "۶", "۸", 17, 1, "سوال 17 ریاضی: حاصل ۲ + ۲ کدام است؟" },
                    { 18, "B", 1, "۲", "۴", "۶", "۸", 18, 1, "سوال 18 ریاضی: حاصل ۲ + ۲ کدام است؟" },
                    { 19, "B", 1, "۲", "۴", "۶", "۸", 19, 1, "سوال 19 ریاضی: حاصل ۲ + ۲ کدام است؟" },
                    { 20, "B", 1, "۲", "۴", "۶", "۸", 20, 1, "سوال 20 ریاضی: حاصل ۲ + ۲ کدام است؟" },
                    { 21, "A", 2, "Good morning", "Good night", "Goodbye", "Please sit", 1, 1, "سوال 1 زبان: ترجمه درست «صبح بخیر» کدام است؟" },
                    { 22, "A", 2, "Good morning", "Good night", "Goodbye", "Please sit", 2, 1, "سوال 2 زبان: ترجمه درست «صبح بخیر» کدام است؟" },
                    { 23, "A", 2, "Good morning", "Good night", "Goodbye", "Please sit", 3, 1, "سوال 3 زبان: ترجمه درست «صبح بخیر» کدام است؟" },
                    { 24, "A", 2, "Good morning", "Good night", "Goodbye", "Please sit", 4, 1, "سوال 4 زبان: ترجمه درست «صبح بخیر» کدام است؟" },
                    { 25, "A", 2, "Good morning", "Good night", "Goodbye", "Please sit", 5, 1, "سوال 5 زبان: ترجمه درست «صبح بخیر» کدام است؟" },
                    { 26, "A", 2, "Good morning", "Good night", "Goodbye", "Please sit", 6, 1, "سوال 6 زبان: ترجمه درست «صبح بخیر» کدام است؟" },
                    { 27, "A", 2, "Good morning", "Good night", "Goodbye", "Please sit", 7, 1, "سوال 7 زبان: ترجمه درست «صبح بخیر» کدام است؟" },
                    { 28, "A", 2, "Good morning", "Good night", "Goodbye", "Please sit", 8, 1, "سوال 8 زبان: ترجمه درست «صبح بخیر» کدام است؟" },
                    { 29, "A", 2, "Good morning", "Good night", "Goodbye", "Please sit", 9, 1, "سوال 9 زبان: ترجمه درست «صبح بخیر» کدام است؟" },
                    { 30, "A", 2, "Good morning", "Good night", "Goodbye", "Please sit", 10, 1, "سوال 10 زبان: ترجمه درست «صبح بخیر» کدام است؟" },
                    { 31, "A", 2, "Good morning", "Good night", "Goodbye", "Please sit", 11, 1, "سوال 11 زبان: ترجمه درست «صبح بخیر» کدام است؟" },
                    { 32, "A", 2, "Good morning", "Good night", "Goodbye", "Please sit", 12, 1, "سوال 12 زبان: ترجمه درست «صبح بخیر» کدام است؟" },
                    { 33, "A", 2, "Good morning", "Good night", "Goodbye", "Please sit", 13, 1, "سوال 13 زبان: ترجمه درست «صبح بخیر» کدام است؟" },
                    { 34, "A", 2, "Good morning", "Good night", "Goodbye", "Please sit", 14, 1, "سوال 14 زبان: ترجمه درست «صبح بخیر» کدام است؟" },
                    { 35, "A", 2, "Good morning", "Good night", "Goodbye", "Please sit", 15, 1, "سوال 15 زبان: ترجمه درست «صبح بخیر» کدام است؟" },
                    { 36, "A", 2, "Good morning", "Good night", "Goodbye", "Please sit", 16, 1, "سوال 16 زبان: ترجمه درست «صبح بخیر» کدام است؟" },
                    { 37, "A", 2, "Good morning", "Good night", "Goodbye", "Please sit", 17, 1, "سوال 17 زبان: ترجمه درست «صبح بخیر» کدام است؟" },
                    { 38, "A", 2, "Good morning", "Good night", "Goodbye", "Please sit", 18, 1, "سوال 18 زبان: ترجمه درست «صبح بخیر» کدام است؟" },
                    { 39, "A", 2, "Good morning", "Good night", "Goodbye", "Please sit", 19, 1, "سوال 19 زبان: ترجمه درست «صبح بخیر» کدام است؟" },
                    { 40, "A", 2, "Good morning", "Good night", "Goodbye", "Please sit", 20, 1, "سوال 20 زبان: ترجمه درست «صبح بخیر» کدام است؟" },
                    { 41, "A", 3, "اکسیژن", "هیدروژن", "نیتروژن", "دی‌اکسید کربن", 1, 1, "سوال 1 علوم: گازی که برای تنفس انسان لازم است کدام است؟" },
                    { 42, "A", 3, "اکسیژن", "هیدروژن", "نیتروژن", "دی‌اکسید کربن", 2, 1, "سوال 2 علوم: گازی که برای تنفس انسان لازم است کدام است؟" },
                    { 43, "A", 3, "اکسیژن", "هیدروژن", "نیتروژن", "دی‌اکسید کربن", 3, 1, "سوال 3 علوم: گازی که برای تنفس انسان لازم است کدام است؟" },
                    { 44, "A", 3, "اکسیژن", "هیدروژن", "نیتروژن", "دی‌اکسید کربن", 4, 1, "سوال 4 علوم: گازی که برای تنفس انسان لازم است کدام است؟" },
                    { 45, "A", 3, "اکسیژن", "هیدروژن", "نیتروژن", "دی‌اکسید کربن", 5, 1, "سوال 5 علوم: گازی که برای تنفس انسان لازم است کدام است؟" },
                    { 46, "A", 3, "اکسیژن", "هیدروژن", "نیتروژن", "دی‌اکسید کربن", 6, 1, "سوال 6 علوم: گازی که برای تنفس انسان لازم است کدام است؟" },
                    { 47, "A", 3, "اکسیژن", "هیدروژن", "نیتروژن", "دی‌اکسید کربن", 7, 1, "سوال 7 علوم: گازی که برای تنفس انسان لازم است کدام است؟" },
                    { 48, "A", 3, "اکسیژن", "هیدروژن", "نیتروژن", "دی‌اکسید کربن", 8, 1, "سوال 8 علوم: گازی که برای تنفس انسان لازم است کدام است؟" },
                    { 49, "A", 3, "اکسیژن", "هیدروژن", "نیتروژن", "دی‌اکسید کربن", 9, 1, "سوال 9 علوم: گازی که برای تنفس انسان لازم است کدام است؟" },
                    { 50, "A", 3, "اکسیژن", "هیدروژن", "نیتروژن", "دی‌اکسید کربن", 10, 1, "سوال 10 علوم: گازی که برای تنفس انسان لازم است کدام است؟" },
                    { 51, "A", 3, "اکسیژن", "هیدروژن", "نیتروژن", "دی‌اکسید کربن", 11, 1, "سوال 11 علوم: گازی که برای تنفس انسان لازم است کدام است؟" },
                    { 52, "A", 3, "اکسیژن", "هیدروژن", "نیتروژن", "دی‌اکسید کربن", 12, 1, "سوال 12 علوم: گازی که برای تنفس انسان لازم است کدام است؟" },
                    { 53, "A", 3, "اکسیژن", "هیدروژن", "نیتروژن", "دی‌اکسید کربن", 13, 1, "سوال 13 علوم: گازی که برای تنفس انسان لازم است کدام است؟" },
                    { 54, "A", 3, "اکسیژن", "هیدروژن", "نیتروژن", "دی‌اکسید کربن", 14, 1, "سوال 14 علوم: گازی که برای تنفس انسان لازم است کدام است؟" },
                    { 55, "A", 3, "اکسیژن", "هیدروژن", "نیتروژن", "دی‌اکسید کربن", 15, 1, "سوال 15 علوم: گازی که برای تنفس انسان لازم است کدام است؟" },
                    { 56, "A", 3, "اکسیژن", "هیدروژن", "نیتروژن", "دی‌اکسید کربن", 16, 1, "سوال 16 علوم: گازی که برای تنفس انسان لازم است کدام است؟" },
                    { 57, "A", 3, "اکسیژن", "هیدروژن", "نیتروژن", "دی‌اکسید کربن", 17, 1, "سوال 17 علوم: گازی که برای تنفس انسان لازم است کدام است؟" },
                    { 58, "A", 3, "اکسیژن", "هیدروژن", "نیتروژن", "دی‌اکسید کربن", 18, 1, "سوال 18 علوم: گازی که برای تنفس انسان لازم است کدام است؟" },
                    { 59, "A", 3, "اکسیژن", "هیدروژن", "نیتروژن", "دی‌اکسید کربن", 19, 1, "سوال 19 علوم: گازی که برای تنفس انسان لازم است کدام است؟" },
                    { 60, "A", 3, "اکسیژن", "هیدروژن", "نیتروژن", "دی‌اکسید کربن", 20, 1, "سوال 20 علوم: گازی که برای تنفس انسان لازم است کدام است؟" },
                    { 61, "B", 4, "اسم", "فعل", "صفت", "قید", 1, 1, "سوال 1 ادبیات: در جمله «او نوشت» واژه «نوشت» چه نقشی دارد؟" },
                    { 62, "B", 4, "اسم", "فعل", "صفت", "قید", 2, 1, "سوال 2 ادبیات: در جمله «او نوشت» واژه «نوشت» چه نقشی دارد؟" },
                    { 63, "B", 4, "اسم", "فعل", "صفت", "قید", 3, 1, "سوال 3 ادبیات: در جمله «او نوشت» واژه «نوشت» چه نقشی دارد؟" },
                    { 64, "B", 4, "اسم", "فعل", "صفت", "قید", 4, 1, "سوال 4 ادبیات: در جمله «او نوشت» واژه «نوشت» چه نقشی دارد؟" },
                    { 65, "B", 4, "اسم", "فعل", "صفت", "قید", 5, 1, "سوال 5 ادبیات: در جمله «او نوشت» واژه «نوشت» چه نقشی دارد؟" },
                    { 66, "B", 4, "اسم", "فعل", "صفت", "قید", 6, 1, "سوال 6 ادبیات: در جمله «او نوشت» واژه «نوشت» چه نقشی دارد؟" },
                    { 67, "B", 4, "اسم", "فعل", "صفت", "قید", 7, 1, "سوال 7 ادبیات: در جمله «او نوشت» واژه «نوشت» چه نقشی دارد؟" },
                    { 68, "B", 4, "اسم", "فعل", "صفت", "قید", 8, 1, "سوال 8 ادبیات: در جمله «او نوشت» واژه «نوشت» چه نقشی دارد؟" },
                    { 69, "B", 4, "اسم", "فعل", "صفت", "قید", 9, 1, "سوال 9 ادبیات: در جمله «او نوشت» واژه «نوشت» چه نقشی دارد؟" },
                    { 70, "B", 4, "اسم", "فعل", "صفت", "قید", 10, 1, "سوال 10 ادبیات: در جمله «او نوشت» واژه «نوشت» چه نقشی دارد؟" },
                    { 71, "B", 4, "اسم", "فعل", "صفت", "قید", 11, 1, "سوال 11 ادبیات: در جمله «او نوشت» واژه «نوشت» چه نقشی دارد؟" },
                    { 72, "B", 4, "اسم", "فعل", "صفت", "قید", 12, 1, "سوال 12 ادبیات: در جمله «او نوشت» واژه «نوشت» چه نقشی دارد؟" },
                    { 73, "B", 4, "اسم", "فعل", "صفت", "قید", 13, 1, "سوال 13 ادبیات: در جمله «او نوشت» واژه «نوشت» چه نقشی دارد؟" },
                    { 74, "B", 4, "اسم", "فعل", "صفت", "قید", 14, 1, "سوال 14 ادبیات: در جمله «او نوشت» واژه «نوشت» چه نقشی دارد؟" },
                    { 75, "B", 4, "اسم", "فعل", "صفت", "قید", 15, 1, "سوال 15 ادبیات: در جمله «او نوشت» واژه «نوشت» چه نقشی دارد؟" },
                    { 76, "B", 4, "اسم", "فعل", "صفت", "قید", 16, 1, "سوال 16 ادبیات: در جمله «او نوشت» واژه «نوشت» چه نقشی دارد؟" },
                    { 77, "B", 4, "اسم", "فعل", "صفت", "قید", 17, 1, "سوال 17 ادبیات: در جمله «او نوشت» واژه «نوشت» چه نقشی دارد؟" },
                    { 78, "B", 4, "اسم", "فعل", "صفت", "قید", 18, 1, "سوال 18 ادبیات: در جمله «او نوشت» واژه «نوشت» چه نقشی دارد؟" },
                    { 79, "B", 4, "اسم", "فعل", "صفت", "قید", 19, 1, "سوال 19 ادبیات: در جمله «او نوشت» واژه «نوشت» چه نقشی دارد؟" },
                    { 80, "B", 4, "اسم", "فعل", "صفت", "قید", 20, 1, "سوال 20 ادبیات: در جمله «او نوشت» واژه «نوشت» چه نقشی دارد؟" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyChallengeQuestions_DailyChallengeId",
                table: "DailyChallengeQuestions",
                column: "DailyChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyChallenges_IsActive_PublishAt",
                table: "DailyChallenges",
                columns: new[] { "IsActive", "PublishAt" });

            migrationBuilder.CreateIndex(
                name: "IX_UserDailyChallengeAttempts_DailyChallengeId",
                table: "UserDailyChallengeAttempts",
                column: "DailyChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDailyChallengeAttempts_UserId_DailyChallengeId_AttemptNumber",
                table: "UserDailyChallengeAttempts",
                columns: new[] { "UserId", "DailyChallengeId", "AttemptNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyChallengeQuestions");

            migrationBuilder.DropTable(
                name: "UserDailyChallengeAttempts");

            migrationBuilder.DropTable(
                name: "DailyChallenges");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "SocialPosts");
        }
    }
}
