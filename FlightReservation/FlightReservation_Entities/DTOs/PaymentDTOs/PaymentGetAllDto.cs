using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Entities.DTOs.ReservationDTOs;
using FlightReservation_Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.PaymentDTOs
{
    public class PaymentGetAllDto: IDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public Guid ReservationId { get; set; }
    }
}
