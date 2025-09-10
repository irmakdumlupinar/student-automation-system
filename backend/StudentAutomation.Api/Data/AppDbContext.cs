using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentAutomation.Api.Domain;

namespace StudentAutomation.Api.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) {}

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Grade> Grades => Set<Grade>();
    public DbSet<Attendance> Attendance => Set<Attendance>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);
        b.Entity<Student>().HasIndex(x => x.Number).IsUnique();

        b.Entity<Enrollment>()
            .HasOne(e => e.Student).WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId).OnDelete(DeleteBehavior.Cascade);

        b.Entity<Enrollment>()
            .HasOne(e => e.Course).WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId).OnDelete(DeleteBehavior.Cascade);
    }
}
