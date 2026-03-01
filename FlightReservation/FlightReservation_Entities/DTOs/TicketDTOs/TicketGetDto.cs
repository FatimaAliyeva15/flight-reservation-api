using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Entities.DTOs.FlightDTOs;
using FlightReservation_Entities.DTOs.PassengerDTOs;
using FlightReservation_Entities.DTOs.SeatDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.TicketDTOs
{
    public class TicketGetDto: IDto
    {
        public decimal Price { get; set; }
        public Guid FlightId { get; set; }
        public string FlightNumber { get; set; }
        public Guid PassengerId { get; set; }
        public string PassengerName { get; set; }
        public Guid SeatId { get; set; }
        public string SeatNumber { get; set; }
        public Guid ReservationId { get; set; }
    }
}
