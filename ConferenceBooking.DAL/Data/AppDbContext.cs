using ConferenceBooking.DAL.Configurations;
using ConferenceBooking.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBooking.DAL.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options)
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Amenity> Amenities { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RoomEntityTypeConfiguration).Assembly);
    }
}