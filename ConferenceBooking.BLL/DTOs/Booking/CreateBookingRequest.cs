namespace ConferenceBooking.BLL.DTOs.Booking;

public class CreateBookingRequest
{
    public Guid RoomId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public List<Guid> AmenityIds { get; set; } = [];   
}