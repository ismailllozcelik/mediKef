using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediKef.Api.Data;
using MediKef.Api.Models;

namespace MediKef.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestsController : ControllerBase
{
    private readonly MediKefDbContext _context;

    public TestsController(MediKefDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Test>>> GetTests([FromQuery] string? category)
    {
        var query = _context.Tests.Where(t => t.IsActive).AsQueryable();

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(t => t.TestCategory == category);
        }

        return await query.OrderBy(t => t.TestCategory).ThenBy(t => t.TestName).ToListAsync();
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<string>>> GetCategories()
    {
        var categories = await _context.Tests
            .Where(t => t.IsActive && t.TestCategory != null)
            .Select(t => t.TestCategory!)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();

        return categories;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Test>> GetTest(int id)
    {
        var test = await _context.Tests.FindAsync(id);

        if (test == null)
        {
            return NotFound();
        }

        return test;
    }
}

