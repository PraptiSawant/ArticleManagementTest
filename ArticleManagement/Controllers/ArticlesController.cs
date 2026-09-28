using ArticleManagement.Data;
using ArticleManagement.DTOs;
using ArticleManagement.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

namespace ArticleManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly ProjectDbContext _context;

    public ArticlesController(ProjectDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateArticleDto articleDto)
    {
        var article = new Article
        {
            Status = articleDto.Status,
            CreatedAt = DateTime.UtcNow
        };

        _context.Articles.Add(article);

        await _context.SaveChangesAsync();

        return Ok(article);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var articles = await _context.Articles.ToListAsync();
        return Ok(articles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var article = await _context.Articles.FindAsync(id);

        if (article == null)
            return NotFound();

        return Ok(article);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateArticleDto articleDto)
    {
        var article = await _context.Articles.FindAsync(id);

        if (article == null)
            return NotFound();

        article.Status = articleDto.Status;

        await _context.SaveChangesAsync();

        return Ok(article);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var article = await _context.Articles.FindAsync(id);

        if (article == null)
            return NotFound();

        try
        {
            _context.Articles.Remove(article);

            await _context.SaveChangesAsync();

            return Ok();
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "Cannot delete this article because it still has associated content records. Delete or un-link the contents first.");
        }
    }

    //Article details call with list of all content items
    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetArticleDetails(int id)
    {
        var article = await _context.Articles
            .Include(a => a.Contents)
            .ThenInclude(u => u.Author)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (article == null)
        {
            return NotFound();
        }

        var result = new ArticleDetailsDto
        {
            Id = article.Id,
            Status = article.Status,
            CreatedAt = article.CreatedAt,

            Contents = article.Contents
                .Select(c => new ContentDetailsDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Content = c.Body,
                    Author = new AuthorDetailsDto
                    {
                        Name = c.Author.UserName,
                    },
                    Status = c.Status.GetDisplayName(),
                    CreatedAt = c.CreatedAt,
                    Language = c.Language.GetDisplayName()
                })
                .ToList()
        };

        return Ok(result);
    }

    //Pagination and filter
    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged([FromQuery] ArticleListQueryDto request)
    {
        var query = _context.Articles
        .Include(a => a.Contents)
        .ThenInclude(c => c.Author)
        .AsQueryable();

        if (request.Status.HasValue)
        {
            query = query.Where(a => a.Status == request.Status.Value);
        }

        if (request.SortBy?.ToLower() == "title")
        {
            query = query.OrderBy(a =>
                a.Contents
                    .Where(c => c.Language == Language.English)
                    .Select(c => c.Title)
                    .FirstOrDefault());
        }
        else
        {
            query = query.OrderByDescending(a => a.CreatedAt);
        }

        var totalItems = await query.CountAsync();

        var articles = await query
             .Skip((request.PageNumber - 1) * request.PageSize)
             .Take(request.PageSize)
             .ToListAsync();

        var result = articles.Select(a => new ArticleListDto
        {
            Id = a.Id,

            Title = a.Contents
        .Where(c => c.Language == Language.English)
        .Select(c => c.Title)
        .FirstOrDefault(),

            Author = a.Contents
        .Select(c => c.Author.UserName)
        .FirstOrDefault(),

            Status = a.Status
        }).ToList();

        return Ok(new
        {
            Page = request.PageNumber,
            PageSize = request.PageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling((double)totalItems / request.PageSize),
            Data = result
        });
    }

}

