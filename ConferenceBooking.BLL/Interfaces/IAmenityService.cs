using ConferenceBooking.BLL.DTOs.Amenity;

namespace ConferenceBooking.BLL.Interfaces;

public interface IAmenityService
{
    Task<Guid> CreateAsync(CreateAmenityRequest request);
    Task<GetAmenityResponse> GetByIdAsync(Guid id);
}