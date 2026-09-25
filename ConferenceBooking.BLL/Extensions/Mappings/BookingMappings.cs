using ConferenceBooking.BLL.DTOs.Amenity;
using ConferenceBooking.BLL.DTOs.Booking;
using ConferenceBooking.DAL.Models;

namespace ConferenceBooking.BLL.Extensions.Mappings;

public static class BookingMappings
{
    public static BookingResponse ToBookingResponse(this Booking booking)
    {
        return new BookingResponse
        {
            Id = booking.Id,
            RoomId = booking.RoomId,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            TotalPrice = booking.TotalPrice,
            Amenities = booking.Amenities.Select(a => new AmenityResponse
            {
                Id = a.Id,
                Name = a.Name,
                Price = a.Price
            }).ToList()
        };
    }
}