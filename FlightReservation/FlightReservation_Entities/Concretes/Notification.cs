using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Core.Entities.Concrete.Auth;
using FlightReservation_Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.Concretes
{
    public class Notification: BaseEntity, IEntity
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public Guid UserId { get; set; }
        public AppUser User { get; set; }

    }
}
