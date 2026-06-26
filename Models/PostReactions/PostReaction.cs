using backend.Models.Posts;
using backend.Models.Users;


namespace backend.Models.PostReactions;

public class PostReaction
{
    public int Id { get; set; }
    public Reaction Reaction { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int PostId { get; set; }
    public Post Post { get; set; } = null!;
}