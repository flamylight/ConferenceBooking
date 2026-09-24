using ConferenceBooking.BLL.DTOs.Amenity;
using ConferenceBooking.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceBooking.API.Controllers
{
    [Route("api/amenities")]
    [ApiController]
    public class AmenityController(IAmenityService amenityService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CreateAmenityResponse>> Create([FromBody] CreateAmenityRequest request)
        {
            var amenityId = await amenityService.CreateAsync(request);
            return CreatedAtAction(
                nameof(GetById), 
                new {id = amenityId}, 
                new CreateAmenityResponse {Id = amenityId});
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetAmenityResponse>> GetById([FromRoute] Guid id)
        {
            var amenity = await amenityService.GetByIdAsync(id);
            return Ok(amenity);
        } 
    }
}
