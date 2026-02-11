using FlightReservation_Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.AirportDTOs
{
    public class AirportCreateDto: IDto
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string IATACode { get; set; }
    }
}
