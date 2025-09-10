using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentAutomation.Api.Data;
using StudentAutomation.Api.Domain;

namespace StudentAutomation.Api.SeedData;

public static class Seeder
{
    public static async Task Run(IServiceProvider sp)
    {
        var db = sp.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();

        var roleMgr = sp.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var r in new[] { "Admin","Teacher","Student" })
            if (!await roleMgr.RoleExistsAsync(r))
                await roleMgr.CreateAsync(new IdentityRole(r));

        var userMgr = sp.GetRequiredService<UserManager<AppUser>>();
        async Task<AppUser> EnsureUser(string email, string pass, string role)
        {
            var u = await userMgr.FindByEmailAsync(email);
            if (u is null)
            {
                u = new AppUser { UserName = email, Email = email };
                await userMgr.CreateAsync(u, pass);
                await userMgr.AddToRoleAsync(u, role);
            }
            return u;
        }

        var adminU = await EnsureUser("admin@demo.com", "Admin123*", "Admin");
        var tU = await EnsureUser("teacher@demo.com", "Teacher123*", "Teacher");
        var sU = await EnsureUser("student@demo.com", "Student123*", "Student");

        if (!await db.Teachers.AnyAsync())
        {
            var t = new Teacher { AppUserId = tU.Id, Title = "Instructor" };
            var s = new Student { AppUserId = sU.Id, Name = "Ada", Surname = "Lovelace", Number = "S10001" };
            var c = new Course { Name = "Algorithms", Teacher = t, Status = CourseStatus.Planned };
            var e = new Enrollment { Student = s, Course = c };
            db.AddRange(t, s, c, e,
                new Grade { Enrollment = e, Value = 95, Note="Midterm" },
                new Attendance { Enrollment = e, Date = DateOnly.FromDateTime(DateTime.UtcNow), IsPresent = true });
            await db.SaveChangesAsync();
        }
    }
}
