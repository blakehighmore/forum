using backend.Models.Users;


namespace backend.Models.Profiles;

public class Profile
{
    public int Id { get; set; }
    public DateOnly? Birthday { get; set; }
    public bool? IsEmployed { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; } 
    public string? AboutMe { get; set; }
    public string? Profession { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}