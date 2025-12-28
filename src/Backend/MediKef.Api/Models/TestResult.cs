using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediKef.Api.Models;

[Table("test_results")]
public class TestResult
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("sample_test_id")]
    public int SampleTestId { get; set; }

    [Column("device_id")]
    public int? DeviceId { get; set; }

    [Column("result_value")]
    [MaxLength(200)]
    public string? ResultValue { get; set; }

    [Column("result_numeric")]
    public decimal? ResultNumeric { get; set; }

    [Column("unit")]
    [MaxLength(50)]
    public string? Unit { get; set; }

    [Column("reference_range")]
    [MaxLength(200)]
    public string? ReferenceRange { get; set; }

    [Column("flag")]
    [MaxLength(10)]
    public string? Flag { get; set; }

    [Column("result_status")]
    [MaxLength(50)]
    public string ResultStatus { get; set; } = "Preliminary";

    [Column("result_date")]
    public DateTime ResultDate { get; set; } = DateTime.UtcNow;

    [Column("validated_by")]
    [MaxLength(100)]
    public string? ValidatedBy { get; set; }

    [Column("validated_at")]
    public DateTime? ValidatedAt { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("SampleTestId")]
    public SampleTest SampleTest { get; set; } = null!;

    [ForeignKey("DeviceId")]
    public Device? Device { get; set; }
}

