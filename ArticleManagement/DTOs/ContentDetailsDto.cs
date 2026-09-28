using ArticleManagement.Entities;

namespace ArticleManagement.DTOs;

public class ContentDetailsDto
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public AuthorDetailsDto Author { get; set; }

    public string Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Language { get; set; }
}