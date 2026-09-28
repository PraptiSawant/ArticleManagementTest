using ArticleManagement.Entities;

namespace ArticleManagement.DTOs;

public class ArticleDetailsDto
{
    public int Id { get; set; }

    public Status Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<ContentDetailsDto> Contents { get; set; }
}
