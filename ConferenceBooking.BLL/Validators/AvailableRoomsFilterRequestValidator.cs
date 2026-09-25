using ConferenceBooking.BLL.DTOs.Room;
using FluentValidation;

namespace ConferenceBooking.BLL.Validators;

public class AvailableRoomsFilterRequestValidator: AbstractValidator<AvailableRoomsFilterRequest>
{
    public AvailableRoomsFilterRequestValidator()
    {
        RuleFor(x => x.MinCapacity)
            .GreaterThan(0)
            .WithMessage("MinCapacity must be greater than 0");
        
        RuleFor(x => x.EndTime)
            .NotEmpty()
            .WithMessage("EndTime is required")
            .GreaterThan(x => x.StartTime)
            .WithMessage("EndTime must be after startTime");
        
        RuleFor(x => x.StartTime)
            .NotEmpty()
            .WithMessage("StartTime is required")
            .GreaterThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("StartTime cannot be in the past");
    }
}