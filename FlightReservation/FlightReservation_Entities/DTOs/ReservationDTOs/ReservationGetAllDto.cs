using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.ReservationDTOs
{
    public class ReservationGetAllDto: IDto
    {
        public Guid Id { get; set; }
        public decimal TotalPrice { get; set; }
        public ReservationStatus Status { get; set; }
        public Guid FlightId { get; set; }
        public Guid UserId { get; set; }
        public List<Guid> TicketIds { get; set; }
        public List<Guid> PaymentIds { get; set; }
    }
}
