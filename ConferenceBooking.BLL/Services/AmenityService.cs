using ConferenceBooking.BLL.DTOs.Amenity;
using ConferenceBooking.BLL.Exceptions;
using ConferenceBooking.BLL.Interfaces;
using ConferenceBooking.DAL.Interfaces;
using ConferenceBooking.DAL.Models;
using FluentValidation;

namespace ConferenceBooking.BLL.Services;

public class AmenityService(
    IValidator<CreateAmenityRequest> createValidator,
    IAmenityRepository amenityRepository): IAmenityService
{
    public async Task<Guid> CreateAsync(CreateAmenityRequest request)
    {
        await createValidator.ValidateAndThrowAsync(request);

        var amenity = new Amenity
        {
            Name = request.Name,
            Price = request.Price
        };
        
        await amenityRepository.AddAsync(amenity);
        return amenity.Id;
    }

    public async Task<AmenityResponse> GetByIdAsync(Guid id)
    {
        var amenity = await amenityRepository.GetByIdAsync(id);

        if (amenity is null)
        {
            throw new NotFoundException($"Amenity with id '{id}' was not found.");
        }

        return new AmenityResponse
        {
            Id = amenity.Id,
            Name = amenity.Name,
            Price = amenity.Price
        };
    }
}