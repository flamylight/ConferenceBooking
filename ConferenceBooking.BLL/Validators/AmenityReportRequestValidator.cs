using ConferenceBooking.BLL.DTOs.Report;
using FluentValidation;

namespace ConferenceBooking.BLL.Validators;

public class AmenityReportRequestValidator: AbstractValidator<AmenityReportRequest>
{
    public AmenityReportRequestValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("End date must be greater than or equal to Start date.");
    }
}