namespace Avina.Models;

public class CourseLesson
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public string? VideoPath { get; set; }
    public string? ThumbnailPath { get; set; }
    public int DurationSeconds { get; set; } = 0;
    public int Order { get; set; } = 0;
    public bool IsFreePreview { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Course Course { get; set; } = null!;
}

public class UserCourseEnrollment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public int CoinSpent { get; set; } = 0;
    public int ProgressPercent { get; set; } = 0;
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    public User User { get; set; } = null!;
    public Course Course { get; set; } = null!;
}

public class Exam
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string Title { get; set; } = "";
    public int PassingScore { get; set; } = 70; // percent
    public int TimeLimitMinutes { get; set; } = 30;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Course Course { get; set; } = null!;
    public ICollection<ExamQuestion> Questions { get; set; } = new List<ExamQuestion>();
    public ICollection<UserExamResult> Results { get; set; } = new List<UserExamResult>();
}

public class ExamQuestion
{
    public int Id { get; set; }
    public int ExamId { get; set; }
    public string QuestionText { get; set; } = "";
    public string OptionA { get; set; } = "";
    public string OptionB { get; set; } = "";
    public string OptionC { get; set; } = "";
    public string OptionD { get; set; } = "";
    public string CorrectAnswer { get; set; } = "A"; // "A","B","C","D"
    public int Points { get; set; } = 1;
    public int Order { get; set; } = 0;

    public Exam Exam { get; set; } = null!;
}

public class UserExamResult
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ExamId { get; set; }
    public int Score { get; set; } = 0;     // percent
    public bool Passed { get; set; } = false;
    public int AttemptNumber { get; set; } = 1;
    public DateTime TakenAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public Exam Exam { get; set; } = null!;
}

public class Certificate
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public int ExamScore { get; set; }
    public string CertificateNumber { get; set; } = "";
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public Course Course { get; set; } = null!;
}
