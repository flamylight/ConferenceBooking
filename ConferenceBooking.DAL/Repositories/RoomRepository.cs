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
            .FirstOrDefaultAsync(r => r.Id == id);
    }
    
    public async Task UpdateAsync(Room room)
    {
        dbContext.Rooms.Update(room);
        await dbContext.SaveChangesAsync();
    }
}