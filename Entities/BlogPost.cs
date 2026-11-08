namespace ModularCommerce.Entities;

public class BlogPost
{
    public long Id { get; set; }
    public required string Slug { get; set; }
    public required string Title { get; set; }
    public string? Excerpt { get; set; }
    public required string Content { get; set; }
    public string? ImageUrl { get; set; }
    public string Language { get; set; } = "en";
    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
    public bool IsPublished { get; set; } = true;
}
