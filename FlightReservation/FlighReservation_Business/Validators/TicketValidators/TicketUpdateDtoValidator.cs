using FlightReservation_Entities.DTOs.TicketDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Validators.TicketValidators
{
    public class TicketUpdateDtoValidator: AbstractValidator<TicketUpdateDto>
    {
        public TicketUpdateDtoValidator()
        {
            RuleFor(x => x.SeatId).NotEmpty().WithMessage("SeatId is required.");
        }
    }
}
