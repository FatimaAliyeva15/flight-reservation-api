using FlightReservation_Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.PaymentDTOs
{
    public class PaymentDto: IDto
    {
        public Guid ReservationId { get; set; }

        public decimal Amount { get; set; }

        public string CardNumber { get; set; }

        public string ExpireDate { get; set; }

        public string CVV { get; set; }
    }
}
