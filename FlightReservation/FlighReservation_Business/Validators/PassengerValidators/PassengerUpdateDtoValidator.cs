using FlightReservation_Entities.DTOs.PassengerDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Validators.PassengerValidators
{
    public class PassengerUpdateDtoValidator: AbstractValidator<PassengerUpdateDto>
    {
        public PassengerUpdateDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required").MaximumLength(100).WithMessage("FirstName  size can be maximum 100");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is required").MaximumLength(100).WithMessage("LastName  size can be maximum 100");
            RuleFor(x => x.PassportNumber).NotEmpty().WithMessage("PassportNumber is required").MaximumLength(100).WithMessage("PassportNumber  size can be maximum 100");
            RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Date of birth is required.").LessThan(DateTime.UtcNow).WithMessage("Date of birth must be in the past.").Must(dob => dob.AddYears(120) > DateTime.UtcNow).WithMessage("Date of birth is invalid.");
        }
    }
}
