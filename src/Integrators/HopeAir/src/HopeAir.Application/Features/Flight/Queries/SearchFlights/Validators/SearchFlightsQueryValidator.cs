using FluentValidation;

namespace HopeAir.Application.Features.Flight.Queries.SearchFlights.Validators;

public class SearchFlightsQueryValidator : AbstractValidator<SearchFlightsQuery>
{
    public SearchFlightsQueryValidator()
    {
        RuleFor(x => x.Origin)
            .NotEmpty().WithMessage("Origin is required.")
            .Length(3).WithMessage("Origin must be exactly 3 characters.")
            .Matches(@"^[A-Z]{3}$").WithMessage("Origin must be uppercase alphabetic characters only.");

        RuleFor(x => x.Destination)
            .NotEmpty().WithMessage("Destination is required.")
            .Length(3).WithMessage("Destination must be exactly 3 characters.")
            .Matches(@"^[A-Z]{3}$").WithMessage("Destination must be uppercase alphabetic characters only.");

        RuleFor(x => x.DepartureDate)
            .NotEmpty().WithMessage("Departure date is required.")
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Departure date cannot be in the past.")
            .Must(BeAValidDate).WithMessage("Departure date must be a valid date.");

        RuleFor(x => x.ReturnDate)
            .GreaterThanOrEqualTo(x => x.DepartureDate).WithMessage("Return date must be after or equal to the departure date.")
            .Must(BeAValidDate).WithMessage("Return date must be a valid date.")
            .When(x => x.ReturnDate != default);

        RuleFor(x => x.PassengerCount)
            .GreaterThan(0).WithMessage("Passenger count must be greater than 0.")
            .LessThanOrEqualTo(9).WithMessage("Passenger count must not exceed 9.");
    }

    private static bool BeAValidDate(DateTime date)
    {
        return date != default;
    }
}