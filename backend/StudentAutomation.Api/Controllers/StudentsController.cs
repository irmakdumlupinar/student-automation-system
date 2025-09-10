using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAutomation.Api.Data;
using StudentAutomation.Api.Domain;

namespace StudentAutomation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "AdminOrTeacher")]
    public async Task<ActionResult<IEnumerable<object>>> GetAll()
        => await db.Students.Select(s => new { s.Id, s.Number, s.Name, s.Surname }).ToListAsync();

    [HttpGet("{id:guid}")]
    [Authorize(Policy = "AdminOrTeacher")]
    public async Task<ActionResult<object>> GetById(Guid id)
    {
        var s = await db.Students.FindAsync(id);
        return s is null ? NotFound() : Ok(new { s.Id, s.Number, s.Name, s.Surname });
    }

    [HttpGet("me")]
    [Authorize(Policy = "StudentOnly")]
    public async Task<ActionResult<object>> Me()
    {
        var uid = User.Claims.First(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
        var s = await db.Students.FirstOrDefaultAsync(x => x.AppUserId == uid);
        if (s is null) return NotFound();
        return new { s.Id, s.Number, s.Name, s.Surname };
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "AdminOrTeacher")]
    public async Task<IActionResult> Update(Guid id, StudentUpdateDto dto)
    {
        var s = await db.Students.FindAsync(id);
        if (s is null) return NotFound();
        s.Number = dto.Number; s.Name = dto.Name; s.Surname = dto.Surname;
        await db.SaveChangesAsync();
        return NoContent();
    }
}
