namespace ConferenceBooking.DAL.Models;

public class Room
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public int Capacity { get; set; }
    public bool IsDeleted { get; set; } = false;
    public decimal HourlyPrice { get; set; }
    public ICollection<Amenity> Amenities { get; set; } = [];
    public ICollection<Booking> Bookings { get; set; } = [];
}