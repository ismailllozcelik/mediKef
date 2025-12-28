using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediKef.Api.Models;

[Table("devices")]
public class Device
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("device_id")]
    [MaxLength(50)]
    public string DeviceId { get; set; } = string.Empty;

    [Required]
    [Column("device_name")]
    [MaxLength(100)]
    public string DeviceName { get; set; } = string.Empty;

    [Column("manufacturer")]
    [MaxLength(100)]
    public string? Manufacturer { get; set; }

    [Column("model")]
    [MaxLength(100)]
    public string? Model { get; set; }

    [Column("serial_number")]
    [MaxLength(100)]
    public string? SerialNumber { get; set; }

    [Column("device_type")]
    [MaxLength(50)]
    public string? DeviceType { get; set; }

    [Column("protocol")]
    [MaxLength(20)]
    public string? Protocol { get; set; }

    [Column("connection_type")]
    [MaxLength(20)]
    public string? ConnectionType { get; set; }

    [Column("ip_address")]
    [MaxLength(50)]
    public string? IpAddress { get; set; }

    [Column("port")]
    public int? Port { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    public ICollection<LisBoxLog> LisBoxLogs { get; set; } = new List<LisBoxLog>();
}

