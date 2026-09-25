using ConferenceBooking.BLL.DTOs.Room;

namespace ConferenceBooking.BLL.Interfaces;

public interface IRoomService
{
    Task<Guid> CreateAsync(CreateRoomRequest request);
    Task<RoomResponse> GetByIdAsync(Guid id);
    Task UpdateAsync(Guid id, UpdateRoomRequest request);
    Task DeleteAsync(Guid id);
    Task<List<RoomResponse>> GetAvailableRoomsAsync(AvailableRoomsFilterRequest request);
}