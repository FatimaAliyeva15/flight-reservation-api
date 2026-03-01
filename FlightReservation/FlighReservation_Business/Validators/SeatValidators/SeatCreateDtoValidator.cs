using FlightReservation_Entities.DTOs.SeatDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Validators.SeatValidators
{
    public class SeatCreateDtoValidator: AbstractValidator<SeatCreateDto>
    {
        public SeatCreateDtoValidator()
        {
            RuleFor(x => x.SeatNumber).NotEmpty().WithMessage("Seat number is required.").MaximumLength(10).WithMessage("Seat number must not exceed 10 characters.");
            RuleFor(x => x.Class).IsInEnum().WithMessage("Invalid seat class.");
            RuleFor(x => x.FlightId).NotEmpty().WithMessage("FlightId is required.");
        }
    }
}
