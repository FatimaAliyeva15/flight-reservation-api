using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.Concretes
{
    public class Ticket: BaseEntity, IEntity
    {
        public decimal Price { get; set; }

        public Guid FlightId { get; set; }
        public Flight Flight { get; set; }

        public Guid PassengerId { get; set; }
        public Passenger Passenger { get; set; }

        public Guid ReservationId { get; set; }
        public Reservation Reservation { get; set; }

        public Guid SeatId { get; set; } 
        public Seat Seat { get; set; }

    }
}
