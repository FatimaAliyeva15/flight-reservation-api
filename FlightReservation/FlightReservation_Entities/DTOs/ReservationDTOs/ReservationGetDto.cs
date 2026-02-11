using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.ReservationDTOs
{
    public class ReservationGetDto: IDto
    {
        public Guid Id { get; set; }

        public Guid FlightId { get; set; }

        public int SeatCount { get; set; }

        public decimal TotalPrice { get; set; }

        public ReservationStatus Status { get; set; }
    }
}
