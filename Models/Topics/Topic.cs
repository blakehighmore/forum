using System.ComponentModel.DataAnnotations.Schema;
using backend.Models.Categories;
using backend.Models.Posts;
using backend.Models.Tags;
using backend.Models.Users;


namespace backend.Models.Topics;

public class Topic
{
    private readonly List<Post> _posts = [];
    private readonly List<Tag> _tags = [];

    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsClosed { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public int AuthorId { get; set; }
    public User Author { get; set; } = null!;

    public IEnumerable<Post> Posts => _posts;
    public IEnumerable<Tag> Tags => _tags;
    public void AddTag(Tag tag) => _tags.Add(tag);

    [NotMapped] public int MessagesCount => Posts.Count();
}