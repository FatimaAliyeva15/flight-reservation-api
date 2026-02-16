using FlightReservation_Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.PaymentDTOs
{
    public class PaymentCreateDto: IDto
    {
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public Guid ReservationId { get; set; }
    }
}
