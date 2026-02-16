using FlightReservation_Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.FlightDTOs
{
    public class FlightUpdateDto
    {
        public decimal? Price { get; set; }
        public string AdminComment { get; set; }
        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public FlightStatus? Status { get; set; }
    }
}
