using FlightReservation_Entities.DTOs.PaymentDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Validators.PaymentValidators
{
    public class PaymentCreateDtoValidator: AbstractValidator<PaymentCreateDto>
    {
        public PaymentCreateDtoValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Price must be greater than zero.");
            RuleFor(x => x.ReservationId).NotEmpty().WithMessage("ReservationId can not be empty");
        }
    }
}
