using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediKef.Api.Models;

[Table("samples")]
public class Sample
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("sample_id")]
    [MaxLength(50)]
    public string SampleId { get; set; } = string.Empty;

    [Required]
    [Column("barcode")]
    [MaxLength(50)]
    public string Barcode { get; set; } = string.Empty;

    [Required]
    [Column("patient_id")]
    public int PatientId { get; set; }

    [Column("sample_type")]
    [MaxLength(50)]
    public string? SampleType { get; set; }

    [Column("collection_date")]
    public DateTime CollectionDate { get; set; } = DateTime.UtcNow;

    [Column("received_date")]
    public DateTime? ReceivedDate { get; set; }

    [Column("status")]
    [MaxLength(50)]
    public string Status { get; set; } = "Pending";

    [Column("priority")]
    [MaxLength(20)]
    public string Priority { get; set; } = "Normal";

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("created_by")]
    [MaxLength(100)]
    public string? CreatedBy { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("PatientId")]
    public Patient Patient { get; set; } = null!;
    
    public ICollection<SampleTest> SampleTests { get; set; } = new List<SampleTest>();
}

