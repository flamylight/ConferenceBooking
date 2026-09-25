using ConferenceBooking.BLL.DTOs.Booking;
using ConferenceBooking.BLL.Exceptions;
using ConferenceBooking.BLL.Services;
using ConferenceBooking.DAL.Interfaces;
using ConferenceBooking.DAL.Models;
using FluentValidation;
using Moq;
using Xunit;

namespace ConferenceBooking.BLL.Tests.Services;

public class BookingServiceTest
{
    private readonly Mock<IValidator<CreateBookingRequest>> _createValidatorMock;
    private readonly Mock<IBookingRepository> _bookingRepositoryMock;
    private readonly Mock<IAmenityRepository> _amenityRepositoryMock;
    private readonly Mock<IRoomRepository> _roomRepositoryMock;
    private readonly BookingService _bookingService;

    public BookingServiceTest()
    {
        _createValidatorMock = new Mock<IValidator<CreateBookingRequest>>();
        _bookingRepositoryMock = new Mock<IBookingRepository>();
        _amenityRepositoryMock = new Mock<IAmenityRepository>();
        _roomRepositoryMock = new Mock<IRoomRepository>();
        _bookingService = new BookingService(
            _createValidatorMock.Object,
            _bookingRepositoryMock.Object,
            _amenityRepositoryMock.Object,
            _roomRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ValidRequestWithoutAmenities_ReturnsResponse()
    {
        var roomId = Guid.NewGuid();
        var startTime = DateTime.UtcNow.Date.AddHours(12);
        var endTime = DateTime.UtcNow.Date.AddHours(14);

        var request = new CreateBookingRequest
        {
            RoomId = roomId,
            StartTime = startTime,
            EndTime = endTime,
            AmenityIds = []
        };

        var room = new Room
        {
            Id = roomId,
            Name = "Conference Hall A",
            HourlyPrice = 100m,
            Amenities = []
        };
        
        _createValidatorMock
            .Setup(v => v.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        
        _roomRepositoryMock
            .Setup(r => r.GetByIdAsync(roomId))
            .ReturnsAsync(room);
        
        _bookingRepositoryMock
            .Setup(b => b.HasOverlappingAsync(roomId, startTime, endTime))
            .ReturnsAsync(false);
        
        _bookingRepositoryMock
            .Setup(b => b.AddAsync(It.IsAny<Booking>()))
            .Returns(Task.CompletedTask);
        
        var result = await _bookingService.CreateAsync(request);
        
        Assert.NotNull(result);
        Assert.Equal(roomId, result.RoomId);
        Assert.Equal(startTime, result.StartTime);
        Assert.Equal(endTime, result.EndTime);
        Assert.Equal(230m, result.TotalPrice);
        Assert.Empty(result.Amenities);
        
        _bookingRepositoryMock.Verify(b => b.AddAsync(It.IsAny<Booking>()), Times.Once);
        _amenityRepositoryMock.Verify(a => a.GetByIdsAsync(It.IsAny<IEnumerable<Guid>>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ValidRequestWithAmenities_ReturnsResponse()
    {
        var roomId = Guid.NewGuid();
        var amenityId = Guid.NewGuid();
        var startTime = DateTime.UtcNow.Date.AddHours(6);
        var endTime = DateTime.UtcNow.Date.AddHours(8);
        
        var request = new CreateBookingRequest
        {
            RoomId = roomId,
            StartTime = startTime,
            EndTime = endTime,
            AmenityIds = [amenityId]
        };
        
        var amenity = new Amenity { Id = amenityId, Name = "Projector", Price = 150m };
        
        var room = new Room
        {
            Id = roomId,
            Name = "Conference Hall B",
            HourlyPrice = 100m,
            Amenities = [amenity]
        };
        
        _createValidatorMock
            .Setup(v => v.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        
        _roomRepositoryMock
            .Setup(r => r.GetByIdAsync(roomId))
            .ReturnsAsync(room);
        
        _bookingRepositoryMock
            .Setup(b => b.HasOverlappingAsync(roomId, startTime, endTime))
            .ReturnsAsync(false);
        
        _amenityRepositoryMock
            .Setup(a => a.GetByIdsAsync(request.AmenityIds))
            .ReturnsAsync([amenity]);
        
        _bookingRepositoryMock
            .Setup(b => b.AddAsync(It.IsAny<Booking>()))
            .Returns(Task.CompletedTask);
        
        var result = await _bookingService.CreateAsync(request);
        
        Assert.NotNull(result);
        Assert.Equal(roomId, result.RoomId);
        Assert.Equal(startTime, result.StartTime);
        Assert.Equal(endTime, result.EndTime);
        Assert.Equal(330m, result.TotalPrice);
        Assert.Equal(amenityId, result.Amenities.First().Id);
        
        _bookingRepositoryMock.Verify(b => b.AddAsync(It.IsAny<Booking>()), Times.Once);
        _amenityRepositoryMock.Verify(a => a.GetByIdsAsync(request.AmenityIds), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WhenRoomNotFound_ThrowsNotFoundException()
    {
        var roomId = Guid.NewGuid();
        var startTime = DateTime.UtcNow.Date.AddHours(6);
        var endTime = DateTime.UtcNow.Date.AddHours(8);
        
        var request = new CreateBookingRequest
        {
            RoomId = roomId,
            StartTime = startTime,
            EndTime = endTime,
            AmenityIds = []
        };
        
        _createValidatorMock
            .Setup(v => v.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        
        _roomRepositoryMock
            .Setup(r => r.GetByIdAsync(roomId))
            .ReturnsAsync(null as Room);
        
        await Assert.ThrowsAsync<NotFoundException>(() => _bookingService.CreateAsync(request));
        
        _bookingRepositoryMock.Verify(b => b.AddAsync(It.IsAny<Booking>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenOverlappingBooking_ThrowsConflictException()
    {
        var roomId = Guid.NewGuid();
        var startTime = DateTime.UtcNow.Date.AddHours(6);
        var endTime = DateTime.UtcNow.Date.AddHours(8);
        
        var request = new CreateBookingRequest
        {
            RoomId = roomId,
            StartTime = startTime,
            EndTime = endTime,
            AmenityIds = []
        };
        
        var room = new Room
        {
            Id = roomId,
            Name = "Conference Hall C",
            HourlyPrice = 100m,
            Amenities = []
        };
        
        _createValidatorMock
            .Setup(v => v.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        
        _roomRepositoryMock
            .Setup(r => r.GetByIdAsync(roomId))
            .ReturnsAsync(room);
        
        _bookingRepositoryMock
            .Setup(b => b.HasOverlappingAsync(roomId, startTime, endTime))
            .ReturnsAsync(true);
        
        await Assert.ThrowsAsync<ConflictException>(() => _bookingService.CreateAsync(request));

        _bookingRepositoryMock.Verify(b => b.AddAsync(It.IsAny<Booking>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenAmenityNotAvailableForRoom_ThrowsBarRequestException()
    {
        var roomId = Guid.NewGuid();
        var amenityId = Guid.NewGuid();
        var startTime = DateTime.UtcNow.Date.AddHours(6);
        var endTime = DateTime.UtcNow.Date.AddHours(8);
        
        var request = new CreateBookingRequest
        {
            RoomId = roomId,
            StartTime = startTime,
            EndTime = endTime,
            AmenityIds = [amenityId]
        };
        
        var amenity = new Amenity { Id = amenityId, Name = "Projector", Price = 150m };
        
        var room = new Room
        {
            Id = roomId,
            Name = "Conference Hall B",
            HourlyPrice = 100m,
            Amenities = []
        };
        
        _createValidatorMock
            .Setup(v => v.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        
        _roomRepositoryMock
            .Setup(r => r.GetByIdAsync(roomId))
            .ReturnsAsync(room);
        
        _bookingRepositoryMock
            .Setup(b => b.HasOverlappingAsync(roomId, startTime, endTime))
            .ReturnsAsync(false);
        
        _amenityRepositoryMock
            .Setup(a => a.GetByIdsAsync(request.AmenityIds))
            .ReturnsAsync([amenity]);
        
        await Assert.ThrowsAsync<BadRequestException>(() => _bookingService.CreateAsync(request));
        
        _bookingRepositoryMock.Verify(b => b.AddAsync(It.IsAny<Booking>()), Times.Never);
    }
    
    [Fact]
    public async Task CreateAsync_WhenAmenityDoesNotExist_ThrowsBadRequestException()
    {
        var roomId = Guid.NewGuid();
        var amenityId = Guid.NewGuid();
        var startTime = DateTime.UtcNow.Date.AddHours(6);
        var endTime = DateTime.UtcNow.Date.AddHours(8);
        
        var request = new CreateBookingRequest
        {
            RoomId = roomId,
            StartTime = startTime,
            EndTime = endTime,
            AmenityIds = [amenityId]
        };
        
        var room = new Room
        {
            Id = roomId,
            Name = "Conference Hall B",
            HourlyPrice = 100m,
            Amenities = []
        };
        
        _createValidatorMock
            .Setup(v => v.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        
        _roomRepositoryMock
            .Setup(r => r.GetByIdAsync(roomId))
            .ReturnsAsync(room);
        
        _bookingRepositoryMock
            .Setup(b => b.HasOverlappingAsync(roomId, startTime, endTime))
            .ReturnsAsync(false);
        
        _amenityRepositoryMock
            .Setup(a => a.GetByIdsAsync(request.AmenityIds))
            .ReturnsAsync([]);
        
        await Assert.ThrowsAsync<BadRequestException>(() => _bookingService.CreateAsync(request));
        
        _bookingRepositoryMock.Verify(b => b.AddAsync(It.IsAny<Booking>()), Times.Never);
    }
}