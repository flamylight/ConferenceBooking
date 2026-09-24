using ConferenceBooking.BLL.DTOs.Amenity;
using ConferenceBooking.BLL.DTOs.Room;
using ConferenceBooking.BLL.Exceptions;
using ConferenceBooking.BLL.Interfaces;
using ConferenceBooking.DAL.Interfaces;
using ConferenceBooking.DAL.Models;
using FluentValidation;

namespace ConferenceBooking.BLL.Services;

public class RoomService(
    IRoomRepository roomRepository,
    IAmenityRepository amenityRepository,
    IValidator<CreateRoomRequest> validator): IRoomService
{
    public async Task<Guid> CreateAsync(CreateRoomRequest request)
    {
        await validator.ValidateAndThrowAsync(request);

        List<Amenity> amenities = [];

        if (request.AmenityIds.Count > 0)
        {
            var distinctAmenityIds = request.AmenityIds.Distinct().ToList();
            
            amenities = await amenityRepository.GetByIdsAsync(distinctAmenityIds);

            if (amenities.Count != distinctAmenityIds.Count)
            {
                var notFoundAmenityIds = distinctAmenityIds
                    .Except(amenities.Select(a => a.Id));
                throw new NotFoundException(
                    $"Amenities with Ids [{string.Join(", ", notFoundAmenityIds)}] were not found.");
            }
        }
        
        var room = new Room
        {
            Name = request.Name,
            Capacity = request.Capacity,
            HourlyPrice = request.HourlyPrice,
            Amenities = amenities
        };

        await roomRepository.AddAsync(room);
        return room.Id;
    }

    public async Task<GetRoomResponse> GetByIdAsync(Guid id)
    {
        var room = await roomRepository.GetByIdAsync(id);

        if (room is null)
        {
            throw new NotFoundException($"Room with id '{id}' was not found.");
        }

        return new GetRoomResponse
        {
            Id = room.Id,
            Name = room.Name,
            Capacity = room.Capacity,
            HourlyPrice = room.HourlyPrice,
            Amenities = room.Amenities.Select(a => new GetAmenityResponse
            {
                Id = a.Id,
                Name = a.Name,
                Price = a.Price
            }).ToList()
        };
    }
}