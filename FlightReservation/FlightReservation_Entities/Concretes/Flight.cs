using FlightReservation_Core.Entities.Abstract;
using FlightReservation_Core.Entities.Common;
using FlightReservation_Core.Entities.Concrete.Auth;
using FlightReservation_Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.Concretes
{
    public class Flight: BaseEntity, IEntity
    {
        public string FlightNumber { get; set; }
        public decimal Price { get; set; }
        public string AdminComment { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public FlightStatus Status { get; set; } = FlightStatus.PendingApproval;

        public Guid AirlineId { get; set; }
        public Airline Airline { get; set; }

        public Guid AircraftId { get; set; }
        public Aircraft Aircraft { get; set; }

        public Guid DepartureAirportId { get; set; }
        public Airport DepartureAirport { get; set; }

        public Guid ArrivalAirportId { get; set; }
        public Airport ArrivalAirport { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Seat> Seats { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
