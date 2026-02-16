using FlightReservation_Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.DTOs.NotificationDTOs
{
    public class NotificationCreateDto: IDto
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public Guid AppUserId { get; set; }
    }
}
