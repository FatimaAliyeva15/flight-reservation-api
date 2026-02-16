using FlightReservation_Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.AirlineDTOs
{
    public class AirlineGetAllDto: IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
