namespace ConferenceBooking.BLL.DTOs.Report;

public class AmenityReportResponse
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalAmenitiesBooked { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<AmenityRevenue> Amenities { get; set; } = [];
}