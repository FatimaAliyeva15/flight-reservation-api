using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Entities.DTOs.AirlineDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.AircraftDTOs
{
    public class AircraftGetAllDto: IDto
    {
        public Guid Id { get; set; }
        public string Model { get; set; }
        public int Capacity { get; set; }
        public string AirlineName { get; set; }
    }
}
