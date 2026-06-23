using System.ComponentModel.DataAnnotations.Schema;
using backend.Models.Topics;


namespace backend.Models.Categories;

public class Category
{
    private readonly List<Topic> _topics = [];

    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public IEnumerable<Topic> Topics => _topics;
    
    [NotMapped] public int TopicsCount => Topics.Count();
}
