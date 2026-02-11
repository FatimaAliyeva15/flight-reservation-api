using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.FlightDTOs
{
    public class FlightGetDto
    {
        public string Airline { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public decimal Price { get; set; }

        public int TotalSeats { get; set; }
    }
}
