using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Core.Entities.Common;
using FlightReservation_Core.Entities.Concrete.Auth;
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
        public DateTime? PaidAt { get; set; }
        public DateTime? RefundedAt { get; set; }

        public Guid ReservationId { get; set; }
        public Reservation Reservation { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

    }
}
