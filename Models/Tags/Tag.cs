using backend.Models.Topics;
using backend.Models.ValueObjects;


namespace backend.Models.Tags;

public class Tag
{
    private readonly List<Topic> _topics = [];
    
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public Color Color { get; set; } = null!;

    public IEnumerable<Topic> Topics => _topics;
}