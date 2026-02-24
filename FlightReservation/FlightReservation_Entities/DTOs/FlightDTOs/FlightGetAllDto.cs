using FlightReservation_Entities.DTOs.AircraftDTOs;
using FlightReservation_Entities.DTOs.AirlineDTOs;
using FlightReservation_Entities.DTOs.AirportDTOs;
using FlightReservation_Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.FlightDTOs
{
    public class FlightGetAllDto
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public string FlightNumber { get; set; }
        public string AdminComment { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public FlightStatus Status { get; set; }


        public Guid AirlineId { get; set; }
        public string AirlineName { get; set; }

        public Guid AircraftId { get; set; }
        public string AircraftModel { get; set; }

        public Guid DepartureAirportId { get; set; }
        public string DepartureAirportName { get; set; }

        public Guid ArrivalAirportId { get; set; }
        public string ArrivalAirportName { get; set; }

        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
    }
}
