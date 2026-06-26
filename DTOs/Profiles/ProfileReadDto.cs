namespace backend.DTOs.Profiles;

public record ProfileReadDto(
    string? FirstName,
    string? LastName,
    string? AboutMe,
    string? Profession,
    DateOnly? Birthday,
    bool? IsEmployed);