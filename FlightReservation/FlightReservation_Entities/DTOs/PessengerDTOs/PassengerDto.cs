using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.PessengerDTOs
{
    public class PassengerDto
    {
        public string FullName { get; set; }

        public string PassportNumber { get; set; }

        public string ContactInfo { get; set; }
    }
}
