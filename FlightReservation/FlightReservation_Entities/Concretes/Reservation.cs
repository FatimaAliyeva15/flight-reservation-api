using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Core.Entities.Concrete.Auth;
using FlightReservation_Entities.Common;
using FlightReservation_Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.Concretes
{
    public class Reservation: BaseEntity, IEntity
    {
        public decimal TotalPrice { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
        public DateTime ReservationDate { get; set; } = DateTime.UtcNow;

        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
        public Payment Payment { get; set; }

    }
}
