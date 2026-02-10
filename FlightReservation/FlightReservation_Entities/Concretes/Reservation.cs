using FlightReservation_Entities.Common;
using FlightReservation_Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.Concretes
{
    public class Reservation: BaseEntity
    {
        public int FlightId { get; set; }
        public Flight Flight { get; set; }

        public string CustomerId { get; set; }
       // public AppUser Customer { get; set; }

        public int SeatCount { get; set; }

        public decimal TotalPrice { get; set; }

        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

        public ICollection<Passenger> Passengers { get; set; }

        public Payment Payment { get; set; }
    }
}
