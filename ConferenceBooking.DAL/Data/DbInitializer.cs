using ConferenceBooking.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBooking.DAL.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(AppDbContext dbContext)
    {
        await dbContext.Database.MigrateAsync();

        if (await dbContext.Rooms.AnyAsync())
        {
            return;
        }
        
        var projector = new Amenity { Id = Guid.NewGuid(), Name = "Проєктор", Price = 500 };
        var wifi = new Amenity { Id = Guid.NewGuid(), Name = "Wi-Fi", Price = 300 };
        var sound = new Amenity { Id = Guid.NewGuid(), Name = "Звук", Price = 700 };
        
        await dbContext.Amenities.AddRangeAsync(projector, wifi, sound);
        
        var roomA = new Room
        {
            Id = Guid.NewGuid(),
            Name = "Зал А",
            Capacity = 50,
            HourlyPrice = 2000,
            IsDeleted = false
        };
        
        var roomB = new Room
        {
            Id = Guid.NewGuid(),
            Name = "Зал B",
            Capacity = 100,
            HourlyPrice = 3500,
            IsDeleted = false
        };
        
        var roomC = new Room
        {
            Id = Guid.NewGuid(),
            Name = "Зал C",
            Capacity = 30,
            HourlyPrice = 1500,
            IsDeleted = false
        };
        
        await dbContext.Rooms.AddRangeAsync(roomA, roomB, roomC);
        
        var booking1 = new Booking
        {
            Id = Guid.NewGuid(),
            RoomId = roomA.Id,
            StartTime = new DateTime(2026, 8, 15, 10, 0, 0, DateTimeKind.Utc),
            EndTime = new DateTime(2026, 8, 15, 13, 0, 0, DateTimeKind.Utc),
            TotalPrice = 7100m,
            Amenities = [projector, wifi]
        };
        
        var booking2 = new Booking
        {
            Id = Guid.NewGuid(),
            RoomId = roomB.Id,
            StartTime = new DateTime(2026, 9, 01, 14, 0, 0, DateTimeKind.Utc),
            EndTime = new DateTime(2026, 9, 01, 18, 0, 0, DateTimeKind.Utc),
            TotalPrice = 15500m,
            Amenities = [projector, wifi, sound]
        };
        
        var booking3 = new Booking
        {
            Id = Guid.NewGuid(),
            RoomId = roomC.Id,
            StartTime = new DateTime(2026, 9, 10, 7, 0, 0, DateTimeKind.Utc),
            EndTime = new DateTime(2026, 9, 10, 9, 0, 0, DateTimeKind.Utc),
            TotalPrice = 3400m,
            Amenities = [sound]
        };
        
        var booking4 = new Booking
        {
            Id = Guid.NewGuid(),
            RoomId = roomA.Id,
            StartTime = new DateTime(2026, 9, 20, 18, 0, 0, DateTimeKind.Utc),
            EndTime = new DateTime(2026, 9, 20, 20, 0, 0, DateTimeKind.Utc),
            TotalPrice = 3200m,
            Amenities = []
        };
        
        await dbContext.Bookings.AddRangeAsync(booking1, booking2, booking3, booking4);
        await dbContext.SaveChangesAsync();
    }
}