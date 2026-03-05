
using FlightReservation_Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.AuthDTOs
{
    public class AuthResponseDto: IDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
