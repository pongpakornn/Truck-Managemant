namespace NexusTransport.Api.Models;
public class Driver {
 public int Id { get; set; }
 public string FullName { get; set; } = "";
 public string PhoneNumber { get; set; } = "";
 public string LicenseNumber { get; set; } = "";
 public string Status { get; set; } = "Available";
 public ICollection<Truck> Trucks { get; set; } = new List<Truck>();
}
