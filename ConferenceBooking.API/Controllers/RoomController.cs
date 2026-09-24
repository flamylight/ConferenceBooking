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
        public async Task<ActionResult<GetRoomResponse>> GetById([FromRoute] Guid id)
        {
            var room = await roomService.GetByIdAsync(id);
            return Ok(room);
        }
    }
}
