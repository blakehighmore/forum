using System.ComponentModel.DataAnnotations.Schema;
using backend.Models.Profiles;


namespace backend.Models.Users;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? BannedAt { get; set; }
    public Role Role { get; set; } = Role.User;
    
    public Profile Profile { get; set; } = null!;
    
    [NotMapped] public bool IsBanned => BannedAt != null;
}