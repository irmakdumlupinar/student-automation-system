namespace StudentAutomation.Api.Domain;

public class AppUser : Microsoft.AspNetCore.Identity.IdentityUser { }

public class Student
{
    public Guid Id { get; set; }
    public string AppUserId { get; set; } = default!;
    public AppUser AppUser { get; set; } = default!;
    public string Number { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}

public class Teacher
{
    public Guid Id { get; set; }
    public string AppUserId { get; set; } = default!;
    public AppUser AppUser { get; set; } = default!;
    public string Title { get; set; } = "Instructor";
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}

public enum CourseStatus { Planned, Started, Finished }

public class Course
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public CourseStatus Status { get; set; } = CourseStatus.Planned;
    public Guid TeacherId { get; set; }
    public Teacher Teacher { get; set; } = default!;
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}

public class Enrollment
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = default!;
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = default!;
    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    public ICollection<Attendance> Attendance { get; set; } = new List<Attendance>();
}

public class Grade
{
    public Guid Id { get; set; }
    public Guid EnrollmentId { get; set; }
    public Enrollment Enrollment { get; set; } = default!;
    public decimal Value { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Attendance
{
    public Guid Id { get; set; }
    public Guid EnrollmentId { get; set; }
    public Enrollment Enrollment { get; set; } = default!;
    public DateOnly Date { get; set; }
    public bool IsPresent { get; set; }
}
