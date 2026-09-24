using ConferenceBooking.BLL.DTOs.Room;
using FluentValidation;

namespace ConferenceBooking.BLL.Validators;

public class UpdateRoomRequestValidator: AbstractValidator<UpdateRoomRequest>
{
    public UpdateRoomRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must be less than 100 characters");
        
        RuleFor(x => x.Capacity)
            .NotEmpty().WithMessage("Capacity is required")
            .GreaterThan(0).WithMessage("Capacity must be greater than 0");
        
        RuleFor(x => x.HourlyPrice)
            .NotEmpty().WithMessage("HourlyPrice is required")
            .GreaterThan(0).WithMessage("HourlyPrice must be greater than 0");
    }
}