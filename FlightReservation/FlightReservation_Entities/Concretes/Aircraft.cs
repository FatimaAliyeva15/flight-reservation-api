using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.Concretes
{
    public class Aircraft: BaseEntity, IEntity
    {
        public string Model { get; set; }
        public int Capacity { get; set; }

        public Guid AirlineId { get; set; }
        public Airline Airline { get; set; }

        public ICollection<Flight> Flights { get; set; }
    }
}
