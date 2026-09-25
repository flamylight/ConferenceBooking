using ConferenceBooking.BLL.DTOs.Amenity;

namespace ConferenceBooking.BLL.DTOs.Booking;

public class BookingResponse
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal TotalPrice { get; set; }
    public List<AmenityResponse> Amenities { get; set; } = [];
}