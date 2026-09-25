using ConferenceBooking.DAL.Data;
using ConferenceBooking.DAL.Interfaces;
using ConferenceBooking.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBooking.DAL.Repositories;

public class RoomRepository(AppDbContext dbContext): IRoomRepository
{
    public async Task AddAsync(Room room)
    {
        await dbContext.Rooms.AddAsync(room);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Room?> GetByIdAsync(Guid id)
    {
        return await dbContext.Rooms
            .Include(r => r.Amenities)
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
    }
    
    public async Task UpdateAsync(Room room)
    {
        dbContext.Rooms.Update(room);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<Room>> GetAvailableRoomsAsync(DateTime start, DateTime end, int minCapacity)
    {
        return await dbContext.Rooms
            .Include(r => r.Amenities)
            .Where(r => !r.IsDeleted 
                        && r.Capacity >= minCapacity
                        && !r.Bookings.Any(b => b.StartTime < end && b.EndTime > start))
            .ToListAsync();
    }

    public async Task<List<Room>> GetAllAsync()
    {
        return await dbContext.Rooms.ToListAsync();
    }
}