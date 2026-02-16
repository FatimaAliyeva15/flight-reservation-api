using FlightReservation_Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.ReservationPassengerDTOs
{
    public class ReservationPassengerGetDto: IDto
    {
        public string FullName { get; set; }
        public string PassportNumber { get; set; }
        public string SeatNumber { get; set; }
    }
}
