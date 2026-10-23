namespace NexusTransport.Api.Models;
public enum ShipmentStatus { Ready = 0, Assigned = 1, InTransit = 2, Delivered = 3, Cancelled = 4 }
public enum ShipmentPriority { Standard = 0, Express = 1, Priority = 2 }
public class Shipment {
 public int Id { get; set; }
 public string Code { get; set; } = "";
 public string Origin { get; set; } = "";
 public string Destination { get; set; } = "";
 public int Pallets { get; set; }
 public decimal WeightKg { get; set; }
 public ShipmentPriority Priority { get; set; } = ShipmentPriority.Standard;
 public ShipmentStatus Status { get; set; } = ShipmentStatus.Ready;
 public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
 public int? TruckId { get; set; }
 public Truck? Truck { get; set; }
}
