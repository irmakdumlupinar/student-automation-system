using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAutomation.Api.Data;
using StudentAutomation.Api.Domain;
using System.Security.Claims;

namespace StudentAutomation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendanceController(AppDbContext db, IHttpContextAccessor http) : ControllerBase
{
    private string? UserId => http.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

    [HttpPost("enrollments/{enrId:guid}")]
    [Authorize(Roles="Teacher")]
    public async Task<IActionResult> Add(Guid enrId, AttendanceCreateDto dto)
    {
        var e = await db.Enrollments.Include(x=>x.Course).FirstOrDefaultAsync(x => x.Id == enrId);
        if (e is null) return NotFound();
        db.Attendance.Add(new Attendance { EnrollmentId = enrId, Date = dto.Date, IsPresent = dto.IsPresent });
        await db.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("me")]
    [Authorize(Roles="Student")]
    public async Task<object> MyAttendance()
    {
        var s = await db.Students.FirstAsync(x => x.AppUserId == UserId);
        return await db.Attendance
            .Where(a => a.Enrollment.StudentId == s.Id)
            .Select(a => new { a.Id, Course = a.Enrollment.Course.Name, a.Date, a.IsPresent })
            .ToListAsync();
    }
}
