namespace ConferenceBooking.BLL.DTOs.Report;

public class RoomRevenue
{
    public Guid RoomId { get; set; }
    public required string Name { get; set; }
    public decimal RentalRevenue { get; set; } 
    public decimal AmenityRevenue { get; set; }
    public decimal TotalRevenue { get; set; }
}