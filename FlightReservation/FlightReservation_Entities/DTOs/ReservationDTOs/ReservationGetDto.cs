using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Entities.DTOs.ReservationPassengerDTOs;
using FlightReservation_Entities.Enums;


namespace FlightReservation_Entities.DTOs.ReservationDTOs
{
    public class ReservationGetDto: IDto
    {
        public decimal TotalPrice { get; set; }
        public ReservationStatus Status { get; set; }

        public Guid FlightId { get; set; }
        public Guid UserId { get; set; }

        public List<Guid> TicketIds { get; set; }
        public List<Guid> PaymentIds { get; set; }
    }
}
