namespace ConferenceBooking.BLL.DTOs.Room;

public class AvailableRoomsFilterRequest
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }  
    public int MinCapacity { get; set; }
}