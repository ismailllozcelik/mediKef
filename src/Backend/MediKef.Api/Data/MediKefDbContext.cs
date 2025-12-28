using Microsoft.EntityFrameworkCore;
using MediKef.Api.Models;

namespace MediKef.Api.Data;

public class MediKefDbContext : DbContext
{
    public MediKefDbContext(DbContextOptions<MediKefDbContext> options) : base(options)
    {
    }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<Test> Tests { get; set; }
    public DbSet<Sample> Samples { get; set; }
    public DbSet<SampleTest> SampleTests { get; set; }
    public DbSet<TestResult> TestResults { get; set; }
    public DbSet<LisBoxLog> LisBoxLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Patient
        modelBuilder.Entity<Patient>()
            .HasIndex(p => p.PatientId)
            .IsUnique();

        modelBuilder.Entity<Patient>()
            .HasIndex(p => p.TcNo)
            .IsUnique();

        // Device
        modelBuilder.Entity<Device>()
            .HasIndex(d => d.DeviceId)
            .IsUnique();

        // Test
        modelBuilder.Entity<Test>()
            .HasIndex(t => t.TestCode)
            .IsUnique();

        // Sample
        modelBuilder.Entity<Sample>()
            .HasIndex(s => s.SampleId)
            .IsUnique();

        modelBuilder.Entity<Sample>()
            .HasIndex(s => s.Barcode)
            .IsUnique();

        // SampleTest - Composite unique index
        modelBuilder.Entity<SampleTest>()
            .HasIndex(st => new { st.SampleId, st.TestId })
            .IsUnique();

        // Relationships
        modelBuilder.Entity<Sample>()
            .HasOne(s => s.Patient)
            .WithMany(p => p.Samples)
            .HasForeignKey(s => s.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SampleTest>()
            .HasOne(st => st.Sample)
            .WithMany(s => s.SampleTests)
            .HasForeignKey(st => st.SampleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SampleTest>()
            .HasOne(st => st.Test)
            .WithMany(t => t.SampleTests)
            .HasForeignKey(st => st.TestId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TestResult>()
            .HasOne(tr => tr.SampleTest)
            .WithMany(st => st.TestResults)
            .HasForeignKey(tr => tr.SampleTestId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TestResult>()
            .HasOne(tr => tr.Device)
            .WithMany(d => d.TestResults)
            .HasForeignKey(tr => tr.DeviceId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<LisBoxLog>()
            .HasOne(l => l.Device)
            .WithMany(d => d.LisBoxLogs)
            .HasForeignKey(l => l.DeviceId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

