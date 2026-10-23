using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusTransport.Api.Data;
using NexusTransport.Api.Models;
namespace NexusTransport.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class DriversController(TransportDbContext db) : ControllerBase {
 [HttpGet]
 public async Task<ActionResult<IEnumerable<Driver>>> GetAll()=>Ok(await db.Drivers.AsNoTracking().OrderBy(x=>x.FullName).ToListAsync());
 public record CreateDriverRequest(string FullName,string PhoneNumber,string LicenseNumber);
 [HttpPost]
 public async Task<ActionResult<Driver>> Create(CreateDriverRequest request) {
  if(string.IsNullOrWhiteSpace(request.FullName))return BadRequest(new {message="Driver name is required."});
  var driver=new Driver{FullName=request.FullName.Trim(),PhoneNumber=request.PhoneNumber?.Trim()??"",LicenseNumber=request.LicenseNumber?.Trim()??""};
  db.Drivers.Add(driver);await db.SaveChangesAsync();return CreatedAtAction(nameof(GetAll),new{id=driver.Id},driver);
 }
}
