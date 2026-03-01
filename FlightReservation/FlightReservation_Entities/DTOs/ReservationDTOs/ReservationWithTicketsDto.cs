using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Entities.DTOs.TicketDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.ReservationDTOs
{
    public class ReservationWithTicketsDto: IDto
    {
        public ReservationCreateDto Reservation { get; set; }
        public List<TicketCreateDto> Tickets { get; set; }

    }
}
