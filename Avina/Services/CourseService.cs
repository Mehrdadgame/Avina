using Avina.Models;

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
    private List<Course> courses = new();

    public CourseService()
    {
        InitializeMockData();
    }

    public Task<List<Course>> GetAllCoursesAsync()
    {
        return Task.FromResult(courses);
    }

    public Task<Course?> GetCourseByIdAsync(int id)
    {
        return Task.FromResult(courses.FirstOrDefault(c => c.Id == id));
    }

    public Task<List<Course>> GetFeaturedCoursesAsync()
    {
        return Task.FromResult(courses.Take(3).ToList());
    }

    public Task<List<Course>> GetEnrolledCoursesAsync(int userId)
    {
        // Mock implementation
        return Task.FromResult(courses.Take(2).ToList());
    }

    private void InitializeMockData()
    {
        courses = new List<Course>
        {
            new Course 
            { 
                Id = 1,
                Title = "ریاضی پایه",
                Description = "آموزش جامع مفاهیم پایه‌ای ریاضی",
                Category = "ریاضی",
                ThumbnailImage = "images/courses/math.jpg",
                Price = 50000,
                Instructor = "محمد علی",
                StudentCount = 450,
                Rating = 4.8,
                Duration = 240
            },
            new Course 
            { 
                Id = 2,
                Title = "علوم طبیعی",
                Description = "درس علوم طبیعی به‌طریقه جذاب و ساده",
                Category = "علوم",
                ThumbnailImage = "images/courses/science.jpg",
                Price = 60000,
                Instructor = "فاطمه احمدی",
                StudentCount = 380,
                Rating = 4.6,
                Duration = 300
            },
            new Course 
            { 
                Id = 3,
                Title = "مهارت‌های زندگی",
                Description = "آموزش مهارت‌های ضروری برای زندگی موفق",
                Category = "مهارت‌ها",
                ThumbnailImage = "images/courses/skills.jpg",
                Price = 45000,
                Instructor = "علی رضائی",
                StudentCount = 520,
                Rating = 4.9,
                Duration = 180
            },
            new Course 
            { 
                Id = 4,
                Title = "فارسی ادبی",
                Description = "درس فارسی و ادبیات فارسی",
                Category = "زبان",
                ThumbnailImage = "images/courses/farsi.jpg",
                Price = 55000,
                Instructor = "زهرا کریمی",
                StudentCount = 320,
                Rating = 4.7,
                Duration = 270
            },
            new Course 
            { 
                Id = 5,
                Title = "تفکر خالق",
                Description = "توسعه مهارت‌های خلاقیت و نوآوری",
                Category = "مهارت‌ها",
                ThumbnailImage = "images/courses/creative.jpg",
                Price = 40000,
                Instructor = "حسن رزاقی",
                StudentCount = 580,
                Rating = 4.95,
                Duration = 200
            },
            new Course 
            { 
                Id = 6,
                Title = "برنامه‌نویسی پایه",
                Description = "شروع به برنامه‌نویسی با Python",
                Category = "برنامه‌نویسی",
                ThumbnailImage = "images/courses/programming.jpg",
                Price = 75000,
                Instructor = "رضا حسنی",
                StudentCount = 420,
                Rating = 4.8,
                Duration = 360
            }
        };
    }
}
