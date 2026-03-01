using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Core.Entities.Common;
using FlightReservation_Core.Entities.Concrete.Auth;
using FlightReservation_Entities.DTOs.ReservationPassengerDTOs;
using FlightReservation_Entities.Enums;
using System.Collections.ObjectModel;


namespace FlightReservation_Entities.Concretes
{
    public class Reservation: BaseEntity, IEntity
    {
        public decimal TotalPrice { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.PendingPayment;
        public bool IsPaid { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        public DateTime? RefundedAt { get; set; }
        public DateTime ExpiresAt { get; set; } 

        public Guid FlightId { get; set; }
        public Flight Flight { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

    }
}
