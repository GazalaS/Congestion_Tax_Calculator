using Congestion.Calculator.Model;
using Microsoft.EntityFrameworkCore;

namespace Congestion.Calculator.Repository
{
    public class ApiContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "CityDb");
        }
        public DbSet<City> Cities { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
    }
}

