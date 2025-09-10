namespace StudentAutomation.Api.Domain;

public record RegisterDto(string Email, string Password, string Role, string? Name, string? Surname);
public record LoginDto(string Email, string Password);
public record TokenDto(string AccessToken);

public record StudentUpdateDto(string Number, string Name, string Surname);

public record CourseCreateDto(string Name, Guid TeacherId);
public record CourseStatusDto(string Status);
public record GradeCreateDto(decimal Value, string? Note);
public record AttendanceCreateDto(DateOnly Date, bool IsPresent);
