using FlightReservation_Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.TicketDTOs
{
    public class TicketCreateDto: IDto
    {
        public decimal Price { get; set; }

        public Guid FlightId { get; set; }
        public Guid PassengerId { get; set; }
        public Guid SeatId { get; set; }
        public Guid ReservationId { get; set; }
    }
}
