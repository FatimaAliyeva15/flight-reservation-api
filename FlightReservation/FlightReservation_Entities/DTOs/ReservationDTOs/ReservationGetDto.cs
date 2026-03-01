using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Entities.DTOs.ReservationPassengerDTOs;
using FlightReservation_Entities.DTOs.TicketDTOs;
using FlightReservation_Entities.Enums;


namespace FlightReservation_Entities.DTOs.ReservationDTOs
{
    public class ReservationGetDto: IDto
    {
        public decimal TotalPrice { get; set; }
        public ReservationStatus Status { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        public DateTime? RefundedAt { get; set; }
        public DateTime ExpiresAt { get; set; }

        public Guid FlightId { get; set; }
        public string FlightNumber { get; set; }
        public string AppUserId { get; set; }
        public string AppUserName { get; set; }

        public List<TicketGetDto> Tickets { get; set; }
    }
}
