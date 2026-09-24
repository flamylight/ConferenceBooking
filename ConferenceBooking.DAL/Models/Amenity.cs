namespace ConferenceBooking.DAL.Models;

public class Amenity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public decimal Price { get; set; }
}