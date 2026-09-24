using ConferenceBooking.DAL.Data;
using ConferenceBooking.DAL.Interfaces;
using ConferenceBooking.DAL.Models;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<Amenity>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        return await dbContext.Amenities
            .Where(a => ids.Contains(a.Id))
            .ToListAsync();
    }
}