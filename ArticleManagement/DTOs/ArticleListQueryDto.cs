using ArticleManagement.Entities;

namespace ArticleManagement.DTOs;

public class ArticleListQueryDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public Status? Status { get; set; }
    public string SortBy { get; set; } 
    public Language? Language { get; set; }
}
