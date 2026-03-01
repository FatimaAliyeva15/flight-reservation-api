using FlightReservation_Entities.DTOs.ReservationDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Validators.ReservationValidators
{
    public class ReservationCreateDtoValidator : AbstractValidator<ReservationCreateDto>
    {
        public ReservationCreateDtoValidator()
        {
            RuleFor(x => x.FlightId).NotEmpty().WithMessage("FlightId is required.");
            RuleFor(x => x.SeatIds).NotEmpty().WithMessage("At least one seat must be selected.").Must(list => list.All(id => id != Guid.Empty)).WithMessage("All SeatIds must be valid GUIDs.");
        }
    }
}
