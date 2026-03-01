using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Entities.DTOs.FlightDTOs;
using FlightReservation_Entities.DTOs.PassengerDTOs;
using FlightReservation_Entities.DTOs.SeatDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.TicketDTOs
{
    public class TicketGetAllDto: IDto
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public string PassengerName { get; set; }
        public string SeatNumber { get; set; }
    }
}
