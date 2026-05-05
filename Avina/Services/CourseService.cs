using Avina.Data;
using Avina.Models;
using Microsoft.EntityFrameworkCore;

namespace Avina.Services;

public interface ICourseService
{
    Task<List<Course>> GetAllCoursesAsync();
    Task<Course?> GetCourseByIdAsync(int id);
    Task<List<Course>> GetFeaturedCoursesAsync();
    Task<List<Course>> GetEnrolledCoursesAsync(int userId);
}

public class CourseService : ICourseService
{
    private readonly AvinaDbContext _context;

    public CourseService(AvinaDbContext context)
    {
        _context = context;
    }

    public async Task<List<Course>> GetAllCoursesAsync()
    {
        return await _context.Courses
            .Where(c => c.IsPublished)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Course?> GetCourseByIdAsync(int id)
    {
        return await _context.Courses
            .Include(c => c.Lessons)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Course>> GetFeaturedCoursesAsync()
    {
        return await _context.Courses
            .Where(c => c.IsPublished)
            .OrderByDescending(c => c.Rating)
            .Take(3)
            .ToListAsync();
    }

    public async Task<List<Course>> GetEnrolledCoursesAsync(int userId)
    {
        return await _context.CourseEnrollments
            .Where(e => e.UserId == userId)
            .Include(e => e.Course)
            .Select(e => e.Course)
            .ToListAsync();
    }
}
