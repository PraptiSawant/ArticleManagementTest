namespace ArticleManagement.Entities;

public class Content
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }


    public int AuthorId { get; set; } // FK
    public User Author { get; set; }



    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public Language Language { get; set; }



    public int ArticleId { get; set; } //FK

    public Article Article { get; set; }

}
