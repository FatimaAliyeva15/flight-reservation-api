using FlightReservation_Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.Concretes
{
    public class Notification: BaseEntity
    {
        public string UserId { get; set; }
       // public AppUser User { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; } = false;
    }
}
