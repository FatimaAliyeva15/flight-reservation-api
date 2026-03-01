using FlightReservation_Entities.DTOs.NotificationDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Validators.NotificationValidators
{
    public class NotificationUpdateDtoValidator:AbstractValidator<NotificationUpdateDto>
    {
        public NotificationUpdateDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required").MaximumLength(100).WithMessage("Title size can be maximum 100");
            RuleFor(x => x.Message).NotEmpty().WithMessage("Message is required").MaximumLength(100).WithMessage("Message size can be maximum 100");
            RuleFor(x => x.IsRead).Must(x => x == true || x == false).When(x => x.IsRead.HasValue).WithMessage("IsRead must be true or false if provided.");
        }
    }
}
