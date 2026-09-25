using ConferenceBooking.BLL.DTOs.Amenity;

namespace ConferenceBooking.BLL.DTOs.Room;

public class RoomResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public int Capacity { get; set; }
    public decimal HourlyPrice { get; set; }
    public List<AmenityResponse> Amenities { get; set; } = [];
}