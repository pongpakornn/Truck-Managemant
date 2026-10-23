using Microsoft.EntityFrameworkCore;
using NexusTransport.Api.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TransportDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("NexusTransport")));
builder.Services.AddCors(options => options.AddPolicy("Frontend", policy => policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod()));
var app = builder.Build();
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
app.UseCors("Frontend"); app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "ok", service = "NEXUS Transport API" }));
app.MapGet("/", () => Results.Redirect("/swagger"));
app.Run();
