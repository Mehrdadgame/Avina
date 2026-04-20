namespace Avina.Models;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Category { get; set; } = "";
    public string Instructor { get; set; } = "";
    public string? ThumbnailImage { get; set; }
    public string? PreviewVideoPath { get; set; }
    public int DurationMinutes { get; set; } = 0;
    public decimal Price { get; set; } = 0;
    public int CoinPrice { get; set; } = 0;
    public bool IsFree { get; set; } = false;
    public int StudentCount { get; set; } = 0;
    public double Rating { get; set; } = 0;
    public int RatingCount { get; set; } = 0;
    public string Level { get; set; } = "مقدماتی"; // مقدماتی / متوسط / پیشرفته
    public string Language { get; set; } = "فارسی";
    public string? Requirements { get; set; }
    public string? WhatYouLearn { get; set; }
    public bool IsPublished { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<CourseLesson> Lessons { get; set; } = new List<CourseLesson>();
    public ICollection<UserCourseEnrollment> Enrollments { get; set; } = new List<UserCourseEnrollment>();
    public ICollection<Exam> Exams { get; set; } = new List<Exam>();
    public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
}
