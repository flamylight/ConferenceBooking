using ConferenceBooking.BLL.DTOs.Report;
using ConferenceBooking.BLL.Interfaces;
using ConferenceBooking.DAL.Interfaces;
using FluentValidation;

namespace ConferenceBooking.BLL.Services;

public class ReportService(
    IBookingRepository bookingRepository,
    IRoomRepository roomRepository,
    IAmenityRepository amenityRepository,
    IValidator<RevenueReportRequest> revenueReportRequestValidator,
    IValidator<AmenityReportRequest> amenityReportRequestValidator): IReportService
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

    public async Task<AmenityReportResponse> GetAmenityReportAsync(AmenityReportRequest request)
    {
        await amenityReportRequestValidator.ValidateAndThrowAsync(request);
        
        var bookings = await bookingRepository
            .GetBookingsForPeriodAsync(request.StartDate, request.EndDate);

        var allAmenities = await amenityRepository.GetAllAsync();
        
        var bookingAmenities = bookings
            .SelectMany(b => b.Amenities)
            .ToList();

        List<AmenityRevenue> amenityRevenues = [];

        foreach (var amenity in allAmenities)
        {
            var usageCount = bookingAmenities.Count(a => a.Id == amenity.Id);
            var totalRevenue = usageCount * amenity.Price;
            
            amenityRevenues.Add(new AmenityRevenue
            {
                AmenityId = amenity.Id,
                Name = amenity.Name,
                UsageCount = usageCount,
                TotalRevenue = totalRevenue
            });
        }

        return new AmenityReportResponse
        {
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            TotalRevenue = amenityRevenues.Sum(r => r.TotalRevenue),
            TotalAmenitiesBooked = amenityRevenues.Sum(r => r.UsageCount),
            Amenities = amenityRevenues
        };
    }
}