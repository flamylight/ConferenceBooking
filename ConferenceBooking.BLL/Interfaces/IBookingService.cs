using ConferenceBooking.BLL.DTOs.Booking;

namespace ConferenceBooking.BLL.Interfaces;

public interface IBookingService
{
    Task<BookingResponse> CreateAsync(CreateBookingRequest request);
    Task<BookingResponse> GetByIdAsync(Guid id);
}