using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediKef.Api.Data;
using MediKef.Api.Models;

namespace MediKef.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SamplesController : ControllerBase
{
    private readonly MediKefDbContext _context;

    public SamplesController(MediKefDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Sample>>> GetSamples([FromQuery] string? status)
    {
        var query = _context.Samples
            .Include(s => s.Patient)
            .Include(s => s.SampleTests)
                .ThenInclude(st => st.Test)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(s => s.Status == status);
        }

        return await query.OrderByDescending(s => s.CreatedAt).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Sample>> GetSample(int id)
    {
        var sample = await _context.Samples
            .Include(s => s.Patient)
            .Include(s => s.SampleTests)
                .ThenInclude(st => st.Test)
            .Include(s => s.SampleTests)
                .ThenInclude(st => st.TestResults)
                    .ThenInclude(tr => tr.Device)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (sample == null)
        {
            return NotFound();
        }

        return sample;
    }

    [HttpGet("barcode/{barcode}")]
    public async Task<ActionResult<Sample>> GetSampleByBarcode(string barcode)
    {
        var sample = await _context.Samples
            .Include(s => s.Patient)
            .Include(s => s.SampleTests)
                .ThenInclude(st => st.Test)
            .Include(s => s.SampleTests)
                .ThenInclude(st => st.TestResults)
            .FirstOrDefaultAsync(s => s.Barcode == barcode);

        if (sample == null)
        {
            return NotFound();
        }

        return sample;
    }

    [HttpPost]
    public async Task<ActionResult<Sample>> CreateSample(CreateSampleDto dto)
    {
        // Generate Sample ID and Barcode
        var lastSample = await _context.Samples
            .OrderByDescending(s => s.Id)
            .FirstOrDefaultAsync();

        var nextId = (lastSample?.Id ?? 0) + 1;
        var sample = new Sample
        {
            SampleId = $"S{DateTime.Now.Year}{nextId:D6}",
            Barcode = $"BAR{DateTime.Now.Year}{nextId:D6}",
            PatientId = dto.PatientId,
            SampleType = dto.SampleType,
            Priority = dto.Priority ?? "Normal",
            Status = "Pending",
            CollectionDate = DateTime.UtcNow,
            CreatedBy = dto.CreatedBy,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Samples.Add(sample);
        await _context.SaveChangesAsync();

        // Add requested tests
        foreach (var testId in dto.TestIds)
        {
            var sampleTest = new SampleTest
            {
                SampleId = sample.Id,
                TestId = testId,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };
            _context.SampleTests.Add(sampleTest);
        }

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSample), new { id = sample.Id }, sample);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateSampleStatus(int id, [FromBody] string status)
    {
        var sample = await _context.Samples.FindAsync(id);
        if (sample == null)
        {
            return NotFound();
        }

        sample.Status = status;
        sample.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

public class CreateSampleDto
{
    public int PatientId { get; set; }
    public string? SampleType { get; set; }
    public string? Priority { get; set; }
    public List<int> TestIds { get; set; } = new();
    public string? CreatedBy { get; set; }
}

