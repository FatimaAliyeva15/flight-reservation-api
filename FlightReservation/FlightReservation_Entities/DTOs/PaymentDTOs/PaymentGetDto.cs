using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Core.Entities.Concrete.Auth;
using FlightReservation_Entities.Concretes;
using FlightReservation_Entities.DTOs.ReservationDTOs;
using FlightReservation_Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.PaymentDTOs
{
    public class PaymentGetDto: IDto
    {
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? RefundedAt { get; set; }

        public Guid ReservationId { get; set; }
        public string AppUserId { get; set; }
    }
}
