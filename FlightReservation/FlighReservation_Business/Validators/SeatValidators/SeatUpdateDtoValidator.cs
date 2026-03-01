using FlightReservation_Entities.DTOs.SeatDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Validators.SeatValidators
{
    public class SeatUpdateDtoValidator: AbstractValidator<SeatUpdateDto>
    {
        public SeatUpdateDtoValidator()
        {
            RuleFor(x => x.SeatNumber).NotEmpty().WithMessage("Seat number is required.").MaximumLength(10).WithMessage("Seat number must not exceed 10 characters.");
            RuleFor(x => x.Class).IsInEnum().When(x => x.Class.HasValue).WithMessage("Invalid seat class.");
            RuleFor(x => x.IsBooked).Must(x => x == true || x == false).When(x => x.IsBooked.HasValue).WithMessage("IsBooked must be true or false if provided.");
            RuleFor(x => x.FlightId).NotEmpty().WithMessage("FlightId is required.");
            RuleFor(x => x.TicketId).NotEmpty().WithMessage("TicketId is required.");
        }
    }
}
