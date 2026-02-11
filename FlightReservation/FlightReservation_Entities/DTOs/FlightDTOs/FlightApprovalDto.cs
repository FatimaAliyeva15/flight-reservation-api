using FlightReservation_Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.FlightDTOs
{
    public class FlightApprovalDto: IDto
    {
        public bool IsApproved { get; set; }
        public string? Comment { get; set; }
    }
}
