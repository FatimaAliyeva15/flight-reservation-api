using FlightReservation_DataAccess.EFCore;
using FlightReservation_DataAccess.Repositories.Abstracts;
using FlightReservation_DataAccess.Repositories.Concretes;
using FlightReservation_DataAccess.UnitOfWork.Abstract;

namespace FlightReservation_DataAccess.UnitOfWork.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IAirlineRepository _airlineRepository;
        private readonly IAirportRepository _airportRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IPassengerRepository _passengerRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IPaymentRepository _paymentRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IAirlineRepository AirlineRepository => _airlineRepository ?? new AirlineRepository(_context);

        public IAirportRepository AirportRepository => _airportRepository ?? new AirportRepository(_context);

        public IAircraftRepository AircraftRepository => _aircraftRepository ?? new AircraftRepository(_context);
        public IFlightRepository FlightRepository => _flightRepository ?? new FlightRepository(_context);

        public INotificationRepository NotificationRepository => _notificationRepository ?? new NotificationRepository(_context);

        public IPassengerRepository PassengerRepository => _passengerRepository ?? new PassengerRepository(_context);

        public ISeatRepository SeatRepository => _seatRepository ?? new SeatRepository(_context);

        public ITicketRepository TicketRepository => _ticketRepository ?? new TicketRepository(_context);

        public IReservationRepository ReservationRepository => _reservationRepository ?? new ReservationRepository(_context);

        public IPaymentRepository PaymentRepository => _paymentRepository ?? new PaymentRepository(_context);

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
