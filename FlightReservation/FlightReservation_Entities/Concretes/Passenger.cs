using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.Concretes
{
    public class Passenger: BaseEntity, IEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PassportNumber { get; set; }
        public DateTime DateOfBirth { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}
