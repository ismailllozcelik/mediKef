namespace MediKef.Api.DTOs;

/// <summary>
/// LisBox'tan gelen cihaz sonuç verisi
/// </summary>
public class LisBoxResultDto
{
    public string DeviceId { get; set; } = string.Empty;
    public string SampleBarcode { get; set; } = string.Empty;
    public List<TestResultItemDto> TestResults { get; set; } = new();
    public string Status { get; set; } = "Final";
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class TestResultItemDto
{
    public string TestCode { get; set; } = string.Empty;
    public string TestName { get; set; } = string.Empty;
    public string ResultValue { get; set; } = string.Empty;
    public decimal? ResultNumeric { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string ReferenceRange { get; set; } = string.Empty;
    public string Flag { get; set; } = "N"; // N=Normal, H=High, L=Low, HH=Very High, LL=Very Low
    public DateTime ResultDateTime { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// LisBox'a gönderilecek yanıt
/// </summary>
public class LisBoxResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? SampleId { get; set; }
    public int? ProcessedResults { get; set; }
}

