using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.Concretes
{
    public class Airline: BaseEntity, IEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public ICollection<Aircraft> Aircrafts { get; set; }
        public ICollection<Flight> Flights { get; set; }
    }
}
