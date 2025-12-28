using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediKef.Api.Models;

[Table("patients")]
public class Patient
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("patient_id")]
    [MaxLength(50)]
    public string PatientId { get; set; } = string.Empty;

    [Column("tc_no")]
    [MaxLength(11)]
    public string? TcNo { get; set; }

    [Required]
    [Column("first_name")]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [Column("last_name")]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Column("birth_date")]
    public DateTime? BirthDate { get; set; }

    [Column("gender")]
    [MaxLength(10)]
    public string? Gender { get; set; }

    [Column("phone")]
    [MaxLength(20)]
    public string? Phone { get; set; }

    [Column("email")]
    [MaxLength(100)]
    public string? Email { get; set; }

    [Column("address")]
    public string? Address { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Sample> Samples { get; set; } = new List<Sample>();
}

