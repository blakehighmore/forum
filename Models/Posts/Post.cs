using backend.Models.Topics;
using backend.Models.Users;


namespace backend.Models.Posts;

public class Post
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = null;
    
    public int AuthorId { get; set; }
    public User Author { get; set; } = null!;
    
    public int TopicId { get; set; }
    public Topic Topic { get; set; } = null!;
}