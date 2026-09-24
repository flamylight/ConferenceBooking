using ConferenceBooking.DAL.Data;
using ConferenceBooking.DAL.Interfaces;
using ConferenceBooking.DAL.Models;

namespace ConferenceBooking.DAL.Repositories;

public class AmenityRepository(AppDbContext dbContext): IAmenityRepository
{
    public async Task AddAsync(Amenity amenity)
    {
        await dbContext.Amenities.AddAsync(amenity);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task<Amenity?> GetByIdAsync(Guid id)
    {
        return await dbContext.Amenities.FindAsync(id);
    }
}