namespace ConferenceBooking.BLL.DTOs.Amenity;

public class AmenityResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
}