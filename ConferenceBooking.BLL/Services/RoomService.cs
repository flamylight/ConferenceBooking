using ConferenceBooking.BLL.DTOs.Room;
using ConferenceBooking.BLL.Exceptions;
using ConferenceBooking.BLL.Interfaces;
using ConferenceBooking.DAL.Interfaces;
using ConferenceBooking.DAL.Models;
using FluentValidation;

namespace ConferenceBooking.BLL.Services;

public class RoomService(
    IRoomRepository repository,
    IValidator<CreateRoomRequest> validator): IRoomService
{
    public async Task<Guid> CreateAsync(CreateRoomRequest request)
    {
        await validator.ValidateAndThrowAsync(request);
        
        var room = new Room
        {
            Name = request.Name,
            Capacity = request.Capacity,
            HourlyPrice = request.HourlyPrice
        };

        await repository.AddAsync(room);
        return room.Id;
    }

    public async Task<GetRoomResponse> GetByIdAsync(Guid id)
    {
        var room = await repository.GetByIdAsync(id);

        if (room is null)
        {
            throw new NotFoundException($"Room with id '{id}' was not found.");
        }

        return new GetRoomResponse
        {
            Id = room.Id,
            Name = room.Name,
            Capacity = room.Capacity,
            HourlyPrice = room.HourlyPrice
        };
    }
}