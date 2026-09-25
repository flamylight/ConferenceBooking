using ConferenceBooking.BLL.DTOs.Report;
using FluentValidation;

namespace ConferenceBooking.BLL.Validators;

public class RevenueReportRequestValidator: AbstractValidator<RevenueReportRequest>
{
    public RevenueReportRequestValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("StartDate is required");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("EndDate is required")
            .GreaterThan(x => x.StartDate)
            .WithMessage("EndDate must be after StartDate");
    }
}