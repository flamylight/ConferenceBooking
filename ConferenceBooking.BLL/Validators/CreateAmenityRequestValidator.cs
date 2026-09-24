using ConferenceBooking.BLL.DTOs.Amenity;
using FluentValidation;

namespace ConferenceBooking.BLL.Validators;

public class CreateAmenityRequestValidator: AbstractValidator<CreateAmenityRequest>
{
    public CreateAmenityRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must be less than 100 characters");
        
        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required")
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}