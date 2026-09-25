using ConferenceBooking.BLL.DTOs.Booking;
using ConferenceBooking.BLL.Exceptions;
using ConferenceBooking.BLL.Extensions.Mappings;
using ConferenceBooking.BLL.Helpers;
using ConferenceBooking.BLL.Interfaces;
using ConferenceBooking.DAL.Interfaces;
using ConferenceBooking.DAL.Models;
using FluentValidation;

namespace ConferenceBooking.BLL.Services;

public class BookingService(
    IValidator<CreateBookingRequest> createValidator,
    IBookingRepository bookingRepository,
    IAmenityRepository amenityRepository,
    IRoomRepository roomRepository): IBookingService
{
    public async Task<BookingResponse> CreateAsync(CreateBookingRequest request)
    {
        await createValidator.ValidateAndThrowAsync(request);
        
        var room = await roomRepository.GetByIdAsync(request.RoomId);
        if (room is null)
        {
            throw new NotFoundException($"Room with id '{request.RoomId}' was not found.");
        }

        var isOverlapping = await bookingRepository.HasOverlappingAsync(
            request.RoomId, request.StartTime, request.EndTime);
        if (isOverlapping)
        {
            throw new ConflictException("The room is already booked for the specified time interval.");
        }

        List<Amenity> amenities = [];
        if (request.AmenityIds.Count > 0)
        {
            amenities = await amenityRepository.GetByIdsAsync(request.AmenityIds);
            var distinctAmenityIds = request.AmenityIds.Distinct().ToList();
            
            if (amenities.Count != distinctAmenityIds.Count)
            {
                var notFoundAmenityIds = distinctAmenityIds
                    .Except(amenities.Select(a => a.Id));
                throw new 
                    BadRequestException(
                        $"Amenities with Ids [{string.Join(", ", notFoundAmenityIds)}] were not found.");
            }
            
            var roomAmenityIds = room.Amenities.Select(a => a.Id);
            var invalidAmenityIds = request.AmenityIds
                .Where(id => !roomAmenityIds.Contains(id))
                .ToList();
            
            if (invalidAmenityIds.Count != 0)
            {
                throw new BadRequestException(
                    $"Amenities [{string.Join(",", invalidAmenityIds)}] are not available for the selected room.");
            }
        }

        var totalPrice = BookingPriceCalculator.CalculateTotalPrice(
            room.HourlyPrice,
            request.StartTime,
            request.EndTime,
            amenities.Select(a => a.Price));

        var booking = new Booking
        {
            RoomId = request.RoomId,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Amenities = amenities.ToList(),
            TotalPrice = totalPrice
        };
        await bookingRepository.AddAsync(booking);

        return booking.ToBookingResponse();
    }

    public async Task<BookingResponse> GetByIdAsync(Guid id)
    {
        var booking = await bookingRepository.GetByIdAsync(id);

        if (booking is null)
        {
            throw new NotFoundException($"Booking with id '{id}' was not found.");
        }

        return booking.ToBookingResponse();
    }
}