using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<SmallApp.Models.Database>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();