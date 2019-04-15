using LocationService.Models;
using Microsoft.EntityFrameworkCore;

namespace LocationService.Persistence
{
    public class LocationDbContext : DbContext
    {
        public LocationDbContext(DbContextOptions<LocationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<LocationRecord> LocationRecords { get; set; }
    }
}
