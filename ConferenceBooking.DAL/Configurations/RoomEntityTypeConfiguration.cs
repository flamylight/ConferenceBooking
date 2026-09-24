using ConferenceBooking.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConferenceBooking.DAL.Configurations;

public class RoomEntityTypeConfiguration: IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(x => x.Capacity)
            .IsRequired();
        
        builder.Property(x => x.HourlyPrice)
            .IsRequired()
            .HasPrecision(10, 2);
    }
}