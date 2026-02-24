using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Entities.DTOs.FlightDTOs;
using FlightReservation_Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.SeatDTOs
{
    public class SeatGetDto: IDto
    {
        public string SeatNumber { get; set; }
        public SeatClass Class { get; set; }
        public bool IsBooked { get; set; }
        public Guid FlightId { get; set; }
        public Guid? TicketId { get; set; }
    }
}
