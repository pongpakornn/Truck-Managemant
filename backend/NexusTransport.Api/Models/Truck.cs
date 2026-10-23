namespace NexusTransport.Api.Models;
public class Truck {
 public int Id { get; set; }
 public string Code { get; set; } = "";
 public string RegistrationNumber { get; set; } = "";
 public decimal CapacityKg { get; set; }
 public string Status { get; set; } = "Available";
 public int? DriverId { get; set; }
 public Driver? Driver { get; set; }
 public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
}
