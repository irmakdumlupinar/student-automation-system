using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAutomation.Api.Data;
using StudentAutomation.Api.Domain;
using System.Security.Claims;

namespace StudentAutomation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GradesController(AppDbContext db, IHttpContextAccessor http) : ControllerBase
{
    private string? UserId => http.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

    [HttpPost("enrollments/{enrId:guid}")]
    [Authorize(Roles="Teacher")]
    public async Task<IActionResult> Add(Guid enrId, GradeCreateDto dto)
    {
        var e = await db.Enrollments.Include(x=>x.Course).FirstOrDefaultAsync(x => x.Id == enrId);
        if (e is null) return NotFound();
        db.Grades.Add(new Grade { EnrollmentId = enrId, Value = dto.Value, Note = dto.Note });
        await db.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("me")]
    [Authorize(Roles="Student")]
    public async Task<object> MyGrades()
    {
        var s = await db.Students.FirstAsync(x => x.AppUserId == UserId);
        return await db.Grades
            .Where(g => g.Enrollment.StudentId == s.Id)
            .Select(g => new { g.Id, Course = g.Enrollment.Course.Name, g.Value, g.Note, g.CreatedAt })
            .ToListAsync();
    }
}
