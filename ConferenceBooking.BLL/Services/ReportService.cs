using ConferenceBooking.BLL.DTOs.Report;
using ConferenceBooking.BLL.Interfaces;
using ConferenceBooking.DAL.Interfaces;
using FluentValidation;

namespace ConferenceBooking.BLL.Services;

public class ReportService(
    IBookingRepository bookingRepository,
    IRoomRepository roomRepository,
    IValidator<RevenueReportRequest> revenueReportRequestValidator): IReportService
{
    public async Task<RevenueReportResponse> GetRevenueReportAsync(RevenueReportRequest request)
    {
        await revenueReportRequestValidator.ValidateAndThrowAsync(request);
        
        var bookings = await bookingRepository
            .GetBookingsForPeriodAsync(request.StartDate, request.EndDate);

        var allRooms = await roomRepository.GetAllAsync();
        
        var bookingsByRoom = bookings
            .GroupBy(b => b.RoomId)
            .ToDictionary(g => g.Key, g => g.ToList());

        List<RoomRevenue> roomRevenues = [];

        foreach (var room in allRooms)
        {
            var roomBookings = bookingsByRoom.GetValueOrDefault(room.Id, []);
            
            var amenityRevenue = roomBookings.Sum(b => b.Amenities.Sum(a => a.Price));
            
            var totalRevenue = roomBookings.Sum(b => b.TotalPrice);

            var rentalRevenue = totalRevenue - amenityRevenue;
            
            roomRevenues.Add(new RoomRevenue
            {
                RoomId = room.Id,
                Name = room.Name,
                RentalRevenue = rentalRevenue,
                AmenityRevenue = amenityRevenue,
                TotalRevenue = totalRevenue
            });
        }

        return new RevenueReportResponse
        {
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            TotalRentalRevenue = roomRevenues.Sum(r => r.RentalRevenue),
            TotalAmenityRevenue = roomRevenues.Sum(r => r.AmenityRevenue),
            TotalRevenue = roomRevenues.Sum(r => r.TotalRevenue),
            Rooms = roomRevenues
        };
    }
}