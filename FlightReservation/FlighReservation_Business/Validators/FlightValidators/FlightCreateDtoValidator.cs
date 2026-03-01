using FlightReservation_Entities.DTOs.FlightDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Validators.FlightValidators
{
    public class FlightCreateDtoValidator:AbstractValidator<FlightCreateDto>
    {
        public FlightCreateDtoValidator()
        {
            RuleFor(x => x.FlightNumber).NotEmpty().WithMessage("Flight number is required.").MaximumLength(20).WithMessage("Flight number must not exceed 20 characters.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
            RuleFor(x => x.DepartureTime).NotEmpty().WithMessage("Departure time is required.").GreaterThan(DateTime.UtcNow).WithMessage("Departure time must be in the future.");
            RuleFor(x => x.ArrivalTime).NotEmpty().WithMessage("Arrival time is required.").GreaterThan(x => x.DepartureTime).WithMessage("Arrival time must be later than departure time.");
            RuleFor(x => x.AirlineId).NotEmpty().WithMessage("AirlineId is required.");
            RuleFor(x => x.AircraftId).NotEmpty().WithMessage("AircraftId is required.");
            RuleFor(x => x.DepartureAirportId).NotEmpty().WithMessage("DepartureAirportId is required.");
            RuleFor(x => x.ArrivalAirportId).NotEmpty().WithMessage("ArrivalAirportId is required.").NotEqual(x => x.DepartureAirportId).WithMessage("Arrival and departure airports must be different.");
        }
    }
}
