using FlightReservation_Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.FlightDTOs
{
    public class FlightCreateDto: IDto
    {
        public string FlightNumber { get; set; }
        public decimal Price { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public Guid AirlineId { get; set; }
        public Guid AircraftId { get; set; }
        public Guid DepartureAirportId { get; set; }
        public Guid ArrivalAirportId { get; set; }


    }
}
