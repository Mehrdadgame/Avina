using Avina.Models;
using Avina.Services;
using Microsoft.AspNetCore.Mvc;

namespace Avina.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    // GET api/courses
    [HttpGet]
    public async Task<ActionResult<List<Course>>> GetAll()
    {
        var courses = await _courseService.GetAllCoursesAsync();
        return Ok(courses);
    }

    // GET api/courses/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Course>> GetById(int id)
    {
        var course = await _courseService.GetCourseByIdAsync(id);
        if (course is null)
            return NotFound(new { message = $"دوره با شناسه {id} یافت نشد" });

        return Ok(course);
    }

    // GET api/courses/featured
    [HttpGet("featured")]
    public async Task<ActionResult<List<Course>>> GetFeatured()
    {
        var courses = await _courseService.GetFeaturedCoursesAsync();
        return Ok(courses);
    }

    // GET api/courses/enrolled/3
    [HttpGet("enrolled/{userId:int}")]
    public async Task<ActionResult<List<Course>>> GetEnrolled(int userId)
    {
        var courses = await _courseService.GetEnrolledCoursesAsync(userId);
        return Ok(courses);
    }
}
