using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Avina.Migrations
{
    /// <inheritdoc />
    public partial class LocalizeGrowthContentFa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "GrowthPaths",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Title" },
                values: new object[] { "رشد در منطق، حل مسئله، تحلیل داده و تصمیم‌گیری.", "مسیر تحلیلی" });

            migrationBuilder.UpdateData(
                table: "GrowthPaths",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Title" },
                values: new object[] { "رشد در ایده‌پردازی، طراحی، داستان‌گویی و خلق محتوا.", "مسیر خلاق" });

            migrationBuilder.UpdateData(
                table: "GrowthPaths",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Title" },
                values: new object[] { "رشد در ارتباط، گفت‌وگو، کار تیمی و رهبری.", "مسیر رهبری اجتماعی" });

            migrationBuilder.UpdateData(
                table: "GrowthPaths",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Title" },
                values: new object[] { "رشد با ساخت پروژه‌های واقعی و تجربه عملی.", "مسیر سازنده عملی" });

            migrationBuilder.UpdateData(
                table: "GrowthPaths",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "Title" },
                values: new object[] { "رشد در همدلی، مسئولیت‌پذیری و اثرگذاری اجتماعی.", "مسیر معنا و مراقبت" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "۲۰ دقیقه بدون موبایل فقط روی یک کار مشخص تمرکز کن.", "افزایش توان تمرکز", "تمرین تمرکز عمیق" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "در یک کار روزمره سه الگوی تکرارشونده پیدا کن و بنویس.", "تقویت مشاهده تحلیلی", "شکار الگو" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "یک معمای منطقی حل کن و روش حل خودت را توضیح بده.", "تقویت استدلال منطقی", "مرور معمای منطقی" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "یک مشکل کوچک واقعی انتخاب کن و دو راه‌حل عملی پیشنهاد بده.", "حل مسئله در دنیای واقعی", "حل یک مسئله واقعی" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "برای یک موضوع ساده، ۱۰ ایده متفاوت بنویس.", "گسترش خلاقیت", "چالش ۱۰ ایده" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "یک داستان کوتاه در ۶ قاب (متن یا تصویر) بساز.", "تمرین داستان‌گویی", "داستان ۶ قاب" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "یک خروجی خلاق نهایی منتشر کن و بازخورد بگیر.", "تبدیل ایده به اجرا", "انتشار خروجی خلاق" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "۱۰ دقیقه به حرف یک نفر گوش بده و ۳ نکته کلیدی بنویس.", "تقویت شنیدن و همدلی", "تمرین شنیدن فعال" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "یک موقعیت اختلاف را شبیه‌سازی کن و روش میانجی‌گری‌ات را ثبت کن.", "تمرین مدیریت تعارض", "نقش‌آفرینی تعارض" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "یک گفت‌وگوی ۲۰ دقیقه‌ای تیمی را هدایت کن و یک بازخورد ثبت کن.", "تقویت رهبری تیم", "هدایت جلسه کوتاه" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "یک خروجی عملی کوچک با ابزارهای موجود بساز.", "اجرای عملی", "ساخت ابزار کوچک" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "خروجی دیروز را براساس یک بازخورد بهبود بده.", "یادگیری بهبود تدریجی", "حلقه بهبود" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "با یک نفر درباره تجربه سختش گفت‌وگو کن و برداشتت را بنویس.", "رشد همدلی", "گفت‌وگوی همدلانه" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "یک تعهد مشخص تعریف کن و تا پایان روز انجامش بده.", "تقویت مسئولیت‌پذیری", "قول مسئولیت" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "یک اقدام واقعی برای کمک به اطرافیان انجام بده و مدرک ثبت کن.", "اثرگذاری معنادار", "اقدام حمایت اجتماعی" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Title" },
                values: new object[] { "حفظ توجه روی یک کار مشخص.", "تمرکز" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Title" },
                values: new object[] { "دیدن الگوها و جزئیات مهم.", "مشاهده" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Title" },
                values: new object[] { "استدلال منظم و تحلیل علت و معلول.", "تفکر منطقی" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Title" },
                values: new object[] { "شکستن مسائل واقعی و رسیدن به راه‌حل.", "حل مسئله" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "Title" },
                values: new object[] { "تولید چندین ایده خلاق برای یک مسئله.", "ایده‌پردازی" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Description", "Title" },
                values: new object[] { "بیان ایده با روایت جذاب.", "داستان‌گویی" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Description", "Title" },
                values: new object[] { "تبدیل ایده به خروجی واقعی.", "اجرای خلاق" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Description", "Title" },
                values: new object[] { "گوش‌دادن دقیق و بازتاب درست.", "شنیدن فعال" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Description", "Title" },
                values: new object[] { "مدیریت اختلاف با گفت‌وگوی امن.", "میانجی‌گری تعارض" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Description", "Title" },
                values: new object[] { "هدایت تیم با وضوح و همدلی.", "رهبری تیم" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Description", "Title" },
                values: new object[] { "ساخت پروژه‌های کوچک کاربردی.", "ساخت عملی" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Description", "Title" },
                values: new object[] { "بهبود خروجی براساس بازخورد.", "تکرار و بهبود" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Description", "Title" },
                values: new object[] { "درک احساس و نگاه دیگران.", "همدلی" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Description", "Title" },
                values: new object[] { "پذیرفتن مسئولیت اثر رفتار خود.", "مسئولیت‌پذیری" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Description", "Title" },
                values: new object[] { "کمک موثر به جامعه و اطرافیان.", "حمایت اجتماعی" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "GrowthPaths",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Title" },
                values: new object[] { "Logic, problem solving, data and decision thinking.", "Analytical Path" });

            migrationBuilder.UpdateData(
                table: "GrowthPaths",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Title" },
                values: new object[] { "Storytelling, design, content and ideation.", "Creative Path" });

            migrationBuilder.UpdateData(
                table: "GrowthPaths",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Title" },
                values: new object[] { "Communication, teamwork and leadership growth.", "Social Leadership Path" });

            migrationBuilder.UpdateData(
                table: "GrowthPaths",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Title" },
                values: new object[] { "Build with hands-on projects and practical output.", "Maker Practical Path" });

            migrationBuilder.UpdateData(
                table: "GrowthPaths",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "Title" },
                values: new object[] { "Empathy, responsibility and meaningful impact.", "Care Meaning Path" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "Work on one task for 20 minutes without phone distraction.", "Build focus stamina", "Deep Focus Sprint" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "Observe a daily system and write 3 repeating patterns.", "Improve analytical observation", "Pattern Hunt" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "Solve one logic puzzle and explain your strategy.", "Strengthen logical reasoning", "Logic Puzzle Review" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "Pick one small real-life problem and propose 2 solutions.", "Real-world problem solving", "Solve a Real Problem" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "Write 10 different solutions for a simple daily challenge.", "Expand creativity", "10 Ideas Challenge" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "Create a short 6-frame visual or text story.", "Practice storytelling", "Story in 6 Frames" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "Publish one final creative piece and reflect on feedback.", "Ship creative work", "Publish Creative Output" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "Listen to someone for 10 minutes and summarize 3 insights.", "Improve listening and empathy", "Active Listening Practice" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "Run a roleplay and document how you de-escalated conflict.", "Practice mediation skills", "Conflict Roleplay" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "Lead a short group discussion and collect one feedback point.", "Develop leadership", "Lead a 20-min Session" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "Build a simple practical output with available tools.", "Hands-on execution", "Build Mini Tool" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "Improve yesterday's output based on one feedback item.", "Learn iterative improvement", "Iteration Loop" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "Ask someone about a hard experience and write what you learned.", "Grow empathy", "Empathy Interview" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "Define one commitment and prove completion by end of day.", "Build accountability", "Responsibility Promise" });

            migrationBuilder.UpdateData(
                table: "Missions",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Description", "Goal", "Title" },
                values: new object[] { "Take one action that helps your community and share evidence.", "Create meaningful impact", "Community Support Action" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Title" },
                values: new object[] { "Maintain attention on one task.", "Focus" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Title" },
                values: new object[] { "Notice patterns and details.", "Observation" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Title" },
                values: new object[] { "Reasoning and argument structure.", "Logical Thinking" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Title" },
                values: new object[] { "Break down and solve real problems.", "Problem Solving" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "Title" },
                values: new object[] { "Generate multiple creative options.", "Idea Generation" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Description", "Title" },
                values: new object[] { "Explain ideas with narrative.", "Storytelling" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Description", "Title" },
                values: new object[] { "Ship a creative output.", "Creative Execution" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Description", "Title" },
                values: new object[] { "Listen and reflect effectively.", "Active Listening" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Description", "Title" },
                values: new object[] { "Resolve disagreement safely.", "Conflict Mediation" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Description", "Title" },
                values: new object[] { "Lead small teams with clarity.", "Team Leadership" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Description", "Title" },
                values: new object[] { "Build practical mini-projects.", "Hands-on Build" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Description", "Title" },
                values: new object[] { "Improve output through feedback.", "Iteration" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Description", "Title" },
                values: new object[] { "Understand others' perspective.", "Empathy" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Description", "Title" },
                values: new object[] { "Take ownership of impact.", "Responsibility" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Description", "Title" },
                values: new object[] { "Contribute to community growth.", "Community Support" });
        }
    }
}
