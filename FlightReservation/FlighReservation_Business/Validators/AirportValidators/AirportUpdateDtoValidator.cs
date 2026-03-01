using FlightReservation_Entities.DTOs.AirportDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Validators.AirportValidators
{
    public class AirportUpdateDtoValidator: AbstractValidator<AirportUpdateDto>
    {
        public AirportUpdateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").MaximumLength(100).WithMessage("Name  size can be maximum 100");
            RuleFor(x => x.City).NotEmpty().WithMessage("City is required").MaximumLength(100).WithMessage("City  size can be maximum 100");
            RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required").MaximumLength(100).WithMessage("Country  size can be maximum 100");
        }
    }
}
