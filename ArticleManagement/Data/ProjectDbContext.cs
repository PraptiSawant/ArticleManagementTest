using Microsoft.EntityFrameworkCore;

namespace ArticleManagement.Data;

public class ProjectDbContext : DbContext
{
    public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
    {
    }
    public DbSet<Entities.User> Users { get; set; }
    public DbSet<Entities.Content> Contents { get; set; }
    public DbSet<Entities.Article> Articles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Entities.Content>()
            .HasOne(u => u.Author)
            .WithMany(c => c.Contents)
            .HasForeignKey(c => c.AuthorId);

        modelBuilder.Entity<Entities.Content>()
            .HasOne(a => a.Article)
            .WithMany(c => c.Contents)
            .HasForeignKey(c => c.ArticleId);
    }
}
