using backend.Models.Users;


namespace backend.DTOs.Users;

public record UserReadDto(int Id, string Username, DateTime CreatedAt, Role Role);