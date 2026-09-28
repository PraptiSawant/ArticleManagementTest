namespace ArticleManagement.Entities;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Content> Contents { get; set; }

}
