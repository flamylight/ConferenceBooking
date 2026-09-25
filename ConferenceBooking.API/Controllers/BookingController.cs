using ConferenceBooking.BLL.DTOs.Booking;
using ConferenceBooking.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceBooking.API.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    public class BookingController(IBookingService bookingService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<BookingResponse>> Create([FromBody] CreateBookingRequest request)
        {
            var booking = await bookingService.CreateAsync(request);
            return CreatedAtAction(
                nameof(GetById), 
                new {id = booking.Id}, 
                booking);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BookingResponse>> GetById([FromRoute] Guid id)
        {
            var booking = await bookingService.GetByIdAsync(id);
            return Ok(booking);
        }
    }
}
