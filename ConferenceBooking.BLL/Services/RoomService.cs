using ConferenceBooking.BLL.DTOs.Room;
using ConferenceBooking.BLL.Exceptions;
using ConferenceBooking.BLL.Extensions.Mappings;
using ConferenceBooking.BLL.Interfaces;
using ConferenceBooking.DAL.Interfaces;
using ConferenceBooking.DAL.Models;
using FluentValidation;

namespace ConferenceBooking.BLL.Services;

public class RoomService(
    IRoomRepository roomRepository,
    IAmenityRepository amenityRepository,
    IBookingRepository bookingRepository,
    IValidator<CreateRoomRequest> createValidator,
    IValidator<AvailableRoomsFilterRequest> availableRoomsFilterValidator,
    IValidator<UpdateRoomRequest> updateValidator): IRoomService
{
    public async Task<Guid> CreateAsync(CreateRoomRequest request)
    {
        await createValidator.ValidateAndThrowAsync(request);

        List<Amenity> amenities = await GetValidAmenitiesAsync(request.AmenityIds);
        
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

    public async Task<RoomResponse> GetByIdAsync(Guid id)
    {
        var room = await roomRepository.GetByIdAsync(id);

        if (room is null)
        {
            throw new NotFoundException($"Room with id '{id}' was not found.");
        }

        return room.ToRoomResponse();
    }

    public async Task UpdateAsync(Guid id, UpdateRoomRequest request)
    {
        await updateValidator.ValidateAndThrowAsync(request);
        
        var room = await roomRepository.GetByIdAsync(id);

        if (room is null)
        {
            throw new NotFoundException($"Room with id '{id}' was not found.");
        }

        room.Name = request.Name;
        room.Capacity = request.Capacity;
        room.HourlyPrice = request.HourlyPrice;
        
        List<Amenity> newAmenities = await GetValidAmenitiesAsync(request.AmenityIds);
        
        room.Amenities = newAmenities;
        await roomRepository.UpdateAsync(room);
    }

    private async Task<List<Amenity>> GetValidAmenitiesAsync(List<Guid> amenityIds)
    {
        if (amenityIds.Count == 0)
        {
            return [];
        }
        
        var distinctAmenityIds = amenityIds.Distinct().ToList();
        var amenities = await amenityRepository.GetByIdsAsync(distinctAmenityIds);
        
        if (amenities.Count != distinctAmenityIds.Count)
        {
            var notFoundAmenityIds = distinctAmenityIds
                .Except(amenities.Select(a => a.Id));
            throw new NotFoundException(
                $"Amenities with Ids [{string.Join(", ", notFoundAmenityIds)}] were not found.");
        }
        
        return amenities;
    }

    public async Task DeleteAsync(Guid id)
    {
        var room = await roomRepository.GetByIdAsync(id);
        if (room is null)
        {
            throw new NotFoundException($"Room with id '{id}' was not found.");
        }
        
        var hasFutureBookings = await bookingRepository.HasFutureBookingsAsync(room.Id, DateTime.UtcNow);
        if (hasFutureBookings)
        {
            throw new ConflictException("Cannot delete room because it has active or future bookings.");
        }
        room.IsDeleted = true;
        await roomRepository.UpdateAsync(room);
    }

    public async Task<List<RoomResponse>> GetAvailableRoomsAsync(AvailableRoomsFilterRequest request)
    {
        await availableRoomsFilterValidator.ValidateAndThrowAsync(request);

        var rooms = await roomRepository
            .GetAvailableRoomsAsync(request.StartTime, request.EndTime, request.MinCapacity);

        return rooms.Select(r => r.ToRoomResponse()).ToList();
    }
}