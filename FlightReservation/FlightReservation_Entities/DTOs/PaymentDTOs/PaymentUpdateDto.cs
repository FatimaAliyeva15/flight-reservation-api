using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.PaymentDTOs
{
    public class PaymentUpdateDto: IDto
    {
        public decimal Amount { get; set; }
        public PaymentStatus? Status { get; set; }
    }
}
