using ArticleManagement.Entities;

namespace ArticleManagement.DTOs;

public class ArticleListDto
{
    public int Id { get; set; }
    public int ArticleId { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public Status Status { get; set; }
}
