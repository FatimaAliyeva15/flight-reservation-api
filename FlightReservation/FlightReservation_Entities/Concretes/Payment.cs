using FlightReservation_Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.Concretes
{
    public class Payment
    {
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }

        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; }

        public string TransactionId { get; set; }
    }
}
