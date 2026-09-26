using ConferenceBooking.BLL.DTOs.Report;
using ConferenceBooking.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceBooking.API.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class ReportController(IReportService reportService) : ControllerBase
    {
        [HttpGet("revenue")]
        public async Task<ActionResult<RevenueReportResponse>> GetRevenueReport(
            [FromQuery] RevenueReportRequest request)
        {
            return Ok(await reportService.GetRevenueReportAsync(request));
        }
        
        [HttpGet("amenities")]
        public async Task<ActionResult<AmenityReportResponse>> GetAmenityReport(
            [FromQuery] AmenityReportRequest request)
        {
            return Ok(await reportService.GetAmenityReportAsync(request));
        }
    }
}
