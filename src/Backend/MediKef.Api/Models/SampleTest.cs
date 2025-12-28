using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediKef.Api.Models;

[Table("sample_tests")]
public class SampleTest
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("sample_id")]
    public int SampleId { get; set; }

    [Required]
    [Column("test_id")]
    public int TestId { get; set; }

    [Column("status")]
    [MaxLength(50)]
    public string Status { get; set; } = "Pending";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("SampleId")]
    public Sample Sample { get; set; } = null!;

    [ForeignKey("TestId")]
    public Test Test { get; set; } = null!;

    public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
}

