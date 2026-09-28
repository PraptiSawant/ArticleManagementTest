using ArticleManagement.Data;
using ArticleManagement.DTOs;
using ArticleManagement.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ArticleManagement.Controllers;

[ApiController]
[Route("api/[controller]")] 
public class ContentsController : ControllerBase
{
    private readonly ProjectDbContext _context;

    public ContentsController(ProjectDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateContentDto contentDto)
    {
        var content = new Content
        {
            Title = contentDto.Title,
            Body = contentDto.Body,
            AuthorId = contentDto.AuthorId,
            ArticleId = contentDto.ArticleId,
            Language = contentDto.Language,
            Status = contentDto.Status,
            CreatedAt = DateTime.UtcNow
        };
        _context.Contents.Add(content);
        await _context.SaveChangesAsync();
        return Ok(content);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var contents = await _context.Contents.ToListAsync();
        return Ok(contents);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var content = await _context.Contents.FindAsync(id);

        if (content == null)
            return NotFound();

        return Ok(content);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateContentDto contentDto)
    {
        var content = await _context.Contents.FindAsync(id);

        if (content == null)
            return NotFound();

        content.Title = contentDto.Title;
        content.Body = contentDto.Body;
        content.AuthorId = contentDto.AuthorId;
        content.ArticleId = contentDto.ArticleId;
        content.Language = contentDto.Language;
        content.Status = contentDto.Status;
        content.CreatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(content);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var content = await _context.Contents.FindAsync(id);

        if (content == null)
            return NotFound();

        _context.Contents.Remove(content);

        await _context.SaveChangesAsync();

        return Ok();
    }

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
