using ConferenceBooking.DAL.Models;

namespace ConferenceBooking.DAL.Interfaces;

public interface IAmenityRepository
{
    Task AddAsync(Amenity amenity);
    Task<Amenity?> GetByIdAsync(Guid id);
}