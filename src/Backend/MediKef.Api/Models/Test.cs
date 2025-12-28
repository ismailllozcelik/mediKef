using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediKef.Api.Models;

[Table("tests")]
public class Test
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("test_code")]
    [MaxLength(20)]
    public string TestCode { get; set; } = string.Empty;

    [Required]
    [Column("test_name")]
    [MaxLength(200)]
    public string TestName { get; set; } = string.Empty;

    [Column("test_category")]
    [MaxLength(100)]
    public string? TestCategory { get; set; }

    [Column("unit")]
    [MaxLength(50)]
    public string? Unit { get; set; }

    [Column("reference_range_min")]
    public decimal? ReferenceRangeMin { get; set; }

    [Column("reference_range_max")]
    public decimal? ReferenceRangeMax { get; set; }

    [Column("reference_range_text")]
    [MaxLength(200)]
    public string? ReferenceRangeText { get; set; }

    [Column("sample_type")]
    [MaxLength(50)]
    public string? SampleType { get; set; }

    [Column("price")]
    public decimal? Price { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<SampleTest> SampleTests { get; set; } = new List<SampleTest>();
}

