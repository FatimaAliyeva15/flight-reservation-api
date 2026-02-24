using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Core.Entities.Common;
using FlightReservation_Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.Concretes
{
    public class Seat: BaseEntity, IEntity
    {
        public string SeatNumber { get; set; } 
        public SeatClass Class { get; set; } 
        public bool IsBooked { get; set; } = false;

        public Guid FlightId { get; set; }
        public Flight Flight { get; set; }

        public Guid? TicketId { get; set; } 
        public Ticket Ticket { get; set; }
    }
}
