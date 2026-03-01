using FlightReservation_Entities.DTOs.PaymentDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Validators.PaymentValidators
{
    public class PaymentUpdateDtoValidator: AbstractValidator<PaymentUpdateDto>
    {
        public PaymentUpdateDtoValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Price must be greater than zero.");
            RuleFor(x => x.Status).IsInEnum().WithMessage("Invalid payment status.");
        }
    }
}
