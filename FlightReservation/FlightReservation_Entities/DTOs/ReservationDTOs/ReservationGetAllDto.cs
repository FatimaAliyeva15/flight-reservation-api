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
        public bool IsPaid { get; set; }
        public string AppUserName { get; set; }
        public string FlightNumber { get; set; }
        public int TicketCount { get; set; }
    }
}
