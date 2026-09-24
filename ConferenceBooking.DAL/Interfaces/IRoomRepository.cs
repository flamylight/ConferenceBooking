using ConferenceBooking.DAL.Models;

namespace ConferenceBooking.DAL.Interfaces;

public interface IRoomRepository
{
    Task AddAsync(Room room);
    Task<Room?> GetByIdAsync(Guid id);
    Task UpdateAsync(Room room);
}