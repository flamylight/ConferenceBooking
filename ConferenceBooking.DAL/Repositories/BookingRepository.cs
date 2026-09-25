using ConferenceBooking.DAL.Data;
using ConferenceBooking.DAL.Interfaces;
using ConferenceBooking.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBooking.DAL.Repositories;

public class BookingRepository(AppDbContext dbContext): IBookingRepository
{
    public async Task AddAsync(Booking booking)
    {
        await dbContext.Bookings.AddAsync(booking);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> HasOverlappingAsync(Guid roomId, DateTime start, DateTime end)
    {
        return await dbContext.Bookings
            .AnyAsync(b => b.RoomId == roomId 
                           && b.StartTime < end 
                           && b.EndTime > start);
    }

    public async Task<Booking?> GetByIdAsync(Guid id)
    {
        return await dbContext.Bookings
            .Include(b => b.Amenities)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<bool> HasFutureBookingsAsync(Guid roomId, DateTime start)
    {
        return await dbContext.Bookings
            .AnyAsync(b => b.RoomId == roomId && b.EndTime > start);
    }
}