using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAutomation.Api.Data;
using StudentAutomation.Api.Domain;
using System.Security.Claims;

namespace StudentAutomation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController(AppDbContext db, IHttpContextAccessor http) : ControllerBase
{
    private string? UserId => http.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create(CourseCreateDto dto)
    {
        if (!await db.Teachers.AnyAsync(t => t.Id == dto.TeacherId)) return BadRequest("Teacher not found");
        db.Courses.Add(new Course { Name = dto.Name, TeacherId = dto.TeacherId });
        await db.SaveChangesAsync();
        return Ok();
    }

    [HttpGet]
    [Authorize]
    public async Task<object> List()
    {
        if (User.IsInRole("Admin"))
            return await db.Courses.Include(c=>c.Teacher).Select(c => new { c.Id, c.Name, c.Status, Teacher=c.Teacher.Id }).ToListAsync();

        if (User.IsInRole("Teacher"))
        {
            var t = await db.Teachers.FirstOrDefaultAsync(x => x.AppUserId == UserId);
            return await db.Courses.Where(c => c.TeacherId == t!.Id)
                .Select(c => new { c.Id, c.Name, c.Status }).ToListAsync();
        }

        var s = await db.Students.FirstOrDefaultAsync(x => x.AppUserId == UserId);
        return await db.Enrollments.Where(e => e.StudentId == s!.Id)
            .Select(e => new { e.Course.Id, e.Course.Name, e.Course.Status }).ToListAsync();
    }

    [HttpPut("{id:guid}/status")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> UpdateStatus(Guid id, CourseStatusDto dto)
    {
        if (!Enum.TryParse<CourseStatus>(dto.Status, true, out var newStatus))
            return BadRequest("Status: Planned|Started|Finished");

        var t = await db.Teachers.FirstOrDefaultAsync(x => x.AppUserId == UserId);
        var c = await db.Courses.FirstOrDefaultAsync(x => x.Id == id && x.TeacherId == t!.Id);
        if (c is null) return NotFound();
        c.Status = newStatus;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("{id:guid}/enroll/{studentId:guid}")]
    [Authorize(Policy = "AdminOrTeacher")]
    public async Task<IActionResult> Enroll(Guid id, Guid studentId)
    {
        if (!await db.Courses.AnyAsync(c => c.Id == id)) return NotFound("Course");
        if (!await db.Students.AnyAsync(s => s.Id == studentId)) return NotFound("Student");
        if (await db.Enrollments.AnyAsync(e => e.CourseId == id && e.StudentId == studentId)) return Ok();

        db.Enrollments.Add(new Enrollment { CourseId = id, StudentId = studentId });
        await db.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id:guid}/enroll/{studentId:guid}")]
    [Authorize(Policy = "AdminOrTeacher")]
    public async Task<IActionResult> Unenroll(Guid id, Guid studentId)
    {
        var e = await db.Enrollments.FirstOrDefaultAsync(x => x.CourseId == id && x.StudentId == studentId);
        if (e is null) return NotFound();
        db.Enrollments.Remove(e);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
