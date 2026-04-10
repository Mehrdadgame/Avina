namespace Avina.Models;

public class Course
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? Instructor { get; set; }
    public string? ThumbnailImage { get; set; }
    public int Duration { get; set; } // in minutes
    public decimal Price { get; set; }
    public int StudentCount { get; set; }
    public double Rating { get; set; }
}
