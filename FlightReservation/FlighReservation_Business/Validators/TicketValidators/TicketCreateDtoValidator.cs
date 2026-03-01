using FlightReservation_Entities.DTOs.TicketDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Validators.TicketValidators
{
    public class TicketCreateDtoValidator: AbstractValidator<TicketCreateDto>
    {
        public TicketCreateDtoValidator()
        {
            RuleFor(x => x.FlightId).NotEmpty().WithMessage("FlightId is required.");
            RuleFor(x => x.PassengerId).NotEmpty().WithMessage("PassengerId is required.");
            RuleFor(x => x.SeatId).NotEmpty().WithMessage("SeatId is required.");
            RuleFor(x => x.ReservationId).NotEmpty().WithMessage("ReservationId is required.");
        }
    }
}
