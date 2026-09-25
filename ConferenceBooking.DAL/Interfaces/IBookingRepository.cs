using ConferenceBooking.DAL.Models;

namespace ConferenceBooking.DAL.Interfaces;

public interface IBookingRepository
{
    Task AddAsync(Booking booking);
    Task<bool> HasOverlappingAsync(Guid roomId, DateTime start, DateTime end);
    Task<Booking?> GetByIdAsync(Guid id);
    Task<bool> HasFutureBookingsAsync(Guid roomId, DateTime start);
}