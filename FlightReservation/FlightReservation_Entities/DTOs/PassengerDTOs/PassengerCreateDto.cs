using FlightReservation_Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.PessengerDTOs
{
    public class PassengerCreateDto: IDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PassportNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
