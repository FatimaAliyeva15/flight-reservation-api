using FlightReservation_Entities.DTOs.PessengerDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.ReservationDTOs
{
    public class ReservationCreateDto
    {
        public int FlightId { get; set; }

        public int SeatCount { get; set; }

        public List<PassengerDto> Passengers { get; set; }
    }
}
