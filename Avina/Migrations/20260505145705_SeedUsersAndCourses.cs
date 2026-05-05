using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Avina.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsersAndCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Seed Users
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Name", "FullName", "Email", "PasswordHash", "PasswordSalt", "Role", "Bio", "ProfileImage", "AvatarUrl", "Coin", "Level", "Experience", "Followers", "Following", "Achievements", "IsActive", "CreatedAt", "LastLoginAt" },
                values: new object[,]
                {
                    {
                        "علی محمدی", "علی محمدی", "ali@avina.ir",
                        "$2a$11$K8Z9qK1mL2nP3oQ4rS5tUuVvWwXxYyZz0123456789abcdefghij",
                        "",
                        "دانش‌آموز", "عاشق یادگیری و کشف دنیای جدید", "images/profiles/user1.jpg", "images/profiles/user1.jpg",
                        2450, 5, 1200, 125, 89, "[]", true,
                        new DateTime(2026, 1, 10, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc)
                    },
                    {
                        "فاطمه احمدی", "فاطمه احمدی", "fateme@avina.ir",
                        "$2a$11$K8Z9qK1mL2nP3oQ4rS5tUuVvWwXxYyZz0123456789abcdefghij",
                        "",
                        "استاد", "مدرس ریاضی و علوم با ۱۰ سال تجربه", "images/profiles/avatar-2.jpg", "images/profiles/avatar-2.jpg",
                        5800, 12, 8500, 980, 45, "[]", true,
                        new DateTime(2025, 8, 15, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 5, 4, 0, 0, 0, DateTimeKind.Utc)
                    },
                    {
                        "رضا حسنی", "رضا حسنی", "reza@avina.ir",
                        "$2a$11$K8Z9qK1mL2nP3oQ4rS5tUuVvWwXxYyZz0123456789abcdefghij",
                        "",
                        "استاد", "مدرس برنامه‌نویسی Python و هوش مصنوعی", "images/profiles/avatar-3.jpg", "images/profiles/avatar-3.jpg",
                        3200, 8, 4200, 650, 120, "[]", true,
                        new DateTime(2025, 11, 3, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 5, 3, 0, 0, 0, DateTimeKind.Utc)
                    },
                    {
                        "زهرا کریمی", "زهرا کریمی", "zahra@avina.ir",
                        "$2a$11$K8Z9qK1mL2nP3oQ4rS5tUuVvWwXxYyZz0123456789abcdefghij",
                        "",
                        "دانش‌آموز", "دانش‌آموز سال سوم، علاقه‌مند به ادبیات فارسی", "images/profiles/avatar-4.jpg", "images/profiles/avatar-4.jpg",
                        890, 3, 420, 32, 67, "[]", true,
                        new DateTime(2026, 3, 20, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 5, 5, 0, 0, 0, DateTimeKind.Utc)
                    },
                }
            );

            // Seed Courses
            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Title", "Description", "Category", "Instructor", "ThumbnailImage", "DurationMinutes", "Price", "CoinPrice", "IsFree", "StudentCount", "Rating", "RatingCount", "Level", "Language", "Requirements", "WhatYouLearn", "IsPublished", "CreatedAt" },
                values: new object[,]
                {
                    {
                        "ریاضی پایه: از پایه تا پیشرفته",
                        "جامع‌ترین روش‌های آموزشی ریاضی مفاهیم را به صورت کاملاً جذاب یاد بگیر.",
                        "ریاضی", "فاطمه احمدی", "images/courses/math.jpg",
                        240, 50000m, 500, false, 450, 4.8, 182,
                        "مقدماتی", "فارسی",
                        "آشنایی با اعداد و حساب پایه",
                        "معادلات - هندسه - جبر - احتمال",
                        true, new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc)
                    },
                    {
                        "علوم طبیعی: کاوش در علوم",
                        "درس علوم طبیعی به‌شکلی جذاب و ساده، با آزمایش‌های عملی.",
                        "علوم", "فاطمه احمدی", "images/courses/science.jpg",
                        300, 60000m, 600, false, 380, 4.6, 145,
                        "مقدماتی", "فارسی",
                        "کنجکاوی و علاقه به طبیعت",
                        "فیزیک - شیمی - زیست‌شناسی - زمین‌شناسی",
                        true, new DateTime(2026, 1, 20, 0, 0, 0, DateTimeKind.Utc)
                    },
                    {
                        "مهارت‌های زندگی",
                        "آموزش مهارت‌های ضروری برای زندگی موفق و شاد.",
                        "مهارت‌ها", "علی محمدی", "images/courses/skills.jpg",
                        180, 45000m, 450, false, 520, 4.9, 210,
                        "مقدماتی", "فارسی",
                        "هیچ پیش‌نیازی لازم نیست",
                        "مدیریت زمان - تفکر انتقادی - ارتباط مؤثر - هدف‌گذاری",
                        true, new DateTime(2026, 2, 5, 0, 0, 0, DateTimeKind.Utc)
                    },
                    {
                        "فارسی ادبی",
                        "درس فارسی و ادبیات فارسی با تمرکز بر شعر کلاسیک و نثر معاصر.",
                        "زبان", "زهرا کریمی", "images/courses/farsi.jpg",
                        270, 55000m, 550, false, 320, 4.7, 98,
                        "متوسط", "فارسی",
                        "آشنایی با الفبای فارسی",
                        "شعر کلاسیک - نثر معاصر - دستور زبان - انشاء",
                        true, new DateTime(2026, 2, 12, 0, 0, 0, DateTimeKind.Utc)
                    },
                    {
                        "تفکر خلاق و نوآوری",
                        "توسعه مهارت‌های خلاقیت، نوآوری و حل خلاقانه مسئله.",
                        "مهارت‌ها", "رضا حسنی", "images/courses/creative.jpg",
                        200, 40000m, 400, false, 580, 4.95, 245,
                        "مقدماتی", "فارسی",
                        "هیچ پیش‌نیازی لازم نیست",
                        "طوفان فکری - تفکر طراحی - نقشه ذهنی - خلق ایده",
                        true, new DateTime(2026, 2, 28, 0, 0, 0, DateTimeKind.Utc)
                    },
                    {
                        "برنامه‌نویسی پایه با Python",
                        "شروع برنامه‌نویسی با Python از صفر، برای کسانی که هیچ تجربه‌ای ندارند.",
                        "برنامه‌نویسی", "رضا حسنی", "images/courses/programming.jpg",
                        360, 75000m, 750, false, 420, 4.8, 163,
                        "مقدماتی", "فارسی",
                        "یک کامپیوتر و اینترنت",
                        "متغیرها - حلقه‌ها - توابع - کار با فایل - پروژه عملی",
                        true, new DateTime(2026, 3, 10, 0, 0, 0, DateTimeKind.Utc)
                    },
                    {
                        "فیزیک دهم",
                        "تدریس کامل فیزیک پایه دهم با حل تمرین و آزمون.",
                        "علوم", "فاطمه احمدی", "images/courses/science.jpg",
                        320, 0m, 0, true, 890, 4.7, 312,
                        "متوسط", "فارسی",
                        "ریاضی پایه",
                        "سینماتیک - دینامیک - کار و انرژی - الکتریسیته",
                        true, new DateTime(2026, 3, 25, 0, 0, 0, DateTimeKind.Utc)
                    },
                    {
                        "زبان انگلیسی A1 تا B1",
                        "یادگیری زبان انگلیسی از مبتدی تا متوسط با روش‌های مدرن.",
                        "زبان", "علی محمدی", "images/courses/farsi.jpg",
                        480, 85000m, 850, false, 670, 4.85, 278,
                        "مقدماتی", "فارسی",
                        "الفبای انگلیسی",
                        "مکالمه - گرامر - لغت - مهارت نوشتاری",
                        true, new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc)
                    },
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "Courses", keyColumn: "Title", keyValues: new object[]
            {
                "ریاضی پایه: از پایه تا پیشرفته", "علوم طبیعی: کاوش در علوم",
                "مهارت‌های زندگی", "فارسی ادبی", "تفکر خلاق و نوآوری",
                "برنامه‌نویسی پایه با Python", "فیزیک دهم", "زبان انگلیسی A1 تا B1"
            });

            migrationBuilder.DeleteData(table: "Users", keyColumn: "Email", keyValues: new object[]
            {
                "ali@avina.ir", "fateme@avina.ir", "reza@avina.ir", "zahra@avina.ir"
            });
        }
    }
}
