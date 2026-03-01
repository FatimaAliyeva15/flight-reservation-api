using FlightReservation_Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.TicketDTOs
{
    public class TicketUpdateDto: IDto
    {
        public Guid SeatId { get; set; }
    }
}
