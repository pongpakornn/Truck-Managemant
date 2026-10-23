using Microsoft.EntityFrameworkCore;
using NexusTransport.Api.Models;
namespace NexusTransport.Api.Data;
public class TransportDbContext(DbContextOptions<TransportDbContext> options) : DbContext(options) {
 public DbSet<Shipment> Shipments => Set<Shipment>();
 public DbSet<Truck> Trucks => Set<Truck>();
 public DbSet<Driver> Drivers => Set<Driver>();
 protected override void OnModelCreating(ModelBuilder modelBuilder) {
  modelBuilder.Entity<Shipment>(e => {
   e.HasIndex(x => x.Code).IsUnique();
   e.Property(x => x.Code).HasMaxLength(40).IsRequired();
   e.Property(x => x.Origin).HasMaxLength(160).IsRequired();
   e.Property(x => x.Destination).HasMaxLength(160).IsRequired();
   e.Property(x => x.WeightKg).HasPrecision(12,2);
   e.HasOne(x => x.Truck).WithMany(x => x.Shipments).HasForeignKey(x => x.TruckId).OnDelete(DeleteBehavior.SetNull);
  });
  modelBuilder.Entity<Truck>(e => {
   e.HasIndex(x => x.Code).IsUnique();
   e.Property(x => x.Code).HasMaxLength(40).IsRequired();
   e.Property(x => x.RegistrationNumber).HasMaxLength(30).IsRequired();
   e.Property(x => x.CapacityKg).HasPrecision(12,2);
   e.Property(x => x.Status).HasMaxLength(30);
   e.HasOne(x => x.Driver).WithMany(x => x.Trucks).HasForeignKey(x => x.DriverId).OnDelete(DeleteBehavior.SetNull);
  });
  modelBuilder.Entity<Driver>(e => {
   e.Property(x => x.FullName).HasMaxLength(160).IsRequired();
   e.Property(x => x.PhoneNumber).HasMaxLength(40);
   e.Property(x => x.LicenseNumber).HasMaxLength(80);
   e.Property(x => x.Status).HasMaxLength(30);
  });
 }
}
