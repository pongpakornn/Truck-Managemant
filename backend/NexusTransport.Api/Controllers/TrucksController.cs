using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusTransport.Api.Data;
using NexusTransport.Api.Models;
namespace NexusTransport.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TrucksController(TransportDbContext db) : ControllerBase {
 [HttpGet]
 public async Task<ActionResult<IEnumerable<Truck>>> GetAll()=>Ok(await db.Trucks.AsNoTracking().Include(x=>x.Driver).Include(x=>x.Shipments).OrderBy(x=>x.Code).ToListAsync());
 public record CreateTruckRequest(string Code,string RegistrationNumber,decimal CapacityKg,int? DriverId);
 [HttpPost]
 public async Task<ActionResult<Truck>> Create(CreateTruckRequest request) {
  if(string.IsNullOrWhiteSpace(request.Code)||string.IsNullOrWhiteSpace(request.RegistrationNumber)||request.CapacityKg<=0)return BadRequest(new {message="Truck code, registration and positive capacity are required."});
  if(await db.Trucks.AnyAsync(x=>x.Code==request.Code.Trim()))return Conflict(new {message="Truck code already exists."});
  if(request.DriverId.HasValue&&!await db.Drivers.AnyAsync(x=>x.Id==request.DriverId.Value))return BadRequest(new {message="Driver does not exist."});
  var truck=new Truck{Code=request.Code.Trim(),RegistrationNumber=request.RegistrationNumber.Trim(),CapacityKg=request.CapacityKg,DriverId=request.DriverId};
  db.Trucks.Add(truck);await db.SaveChangesAsync();return CreatedAtAction(nameof(GetAll),new{id=truck.Id},truck);
 }
 [HttpPost("{truckId:int}/assign/{shipmentId:int}")]
 public async Task<IActionResult> AssignShipment(int truckId,int shipmentId) {
  var truck=await db.Trucks.Include(x=>x.Shipments).FirstOrDefaultAsync(x=>x.Id==truckId);
  var shipment=await db.Shipments.FindAsync(shipmentId);
  if(truck is null||shipment is null)return NotFound();
  if(shipment.Status!=ShipmentStatus.Ready)return Conflict(new {message="Only Ready shipments can be assigned."});
  var load=truck.Shipments.Where(x=>x.Status!=ShipmentStatus.Cancelled).Sum(x=>x.WeightKg);
  if(load+shipment.WeightKg>truck.CapacityKg)return Conflict(new {message="Assignment would exceed truck capacity."});
  shipment.TruckId=truck.Id;shipment.Status=ShipmentStatus.Assigned;await db.SaveChangesAsync();
  return Ok(new {message="Shipment assigned.",truck.Code,shipment.Code,totalLoadKg=load+shipment.WeightKg,truck.CapacityKg});
 }
}
