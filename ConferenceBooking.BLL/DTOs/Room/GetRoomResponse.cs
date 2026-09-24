namespace ConferenceBooking.BLL.DTOs.Room;

public class GetRoomResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public int Capacity { get; set; }
    public decimal HourlyPrice { get; set; }
}