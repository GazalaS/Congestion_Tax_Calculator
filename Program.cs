using Congestion.Calculator.Repository;
using Congestion.Calculator.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Ensure the DbContext is configured with the correct connection string
//builder.Services.AddDbContext<ApiContext>(options =>
  //  options.UseSqlServer(builder.Configuration.GetConnectionString("YourConnectionString")));

// Register repositories and services
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<ICongestionTaxCalculatorService, CongestionTaxCalculatorService>();

// Add controllers
builder.Services.AddControllers();

// Add Swagger/OpenAPI configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add authentication if necessary
// app.UseAuthentication();

app.UseAuthorization();

// Map controllers to endpoints
app.MapControllers();

app.Run();
