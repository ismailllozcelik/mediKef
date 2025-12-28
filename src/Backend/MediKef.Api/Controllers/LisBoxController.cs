using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediKef.Api.Data;
using MediKef.Api.DTOs;
using MediKef.Api.Models;
using System.Text.Json;

namespace MediKef.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LisBoxController : ControllerBase
{
    private readonly MediKefDbContext _context;
    private readonly ILogger<LisBoxController> _logger;
    private readonly IConfiguration _configuration;

    public LisBoxController(
        MediKefDbContext context,
        ILogger<LisBoxController> logger,
        IConfiguration configuration)
    {
        _context = context;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// LisBox'tan cihaz sonuçlarını al
    /// </summary>
    [HttpPost("receive-results")]
    public async Task<ActionResult<LisBoxResponseDto>> ReceiveResults([FromBody] LisBoxResultDto resultDto)
    {
        try
        {
            _logger.LogInformation("LisBox'tan sonuç alındı. Cihaz: {DeviceId}, Barkod: {Barcode}", 
                resultDto.DeviceId, resultDto.SampleBarcode);

            // 1. API Key kontrolü (güvenlik)
            if (!ValidateApiKey())
            {
                _logger.LogWarning("Geçersiz API Key");
                return Unauthorized(new LisBoxResponseDto 
                { 
                    Success = false, 
                    Message = "Geçersiz API Key" 
                });
            }

            // 2. Cihaz kontrolü
            var device = await _context.Devices
                .FirstOrDefaultAsync(d => d.DeviceId == resultDto.DeviceId);

            if (device == null)
            {
                _logger.LogWarning("Cihaz bulunamadı: {DeviceId}", resultDto.DeviceId);
                await LogLisBoxRequest(null, resultDto, "Failed", "Cihaz bulunamadı");
                return NotFound(new LisBoxResponseDto 
                { 
                    Success = false, 
                    Message = $"Cihaz bulunamadı: {resultDto.DeviceId}" 
                });
            }

            // 3. Numune kontrolü
            var sample = await _context.Samples
                .Include(s => s.SampleTests)
                    .ThenInclude(st => st.Test)
                .FirstOrDefaultAsync(s => s.Barcode == resultDto.SampleBarcode);

            if (sample == null)
            {
                _logger.LogWarning("Numune bulunamadı: {Barcode}", resultDto.SampleBarcode);
                await LogLisBoxRequest(device.Id, resultDto, "Failed", "Numune bulunamadı");
                return NotFound(new LisBoxResponseDto 
                { 
                    Success = false, 
                    Message = $"Numune bulunamadı: {resultDto.SampleBarcode}" 
                });
            }

            // 4. Test sonuçlarını kaydet
            int processedCount = 0;
            foreach (var testResultItem in resultDto.TestResults)
            {
                // Test koduna göre test bul
                var test = await _context.Tests
                    .FirstOrDefaultAsync(t => t.TestCode == testResultItem.TestCode);

                if (test == null)
                {
                    _logger.LogWarning("Test bulunamadı: {TestCode}", testResultItem.TestCode);
                    continue;
                }

                // Bu numune için bu test talep edilmiş mi?
                var sampleTest = sample.SampleTests
                    .FirstOrDefault(st => st.TestId == test.Id);

                if (sampleTest == null)
                {
                    _logger.LogWarning("Bu numune için test talep edilmemiş: {TestCode}", testResultItem.TestCode);
                    continue;
                }

                // Test sonucunu kaydet
                var testResult = new TestResult
                {
                    SampleTestId = sampleTest.Id,
                    DeviceId = device.Id,
                    ResultValue = testResultItem.ResultValue,
                    ResultNumeric = testResultItem.ResultNumeric,
                    Unit = testResultItem.Unit,
                    ReferenceRange = testResultItem.ReferenceRange,
                    Flag = testResultItem.Flag,
                    ResultStatus = resultDto.Status,
                    ResultDate = testResultItem.ResultDateTime,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.TestResults.Add(testResult);

                // SampleTest durumunu güncelle
                sampleTest.Status = "Completed";

                processedCount++;
            }

            // 5. Numune durumunu güncelle
            var allTestsCompleted = sample.SampleTests.All(st => st.Status == "Completed");
            if (allTestsCompleted)
            {
                sample.Status = "Completed";
                sample.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                sample.Status = "InProgress";
                sample.UpdatedAt = DateTime.UtcNow;
            }

            // 6. Değişiklikleri kaydet
            await _context.SaveChangesAsync();

            // 7. Log kaydet
            await LogLisBoxRequest(device.Id, resultDto, "Success", null);

            _logger.LogInformation("Sonuçlar başarıyla kaydedildi. İşlenen test sayısı: {Count}", processedCount);

            return Ok(new LisBoxResponseDto
            {
                Success = true,
                Message = "Sonuçlar başarıyla kaydedildi",
                SampleId = sample.SampleId,
                ProcessedResults = processedCount
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LisBox sonuç işleme hatası");
            return StatusCode(500, new LisBoxResponseDto
            {
                Success = false,
                Message = $"Sunucu hatası: {ex.Message}"
            });
        }
    }

    private bool ValidateApiKey()
    {
        if (!Request.Headers.TryGetValue("X-API-Key", out var apiKey))
        {
            return false;
        }

        var expectedApiKey = _configuration["LisBox:ApiKey"] ?? "LISBOX_SECRET_KEY_2024";
        return apiKey == expectedApiKey;
    }

    private async Task LogLisBoxRequest(int? deviceId, LisBoxResultDto resultDto, string status, string? errorMessage)
    {
        var log = new LisBoxLog
        {
            DeviceId = deviceId,
            SampleBarcode = resultDto.SampleBarcode,
            RawData = JsonSerializer.Serialize(resultDto),
            ParsedData = JsonSerializer.Serialize(resultDto),
            Status = status,
            ErrorMessage = errorMessage,
            CreatedAt = DateTime.UtcNow
        };

        _context.LisBoxLogs.Add(log);
        await _context.SaveChangesAsync();
    }
}

