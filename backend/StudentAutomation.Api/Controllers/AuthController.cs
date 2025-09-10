using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudentAutomation.Api.Data;
using StudentAutomation.Api.Domain;

namespace StudentAutomation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    UserManager<AppUser> userManager,
    RoleManager<IdentityRole> roleMgr,
    IConfiguration cfg,
    AppDbContext db) : ControllerBase
{
    [HttpPost("register")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (!new[] {"Admin","Teacher","Student"}.Contains(dto.Role))
            return BadRequest("Role must be Admin/Teacher/Student");

        var user = new AppUser { UserName = dto.Email, Email = dto.Email };
        var res = await userManager.CreateAsync(user, dto.Password);
        if (!res.Succeeded) return BadRequest(res.Errors);
        await userManager.AddToRoleAsync(user, dto.Role);

        if (dto.Role == "Student")
            db.Students.Add(new Student { AppUserId = user.Id, Name = dto.Name ?? "", Surname = dto.Surname ?? "", Number = $"S{Random.Shared.Next(10000,99999)}" });
        else if (dto.Role == "Teacher")
            db.Teachers.Add(new Teacher { AppUserId = user.Id, Title = "Instructor" });

        await db.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<TokenDto>> Login(LoginDto dto)
    {
        var u = await userManager.FindByEmailAsync(dto.Email);
        if (u is null || !await userManager.CheckPasswordAsync(u, dto.Password))
            return Unauthorized();

        var roles = await userManager.GetRolesAsync(u);
        var claims = new List<Claim> {
            new(JwtRegisteredClaimNames.Sub, u.Id),
            new(ClaimTypes.Email, u.Email!),
            new(ClaimTypes.NameIdentifier, u.Id)
        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["Jwt:Key"]!));
        var token = new JwtSecurityToken(
            issuer: cfg["Jwt:Issuer"],
            audience: cfg["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return new TokenDto(new JwtSecurityTokenHandler().WriteToken(token));
    }
}
