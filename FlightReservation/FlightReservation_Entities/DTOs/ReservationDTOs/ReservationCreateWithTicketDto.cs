using FlightReservation_Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.ReservationDTOs
{
    public class ReservationCreateWithTicketDto: IDto
    {
        public Guid PassengerId { get; set; }
    }
}
