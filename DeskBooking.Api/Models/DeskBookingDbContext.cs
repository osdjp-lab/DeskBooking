using Microsoft.EntityFrameworkCore;

namespace DeskBooking.Data
{
    public class DeskBookingDbContext : DbContext
    {
        public DeskBookingDbContext(DbContextOptions<DeskBookingDbContext> options) : base(options)
        {
            
        }

        public DbSet<Desk>? Desks { get; set; }
        public DbSet<Location>? Locations { get; set; }
        public DbSet<SystemUser>? SystemUsers { get; set; }
        public DbSet<Reservation>? Reservations { get; set; }
    }
}
