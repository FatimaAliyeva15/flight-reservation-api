using FlightReservation_Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.AuthDTOs
{
    public class LoginDto: IDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
