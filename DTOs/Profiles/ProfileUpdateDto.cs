namespace backend.DTOs.Profiles;

public record ProfileUpdateDto(
    string? FirstName,
    string? LastName,
    string? AboutMe,
    string? Profession,
    DateOnly? Birthday,
    bool? IsEmployed);