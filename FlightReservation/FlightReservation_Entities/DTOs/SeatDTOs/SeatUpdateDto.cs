using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.SeatDTOs
{
    public class SeatUpdateDto: IDto
    {
        public string SeatNumber { get; set; }
        public SeatClass? Class { get; set; }
        public bool IsBooked { get; set; }
    }
}
