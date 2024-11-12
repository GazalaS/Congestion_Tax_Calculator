using Microsoft.EntityFrameworkCore;
using Congestion.Calculator.Model;
using Congestion.Calculator.Model.Calendar;  // Ensure this import points to your model classes

public class ApiContext : DbContext
{
    public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

    // DbSets for your models
    public DbSet<City> Cities { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<HolidayCalendar> HolidayCalendars { get; set; }
    public DbSet<WorkingCalendar> WorkingCalendars { get; set; }
    public DbSet<HolidayMonths> HolidayMonths { get; set; }
    public DbSet<Tariff> Tariffs { get; set; }
    public DbSet<CityPreference> CityPreferences { get; set; } // Added CityPreference

    // OnModelCreating to configure relationships and constraints
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the one-to-one relationship between City and CityPreference
        modelBuilder.Entity<City>()
            .HasOne(c => c.CityPreference)    // One CityPreference per City
            .WithOne()                         // CityPreference has no navigation property for City (it's unidirectional)
            .HasForeignKey<CityPreference>(cp => cp.CityPreferenceId); // Foreign key in CityPreference

        // Configure the one-to-many relationship between Vehicle and VehicleType
        modelBuilder.Entity<Vehicle>()
            .Property(v => v.Type)            // Ensures that the enum is stored as an integer
            .HasConversion<int>();            // VehicleType enum is stored as integer in the database

        // Configure the many-to-one relationship between Tariff and City
        modelBuilder.Entity<Tariff>()
            .HasOne(t => t.City)  // A Tariff is associated with one City
            .WithMany(c => c.Tariffs) // A City can have many Tariffs
            .HasForeignKey(t => t.CityId);  // Foreign key in Tariff (if Tariff contains CityId)

        // Configure the many-to-one relationship between WorkingCalendar and City
        modelBuilder.Entity<WorkingCalendar>()
            .HasOne(w => w.City)  // A WorkingCalendar is associated with one City
            .WithMany(c => c.WorkingCalendars)  // A City can have many WorkingCalendars
            .HasForeignKey(w => w.CityId);  // Foreign key in WorkingCalendar

        // Configure the many-to-one relationship between HolidayCalendar and City
        modelBuilder.Entity<HolidayCalendar>()
            .HasOne(h => h.City)  // A HolidayCalendar is associated with one City
            .WithMany(c => c.HolidayCalendars)  // A City can have many HolidayCalendars
            .HasForeignKey(h => h.CityId);  // Foreign key in HolidayCalendar

        // Configure the one-to-many relationship between City and HolidayMonths
        modelBuilder.Entity<HolidayMonths>()
            .HasOne(hm => hm.City)  // A HolidayMonths is associated with one City
            .WithMany(c => c.HolidayMonths)  // A City can have many HolidayMonths
            .HasForeignKey(hm => hm.CityId);  // Foreign key in HolidayMonths
    }

}
