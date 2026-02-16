using FlightReservation_Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.AircraftDTOs
{
    public class AircraftCreateDto: IDto
    {
        public string Model { get; set; }
        public int Capacity { get; set; }
        public Guid AirlineId { get; set; }
    }
}
