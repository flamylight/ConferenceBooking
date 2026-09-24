using ConferenceBooking.BLL.Interfaces;
using ConferenceBooking.BLL.Services;
using ConferenceBooking.BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ConferenceBooking.BLL.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddBll(this IServiceCollection services)
    {
        services.AddScoped<IRoomService, RoomService>();

        services.AddValidatorsFromAssemblyContaining<CreateRoomRequestValidator>();
        
        return services;
    }
}