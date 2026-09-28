using ArticleManagement.Entities;

namespace ArticleManagement.DTOs;

public class CreateContentDto
{
    public string Title { get; set; }

    public string Body { get; set; }

    public int AuthorId { get; set; }

    public int ArticleId { get; set; }

    public Language Language { get; set; }

    public Status Status { get; set; }
}
