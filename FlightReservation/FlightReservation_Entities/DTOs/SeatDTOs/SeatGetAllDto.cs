using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Entities.DTOs.FlightDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.SeatDTOs
{
    public class SeatGetAllDto: IDto
    {
        public Guid Id { get; set; }
        public string SeatNumber { get; set; }
        public string Class { get; set; }
        public bool IsBooked { get; set; }
        public Guid FlightId { get; set; }
        public Guid? TicketId { get; set; }
    }
}
