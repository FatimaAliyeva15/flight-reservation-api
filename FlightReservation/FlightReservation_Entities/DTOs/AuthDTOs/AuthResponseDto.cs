using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.AuthDTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
