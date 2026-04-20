using Avina.Data;
using Avina.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Avina.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly AvinaDbContext _db;

    public CoursesController(AvinaDbContext db)
    {
        _db = db;
    }

    // GET /api/courses?page=1&size=12&category=&level=&free=
    [HttpGet]
    public async Task<IActionResult> GetAll(int page = 1, int size = 12,
        string? category = null, string? level = null, bool? free = null)
    {
        var q = _db.Courses.Where(c => c.IsPublished).AsQueryable();
        if (!string.IsNullOrEmpty(category)) q = q.Where(c => c.Category == category);
        if (!string.IsNullOrEmpty(level)) q = q.Where(c => c.Level == level);
        if (free.HasValue) q = q.Where(c => c.IsFree == free.Value);

        var total = await q.CountAsync();
        var items = await q.OrderByDescending(c => c.StudentCount)
            .Skip((page - 1) * size).Take(size)
            .Select(c => new
            {
                c.Id, c.Title, c.Description, c.Category, c.Instructor,
                c.ThumbnailImage, c.DurationMinutes, c.CoinPrice, c.IsFree,
                c.StudentCount, c.Rating, c.RatingCount, c.Level, c.Language,
                c.IsPublished, c.CreatedAt
            }).ToListAsync();

        return Ok(new { total, page, size, items });
    }

    // GET /api/courses/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var course = await _db.Courses
            .Include(c => c.Lessons.OrderBy(l => l.Order))
            .Include(c => c.Exams)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (course == null) return NotFound();

        var whatYouLearnList = course.WhatYouLearn?.Split('|') ?? Array.Empty<string>();
        var requirementsList = course.Requirements?.Split('|') ?? Array.Empty<string>();

        return Ok(new
        {
            course.Id, course.Title, course.Description, course.Category,
            course.Instructor, course.ThumbnailImage, course.PreviewVideoPath,
            course.DurationMinutes, course.CoinPrice, course.IsFree,
            course.StudentCount, course.Rating, course.RatingCount,
            course.Level, course.Language,
            whatYouLearn = whatYouLearnList,
            requirements = requirementsList,
            course.CreatedAt,
            lessons = course.Lessons.Select(l => new
            {
                l.Id, l.Title, l.Description, l.Order,
                l.IsFreePreview, l.DurationSeconds, l.ThumbnailPath,
                videoPath = l.IsFreePreview ? l.VideoPath : null
            }),
            hasExam = course.Exams.Any(),
            examId = course.Exams.FirstOrDefault()?.Id
        });
    }

    // POST /api/courses/{id}/enroll
    [HttpPost("{id}/enroll")]
    public async Task<IActionResult> Enroll(int id, [FromBody] EnrollRequest req)
    {
        var course = await _db.Courses.FindAsync(id);
        if (course == null) return NotFound(new { message = "دوره یافت نشد" });

        var user = await _db.Users.FindAsync(req.UserId);
        if (user == null) return NotFound(new { message = "کاربر یافت نشد" });

        var alreadyEnrolled = await _db.CourseEnrollments
            .AnyAsync(e => e.UserId == req.UserId && e.CourseId == id);
        if (alreadyEnrolled) return BadRequest(new { message = "قبلاً ثبت‌نام کرده‌اید" });

        int coinSpent = 0;
        if (!course.IsFree)
        {
            if (user.Coin < course.CoinPrice)
                return BadRequest(new { message = $"کوین کافی نیست. نیاز: {course.CoinPrice}, موجودی: {user.Coin}" });
            user.Coin -= course.CoinPrice;
            coinSpent = course.CoinPrice;
        }

        course.StudentCount++;

        _db.CourseEnrollments.Add(new UserCourseEnrollment
        {
            UserId = req.UserId,
            CourseId = id,
            CoinSpent = coinSpent
        });

        await _db.SaveChangesAsync();

        return Ok(new { message = "ثبت‌نام موفق", newCoinBalance = user.Coin });
    }

    // GET /api/courses/{id}/enrollment?userId=
    [HttpGet("{id}/enrollment")]
    public async Task<IActionResult> GetEnrollment(int id, int? userId)
    {
        var course = await _db.Courses.FindAsync(id);
        if (course == null) return NotFound();

        if (course.IsFree) return Ok(new { isEnrolled = true, isFree = true, coinPrice = 0 });
        if (!userId.HasValue) return Ok(new { isEnrolled = false, isFree = false, coinPrice = course.CoinPrice });

        var enrollment = await _db.CourseEnrollments
            .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == id);

        return Ok(new
        {
            isEnrolled = enrollment != null,
            isFree = false,
            coinPrice = course.CoinPrice,
            progressPercent = enrollment?.ProgressPercent ?? 0,
            completedAt = enrollment?.CompletedAt
        });
    }

    // GET /api/courses/{id}/exam?userId=
    [HttpGet("{id}/exam")]
    public async Task<IActionResult> GetExam(int id, int? userId)
    {
        var exam = await _db.Exams
            .Include(e => e.Questions.OrderBy(q => q.Order))
            .FirstOrDefaultAsync(e => e.CourseId == id);

        if (exam == null) return NotFound(new { message = "آزمونی برای این دوره وجود ندارد" });

        if (userId.HasValue)
        {
            var course = await _db.Courses.FindAsync(id);
            if (course != null && !course.IsFree)
            {
                var enrolled = await _db.CourseEnrollments
                    .AnyAsync(e => e.UserId == userId && e.CourseId == id);
                if (!enrolled) return Forbid();
            }
        }

        return Ok(new
        {
            exam.Id,
            exam.Title,
            exam.PassingScore,
            exam.TimeLimitMinutes,
            questions = exam.Questions.Select(q => new
            {
                q.Id, q.QuestionText, q.Order, q.Points,
                q.OptionA, q.OptionB, q.OptionC, q.OptionD
                // CorrectAnswer intentionally omitted
            })
        });
    }

    // POST /api/courses/{id}/exam/submit
    [HttpPost("{id}/exam/submit")]
    public async Task<IActionResult> SubmitExam(int id, [FromBody] ExamSubmitRequest req)
    {
        var exam = await _db.Exams
            .Include(e => e.Questions)
            .FirstOrDefaultAsync(e => e.CourseId == id);
        if (exam == null) return NotFound(new { message = "آزمون یافت نشد" });

        var user = await _db.Users.FindAsync(req.UserId);
        if (user == null) return NotFound(new { message = "کاربر یافت نشد" });

        int totalPoints = exam.Questions.Sum(q => q.Points);
        int earnedPoints = 0;
        foreach (var q in exam.Questions)
        {
            if (req.Answers.TryGetValue(q.Id, out var answer) &&
                answer.Equals(q.CorrectAnswer, StringComparison.OrdinalIgnoreCase))
                earnedPoints += q.Points;
        }

        int scorePercent = totalPoints > 0 ? (int)Math.Round(earnedPoints * 100.0 / totalPoints) : 0;
        bool passed = scorePercent >= exam.PassingScore;

        int attemptNumber = await _db.ExamResults
            .Where(r => r.UserId == req.UserId && r.ExamId == exam.Id)
            .CountAsync() + 1;

        _db.ExamResults.Add(new UserExamResult
        {
            UserId = req.UserId,
            ExamId = exam.Id,
            Score = scorePercent,
            Passed = passed,
            AttemptNumber = attemptNumber
        });

        string? certNumber = null;
        bool isNewCert = false;
        if (passed)
        {
            var existingCert = await _db.Certificates
                .FirstOrDefaultAsync(c => c.UserId == req.UserId && c.CourseId == id);

            if (existingCert == null)
            {
                certNumber = $"AV-{DateTime.UtcNow:yyyyMMdd}-{req.UserId:D4}-{id:D3}";
                _db.Certificates.Add(new Certificate
                {
                    UserId = req.UserId,
                    CourseId = id,
                    ExamScore = scorePercent,
                    CertificateNumber = certNumber
                });

                user.Coin += 50;
                isNewCert = true;

                var enrollment = await _db.CourseEnrollments
                    .FirstOrDefaultAsync(e => e.UserId == req.UserId && e.CourseId == id);
                if (enrollment != null)
                {
                    enrollment.ProgressPercent = 100;
                    enrollment.CompletedAt = DateTime.UtcNow;
                }
            }
            else
            {
                certNumber = existingCert.CertificateNumber;
            }
        }

        await _db.SaveChangesAsync();

        return Ok(new
        {
            score = scorePercent,
            passed,
            passingScore = exam.PassingScore,
            earnedPoints,
            totalPoints,
            attemptNumber,
            certificateNumber = certNumber,
            bonusCoins = isNewCert ? 50 : 0,
            newCoinBalance = user.Coin
        });
    }

    // GET /api/courses/{id}/certificate?userId=
    [HttpGet("{id}/certificate")]
    public async Task<IActionResult> GetCertificate(int id, int userId)
    {
        var cert = await _db.Certificates
            .Include(c => c.User)
            .Include(c => c.Course)
            .FirstOrDefaultAsync(c => c.UserId == userId && c.CourseId == id);

        if (cert == null) return NotFound(new { message = "گواهینامه یافت نشد" });

        return Ok(new
        {
            cert.Id,
            cert.CertificateNumber,
            cert.ExamScore,
            cert.IssuedAt,
            userName = cert.User.Name,
            courseTitle = cert.Course.Title,
            courseInstructor = cert.Course.Instructor
        });
    }

    // GET /api/courses/{id}/lessons/{lessonId}?userId=
    [HttpGet("{id}/lessons/{lessonId}")]
    public async Task<IActionResult> GetLesson(int id, int lessonId, int? userId)
    {
        var lesson = await _db.CourseLessons
            .FirstOrDefaultAsync(l => l.Id == lessonId && l.CourseId == id);
        if (lesson == null) return NotFound();

        if (lesson.IsFreePreview)
            return Ok(new { lesson.Id, lesson.Title, lesson.Description, lesson.VideoPath, lesson.DurationSeconds, lesson.Order });

        if (!userId.HasValue) return Forbid();

        var course = await _db.Courses.FindAsync(id);
        if (course != null && !course.IsFree)
        {
            var enrolled = await _db.CourseEnrollments
                .AnyAsync(e => e.UserId == userId && e.CourseId == id);
            if (!enrolled) return Forbid();
        }

        return Ok(new { lesson.Id, lesson.Title, lesson.Description, lesson.VideoPath, lesson.DurationSeconds, lesson.Order });
    }
}

public record EnrollRequest(int UserId);
public record ExamSubmitRequest(int UserId, Dictionary<int, string> Answers);
