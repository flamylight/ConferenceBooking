namespace ConferenceBooking.BLL.DTOs.Report;

public class AmenityRevenue
{
    public Guid AmenityId { get; set; }
    public required string Name { get; set; }
    public int UsageCount { get; set; }
    public decimal TotalRevenue { get; set; }
}