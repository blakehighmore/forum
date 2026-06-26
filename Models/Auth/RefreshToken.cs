using System.ComponentModel.DataAnnotations.Schema;
using backend.Models.Users;


namespace backend.Models.Auth;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? RevokedAt { get; set; }

    public int UserId { get; set; }

    [NotMapped] public bool IsActive => RevokedAt is null && ExpiresAt > DateTime.UtcNow;
}