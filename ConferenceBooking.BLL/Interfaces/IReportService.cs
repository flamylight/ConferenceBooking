using ConferenceBooking.BLL.DTOs.Report;

namespace ConferenceBooking.BLL.Interfaces;

public interface IReportService
{
    Task<RevenueReportResponse> GetRevenueReportAsync(RevenueReportRequest request);
}