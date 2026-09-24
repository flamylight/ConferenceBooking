namespace ConferenceBooking.DAL.Models;

public class Room
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public int Capacity { get; set; }
    public decimal HourlyPrice { get; set; }
    public ICollection<Amenity> Amenities { get; set; } = [];
}