namespace backend.DTOs.Auth;

public record TokenPairDto(string AccessToken, string RefreshToken);