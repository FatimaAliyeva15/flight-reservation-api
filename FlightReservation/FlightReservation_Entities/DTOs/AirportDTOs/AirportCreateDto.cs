using FlightReservation_Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FlightReservation_Entities.DTOs.AirportDTOs
{
    public class AirportCreateDto: IDto
    {
        [Required]
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        [Required]
        [MaxLength(3, ErrorMessage = "IATA code must be 3 characters")]
        public string IATACode { get; set; }
    }
}
