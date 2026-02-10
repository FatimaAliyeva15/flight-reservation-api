using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.AuthDTOs
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
