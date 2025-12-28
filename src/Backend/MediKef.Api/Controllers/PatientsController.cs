using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediKef.Api.Data;
using MediKef.Api.Models;

namespace MediKef.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly MediKefDbContext _context;
    private readonly ILogger<PatientsController> _logger;

    public PatientsController(MediKefDbContext context, ILogger<PatientsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Patient>>> GetPatients([FromQuery] string? search)
    {
        var query = _context.Patients.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => 
                p.FirstName.Contains(search) || 
                p.LastName.Contains(search) ||
                p.PatientId.Contains(search) ||
                (p.TcNo != null && p.TcNo.Contains(search)));
        }

        return await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Patient>> GetPatient(int id)
    {
        var patient = await _context.Patients
            .Include(p => p.Samples)
                .ThenInclude(s => s.SampleTests)
                    .ThenInclude(st => st.Test)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (patient == null)
        {
            return NotFound();
        }

        return patient;
    }

    [HttpPost]
    public async Task<ActionResult<Patient>> CreatePatient(Patient patient)
    {
        // Generate Patient ID
        var lastPatient = await _context.Patients
            .OrderByDescending(p => p.Id)
            .FirstOrDefaultAsync();

        var nextId = (lastPatient?.Id ?? 0) + 1;
        patient.PatientId = $"P{DateTime.Now.Year}{nextId:D6}";
        patient.CreatedAt = DateTime.UtcNow;
        patient.UpdatedAt = DateTime.UtcNow;

        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePatient(int id, Patient patient)
    {
        if (id != patient.Id)
        {
            return BadRequest();
        }

        patient.UpdatedAt = DateTime.UtcNow;
        _context.Entry(patient).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await PatientExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatient(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null)
        {
            return NotFound();
        }

        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> PatientExists(int id)
    {
        return await _context.Patients.AnyAsync(e => e.Id == id);
    }
}

