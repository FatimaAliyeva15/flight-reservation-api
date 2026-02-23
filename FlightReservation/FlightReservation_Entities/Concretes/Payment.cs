using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Core.Entities.Common;
using FlightReservation_Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.Concretes
{
    public class Payment: BaseEntity, IEntity
    {
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public Guid ReservationId { get; set; }
        public Reservation Reservation { get; set; }

    }
}
