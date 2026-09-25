using ConferenceBooking.BLL.DTOs.Room;
using ConferenceBooking.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceBooking.API.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomController(IRoomService roomService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CreateRoomResponse>> Create([FromBody] CreateRoomRequest request)
        {
            var roomId = await roomService.CreateAsync(request);
            return CreatedAtAction(
                nameof(GetById), 
                new {id = roomId}, 
                new CreateRoomResponse {Id = roomId});
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RoomResponse>> GetById([FromRoute] Guid id)
        {
            var room = await roomService.GetByIdAsync(id);
            return Ok(room);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRoomRequest request)
        {
            await roomService.UpdateAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await roomService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<List<RoomResponse>>> GetAvailableRooms(
            [FromQuery] AvailableRoomsFilterRequest request)
        {
            var rooms = await roomService.GetAvailableRoomsAsync(request);
            return Ok(rooms);
        }
    }
}
