using FlightReservation_Entities.Common;
using FlightReservation_Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_Entities.Concretes
{
    public class Flight: BaseEntity
    {
        public string Airline { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public decimal Price { get; set; }

        public int TotalSeats { get; set; }

        public int AvailableSeats { get; set; }

        public FlightStatus Status { get; set; } = FlightStatus.PendingApproval;

        // Operator əlaqəsi
        public string OperatorId { get; set; }
        //public AppUser Operator { get; set; }

        public string? AdminComment { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
