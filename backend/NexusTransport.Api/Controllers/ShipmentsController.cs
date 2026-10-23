using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusTransport.Api.Data;
using NexusTransport.Api.Models;
namespace NexusTransport.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ShipmentsController(TransportDbContext db) : ControllerBase {
 [HttpGet]
 public async Task<ActionResult<IEnumerable<Shipment>>> GetAll([FromQuery] string? search, [FromQuery] ShipmentStatus? status) {
  var q=db.Shipments.AsNoTracking().Include(x=>x.Truck).AsQueryable();
  if(!string.IsNullOrWhiteSpace(search)) q=q.Where(x=>x.Code.Contains(search)||x.Origin.Contains(search)||x.Destination.Contains(search));
  if(status.HasValue) q=q.Where(x=>x.Status==status.Value);
  return Ok(await q.OrderByDescending(x=>x.CreatedAtUtc).ToListAsync());
 }
 [HttpGet("{id:int}")]
 public async Task<ActionResult<Shipment>> GetById(int id) {
  var item=await db.Shipments.AsNoTracking().Include(x=>x.Truck).FirstOrDefaultAsync(x=>x.Id==id);
  return item is null?NotFound():Ok(item);
 }
 public record CreateShipmentRequest(string Code,string Origin,string Destination,int Pallets,decimal WeightKg,ShipmentPriority Priority);
 [HttpPost]
 public async Task<ActionResult<Shipment>> Create(CreateShipmentRequest request) {
  if(string.IsNullOrWhiteSpace(request.Code)||string.IsNullOrWhiteSpace(request.Origin)||string.IsNullOrWhiteSpace(request.Destination)||request.Pallets<0||request.WeightKg<=0)
   return BadRequest(new {message="Code, origin, destination, non-negative pallets and positive weight are required."});
  var code=request.Code.Trim();
  if(await db.Shipments.AnyAsync(x=>x.Code==code)) return Conflict(new {message="Shipment code already exists."});
  var item=new Shipment{Code=code,Origin=request.Origin.Trim(),Destination=request.Destination.Trim(),Pallets=request.Pallets,WeightKg=request.WeightKg,Priority=request.Priority};
  db.Shipments.Add(item); await db.SaveChangesAsync();
  return CreatedAtAction(nameof(GetById),new{id=item.Id},item);
 }
 public record UpdateStatusRequest(ShipmentStatus Status);
 [HttpPatch("{id:int}/status")]
 public async Task<IActionResult> UpdateStatus(int id,UpdateStatusRequest request) {
  var item=await db.Shipments.FindAsync(id); if(item is null)return NotFound();
  if(item.Status is ShipmentStatus.Delivered or ShipmentStatus.Cancelled)return Conflict(new {message="Delivered or cancelled shipments cannot be changed."});
  item.Status=request.Status; await db.SaveChangesAsync(); return NoContent();
 }
}
