namespace ConferenceBooking.BLL.DTOs.Room;

public class CreateRoomRequest
{
    public required string Name { get; set; }
    public int Capacity { get; set; }
    public decimal HourlyPrice { get; set; }
    public List<Guid> AmenityIds { get; set; } = [];
}