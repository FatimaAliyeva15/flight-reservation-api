using FlightReservation_Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.SeatDTOs
{
    public class SeatCreateDto: IDto
    {
        public string SeatNumber { get; set; }
        public string Class { get; set; }
        public Guid FlightId { get; set; }
    }
}
