using FlightReservation_Entities.DTOs.AirlineDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Validators.AirlineValidators
{
    public class AirlineUpdateDtoValidator: AbstractValidator<AirlineUpdateDto>
    {
        public AirlineUpdateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").MaximumLength(100).WithMessage("Name  size can be maximum 100");
            RuleFor(x => x.Code).NotEmpty().WithMessage("Code is required").MaximumLength(100).WithMessage("Code  size can be maximum 100");
        }
    }
}
