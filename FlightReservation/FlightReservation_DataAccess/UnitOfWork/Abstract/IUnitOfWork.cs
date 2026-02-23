using FlightReservation_DataAccess.Repositories.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightReservation_DataAccess.UnitOfWork.Abstract
{
    public interface IUnitOfWork
    {
        public IAircraftRepository AircraftRepository { get; }
        public IAirlineRepository AirlineRepository { get; }
        public IAirportRepository AirportRepository { get; }
        public IFlightRepository FlightRepository { get; }
        public INotificationRepository NotificationRepository { get; }
        public IPassengerRepository PassengerRepository { get; }
        public ISeatRepository SeatRepository { get; }
        public ITicketRepository TicketRepository { get; }
        public IReservationRepository ReservationRepository { get; }
        public IPaymentRepository PaymentRepository { get; }

        public Task<int> SaveAsync();
    }
}
