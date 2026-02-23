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
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

        public Guid FlightId { get; set; }
        public Flight Flight { get; set; }

        public Guid UserId { get; set; }
        public AppUser User { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

        public ICollection<Payment> Payments { get; set; }

    }
}
