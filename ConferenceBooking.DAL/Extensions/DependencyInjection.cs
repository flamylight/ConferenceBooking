using ConferenceBooking.DAL.Data;
using ConferenceBooking.DAL.Interfaces;
using ConferenceBooking.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConferenceBooking.DAL.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddDal(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IAmenityRepository, AmenityRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        
        return services;
    }
}