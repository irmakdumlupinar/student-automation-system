using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAutomation.Api.Data;

namespace StudentAutomation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeachersController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    public async Task<object> Get() =>
        await db.Teachers.Select(t => new { t.Id, t.Title, Email = t.AppUser.Email }).ToListAsync();
}
