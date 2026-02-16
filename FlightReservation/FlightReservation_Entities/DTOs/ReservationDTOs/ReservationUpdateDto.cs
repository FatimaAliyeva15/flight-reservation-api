using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.ReservationDTOs
{
    public class ReservationUpdateDto: IDto
    {
        public ReservationStatus Status { get; set; }
    }
}
