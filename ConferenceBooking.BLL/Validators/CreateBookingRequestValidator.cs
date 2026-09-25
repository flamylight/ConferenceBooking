using ConferenceBooking.BLL.DTOs.Booking;
using FluentValidation;

namespace ConferenceBooking.BLL.Validators;

public class CreateBookingRequestValidator: AbstractValidator<CreateBookingRequest>
{
    public CreateBookingRequestValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("RoomId is required");
        
        RuleFor(x => x.StartTime)
            .NotEmpty()
            .WithMessage("StartTime is required")
            .GreaterThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("StartTime cannot be in the past.")
            .Must(x => x is { Minute: 0, Second: 0 })
            .WithMessage("StartTime must be at the start of an hour")
            .Must(x => x.Hour is >= 6 and < 23)
            .WithMessage("StartTime must be between 6am and 10pm");
        
        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("EndTime is required")
            .GreaterThan(x => x.StartTime)
            .WithMessage("EndTime must be after startTime")
            .Must(time => time is { Minute: 0, Second: 0 })
            .WithMessage("EndTime must be at the start of an hour")
            .Must(time => time.Hour is >= 7 and <= 23)
            .WithMessage("EndTime cannot exceed working hours (up to 23:00).");

        RuleFor(x => x)
            .Must(x => (x.EndTime - x.StartTime).TotalHours >= 1)
            .WithMessage("Minimum booking duration is 1 hour.");
    }
}