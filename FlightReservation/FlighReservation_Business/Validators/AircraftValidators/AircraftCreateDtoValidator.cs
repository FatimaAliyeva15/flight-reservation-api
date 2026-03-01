using FlightReservation_Entities.DTOs.AircraftDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Validators.AircraftValidators
{
    public class AircraftCreateDtoValidator: AbstractValidator<AircraftCreateDto>
    {
        public AircraftCreateDtoValidator()
        {
            RuleFor(x => x.Model).NotEmpty().WithMessage("Model is required").MaximumLength(100).WithMessage("Model  size can be maximum 100");
            RuleFor(x => x.Capacity).GreaterThan(0).WithMessage("Capacity must be greater than 0.").LessThanOrEqualTo(1000).WithMessage("Capacity is very large.");
            RuleFor(x => x.AirlineId).NotEmpty().WithMessage("AirlineId can not be empty");
        }
    }
}
