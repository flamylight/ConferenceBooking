using ConferenceBooking.BLL.DTOs.Amenity;
using ConferenceBooking.BLL.DTOs.Room;
using ConferenceBooking.DAL.Models;

namespace ConferenceBooking.BLL.Extensions.Mappings;

public static class RoomMappings
{
    public static RoomResponse ToRoomResponse(this Room room)
    {
        return new RoomResponse
        {
            Id = room.Id,
            Name = room.Name,
            Capacity = room.Capacity,
            HourlyPrice = room.HourlyPrice,
            Amenities = room.Amenities.Select(a => new AmenityResponse
            {
                Id = a.Id,
                Name = a.Name,
                Price = a.Price
            }).ToList()
        };
    }
}