using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.Concretes
{
    public class Airport: BaseEntity, IEntity
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string IATACode { get; set; }   

        public ICollection<Flight> DepartureFlights { get; set; }
        public ICollection<Flight> ArrivalFlights { get; set; }
    }
}
