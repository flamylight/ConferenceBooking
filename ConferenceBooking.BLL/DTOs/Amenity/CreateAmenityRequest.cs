namespace ConferenceBooking.BLL.DTOs.Amenity;

public class CreateAmenityRequest
{
    public required string Name { get; set; }
    public decimal Price { get; set; }
}