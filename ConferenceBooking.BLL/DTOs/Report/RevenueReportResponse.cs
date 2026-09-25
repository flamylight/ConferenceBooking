namespace ConferenceBooking.BLL.DTOs.Report;

public class RevenueReportResponse
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalRentalRevenue { get; set; }
    public decimal TotalAmenityRevenue { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<RoomRevenue> Rooms { get; set; } = [];
}