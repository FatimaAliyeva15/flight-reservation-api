using FlightReservation_Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.Concretes
{
    public class Passenger: BaseEntity
    {
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }

        public string FullName { get; set; }

        public string PassportNumber { get; set; }

        public string ContactInfo { get; set; }
    }
}
