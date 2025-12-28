using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediKef.Api.Models;

[Table("lisbox_logs")]
public class LisBoxLog
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("device_id")]
    public int? DeviceId { get; set; }

    [Column("sample_barcode")]
    [MaxLength(50)]
    public string? SampleBarcode { get; set; }

    [Column("raw_data")]
    public string? RawData { get; set; }

    [Column("parsed_data", TypeName = "jsonb")]
    public string? ParsedData { get; set; }

    [Column("status")]
    [MaxLength(50)]
    public string? Status { get; set; }

    [Column("error_message")]
    public string? ErrorMessage { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("DeviceId")]
    public Device? Device { get; set; }
}

