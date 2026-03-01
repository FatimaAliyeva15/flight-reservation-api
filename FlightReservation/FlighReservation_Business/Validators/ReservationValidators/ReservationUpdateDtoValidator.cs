using FlightReservation_Entities.DTOs.ReservationDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Validators.ReservationValidators
{
    public class ReservationUpdateDtoValidator: AbstractValidator<ReservationUpdateDto>
    {
        public ReservationUpdateDtoValidator()
        {
            RuleFor(x => x.Status).IsInEnum().When(x => x.Status.HasValue).WithMessage("Invalid reservation status.");
            RuleFor(x => x.IsPaid).Must(x => x == true || x == false).When(x => x.IsPaid.HasValue).WithMessage("IsPaid must be true or false if provided.");
            RuleFor(x => x.ExpiresAt).GreaterThan(DateTime.UtcNow).WithMessage("ExpiresAt must be a future date.");
        }
    }
}
