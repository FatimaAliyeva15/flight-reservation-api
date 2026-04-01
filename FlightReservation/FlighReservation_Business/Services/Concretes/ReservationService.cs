using AutoMapper;
using FlighReservation_Business.Services.Abstracts;
using FlighReservation_Business.Utilities.Constants;
using FlightReservation_Core.Business.Utilities.Exceptions;
using FlightReservation_Core.Business.Utilities.Results.Abstract;
using FlightReservation_Core.Business.Utilities.Results.Concrete;
using FlightReservation_DataAccess.UnitOfWork.Abstract;
using FlightReservation_Entities.Concretes;
using FlightReservation_Entities.DTOs.ReservationDTOs;
using FlightReservation_Entities.DTOs.TicketDTOs;
using FlightReservation_Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Services.Concretes
{
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISeatService _seatService;

        public ReservationService(IUnitOfWork unitOfWork, IMapper mapper, ISeatService seatService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _seatService = seatService;
        }

        public async Task<IResult> AddReservationAsync(ReservationCreateDto createDto, string userId)
        {
            var seats = await _unitOfWork.SeatRepository.GetAllAsync(s => createDto.SeatIds.Contains(s.Id));

            if (seats.Count != createDto.SeatIds.Count)
                return new ErrorResult("Some seats not found");

            var alreadyTaken = seats.Where(s => s.Status != SeatStatus.Available).ToList();
            if (alreadyTaken.Any())
                return new ErrorResult($"Seats already reserved or booked: {string.Join(", ", alreadyTaken.Select(s => s.SeatNumber))}");

            foreach (var seat in seats)
            {
                seat.Status = SeatStatus.Reserved;
                seat.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.SeatRepository.Update(seat);
            }

            var reservation = new Reservation
            {
                FlightId = createDto.FlightId,
                AppUserId = userId,
                CreatedAt = DateTime.UtcNow,
                Status = ReservationStatus.PendingPayment,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15), 
                Tickets = new List<Ticket>() 
            };

            await _unitOfWork.ReservationRepository.AddAsync(reservation);

            var result = await _unitOfWork.SaveAsync();

            return result > 0
                ? new SuccessResult("Reservation added successfully")
                : new ErrorResult("Reservation not added");
        }

        public async Task<IResult> CancelReservationAsync(Guid reservationId)
        {
            var reservation = await _unitOfWork.ReservationRepository.GetAsync(r => r.Id == reservationId, false, "Tickets");

            if (reservation == null)
                throw new NotFoundException(ExceptionMessage.ReservationNotFound);

            if (reservation.Tickets == null || reservation.Tickets.Count == 0)
            {
                reservation.Status = ReservationStatus.Cancelled;
                reservation.CancelledAt = DateTime.UtcNow;
                reservation.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.ReservationRepository.Update(reservation);
                var resultOnlyReservation = await _unitOfWork.SaveAsync();

                return resultOnlyReservation > 0
                    ? new SuccessResult("Reservation cancelled (no tickets)")
                    : new ErrorResult("Cancellation failed");
            }

            foreach (var ticket in reservation.Tickets)
            {
                var seat = await _unitOfWork.SeatRepository.GetAsync(s => s.Id == ticket.SeatId);
                if (seat != null)
                {
                    seat.Status = SeatStatus.Available;
                    await _unitOfWork.SeatRepository.Update(seat);
                }

                ticket.IsDeleted = true;
                ticket.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.TicketRepository.Update(ticket);
            }

            reservation.Status = ReservationStatus.Cancelled;
            reservation.CancelledAt = DateTime.UtcNow;
            reservation.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.ReservationRepository.Update(reservation);
            var result = await _unitOfWork.SaveAsync();

            return result > 0 ? new SuccessResult("Reservation cancelled") : new ErrorResult("Cancellation failed");
        }

        public async Task<IResult> ConfirmReservationAfterPaymentAsync(Guid reservationId)
        {
            var reservation = await _unitOfWork.ReservationRepository.GetAsync(r => r.Id == reservationId, false, "Tickets");
            if (reservation == null)
                throw new NotFoundException(ExceptionMessage.ReservationNotFound);

            reservation.IsPaid = true;
            reservation.PaidAt = DateTime.UtcNow;
            reservation.Status = ReservationStatus.Confirmed;
            reservation.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.ReservationRepository.Update(reservation);
            var result = await _unitOfWork.SaveAsync();

            return result > 0 ? new SuccessResult("Reservation confirmed after payment") : new ErrorResult("Confirmation failed");

        }

        public async Task<IResult> CreateReservationWithTicketsAsync(ReservationCreateDto createDto, List<ReservationCreateWithTicketDto> ticketsDto, string userId)
        {
            var flight = await _unitOfWork.FlightRepository.GetAsync(f => f.Id == createDto.FlightId);

            if (flight == null)
                throw new NotFoundException(ExceptionMessage.FlightNotFound);


            var reservation = new Reservation
            {
                FlightId = createDto.FlightId,
                AppUserId = userId,
                CreatedAt = DateTime.UtcNow,
                Status = ReservationStatus.PendingPayment,
                TotalPrice = 0,
                Tickets = new List<Ticket>()
            };

            await _unitOfWork.ReservationRepository.AddAsync(reservation);
            await _unitOfWork.SaveAsync();

            for (int i = 0; i < createDto.SeatIds.Count; i++)
            {
                var seatId = createDto.SeatIds[i];
                var ticketDto = ticketsDto[i];

                var seat = await _unitOfWork.SeatRepository
                    .GetAsync(s => s.Id == seatId && s.Status == SeatStatus.Available);

                if (seat == null)
                    return new ErrorResult($"Seat {seatId} is not available");

                var ticket = new Ticket
                {
                    PassengerId = ticketDto.PassengerId,
                    FlightId = reservation.FlightId,
                    SeatId = seat.Id,
                    Reservation = reservation,
                    Price = flight.Price,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.TicketRepository.AddAsync(ticket);
                await _unitOfWork.SaveAsync();

                seat.Status = SeatStatus.Booked;
                seat.TicketId = ticket.Id;
                seat.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.SeatRepository.Update(seat);

                reservation.Tickets.Add(ticket);
                reservation.TotalPrice += flight.Price;

            }

            var result = await _unitOfWork.SaveAsync();
            return result > 0 
                ? new SuccessResult("Reservation with tickets created") 
                : new ErrorResult("Creation failed");

        }


        public async Task<IDataResult<List<ReservationGetAllDto>>> GetAllDeletedReservationsAsync()
        {
            var deletedReservations = await _unitOfWork.ReservationRepository.GetDeletedAsync("Flight", "Tickets", "AppUser");
            if (deletedReservations.Count == 0)
                return new ErrorDataResult<List<ReservationGetAllDto>>(new List<ReservationGetAllDto>(), "Deleted reservations not found");

            var dtos = _mapper.Map<List<ReservationGetAllDto>>(deletedReservations);
            return new SuccessDataResult<List<ReservationGetAllDto>>(dtos, "Deleted reservations retrieved successfully");

        }

        public async Task<IDataResult<List<ReservationGetAllDto>>> GetAllReservationsAsync()
        {
            var reservations = await _unitOfWork.ReservationRepository.GetAllAsync(null, "Flight", "Tickets", "AppUser");

            if (reservations.Count == 0)
                return new ErrorDataResult<List<ReservationGetAllDto>>(new List<ReservationGetAllDto>(), "No reservations found");

            var dtos = reservations.Select(r => new ReservationGetAllDto
            {
                Id = r.Id,
                TotalPrice = r.TotalPrice,
                Status = r.Status,
                IsPaid = r.IsPaid,
                AppUserName = r.AppUser?.FullName,
                FlightNumber = r.Flight?.FlightNumber,
                TicketCount = r.Tickets?.Count ?? 0
            }).ToList();

            return new SuccessDataResult<List<ReservationGetAllDto>>(dtos, "Reservations retrieved successfully");
        }

        public async Task<IDataResult<List<ReservationGetAllDto>>> GetAllReservationsPaginatedAsync(int page, int size)
        {
            if (page <= 0 || size <= 0)
                return new ErrorDataResult<List<ReservationGetAllDto>>(new List<ReservationGetAllDto>(), "Page or size invalid");

            var reservations = await _unitOfWork.ReservationRepository.GetAllPaginatedAsync(page, size, null, "Flight", "Tickets", "AppUser");
            if (reservations.Count == 0)
                return new ErrorDataResult<List<ReservationGetAllDto>>(new List<ReservationGetAllDto>(), "No reservations found");

            var dtos = reservations.Select(r => new ReservationGetAllDto
            {
                Id = r.Id,
                TotalPrice = r.TotalPrice,
                Status = r.Status,
                IsPaid = r.IsPaid,
                AppUserName = r.AppUser?.FullName,
                FlightNumber = r.Flight?.FlightNumber,
                TicketCount = r.Tickets?.Count ?? 0
            }).ToList();
            return new SuccessDataResult<List<ReservationGetAllDto>>(dtos, "Reservations retrieved with pagination");
        }

        public async Task<IDataResult<ReservationGetDto>> GetReservationByIdAsync(Guid id)
        {
            var reservation = await _unitOfWork.ReservationRepository.GetAsync(r => r.Id == id, includeDeleted: false, "Flight", "Tickets.Seat", "Tickets.Passenger", "AppUser");
            if (reservation == null)
                return new ErrorDataResult<ReservationGetDto>("Reservation not found");

            var dto = new ReservationGetDto
            {
                TotalPrice = reservation.TotalPrice,
                Status = reservation.Status,
                IsPaid = reservation.IsPaid,
                PaidAt = reservation.PaidAt,
                CancelledAt = reservation.CancelledAt,
                RefundedAt = reservation.RefundedAt,
                ExpiresAt = reservation.ExpiresAt,
                FlightId = reservation.FlightId,
                FlightNumber = reservation.Flight.FlightNumber,
                AppUserId = reservation.AppUserId,
                AppUserName = reservation.AppUser.FullName,
                Tickets = reservation.Tickets.Select(t => new TicketGetDto
                {
                    Price = t.Price,
                    FlightId = t.FlightId,
                    FlightNumber = t.Flight.FlightNumber,
                    PassengerId = t.PassengerId,
                    PassengerName = t.Passenger.FirstName + t.Passenger.LastName,
                    SeatId = t.SeatId,
                    SeatNumber = t.Seat.SeatNumber,
                    ReservationId = t.ReservationId
                }).ToList()
            };
            return new SuccessDataResult<ReservationGetDto>(dto, "Reservation retrieved successfully");

        }

        public async Task<IDataResult<List<ReservationGetAllDto>>> GetReservationsByPassengerAsync(string userId)
        {
            var reservations = await _unitOfWork.ReservationRepository.GetAllAsync(r => r.AppUserId == userId, "Flight", "Tickets", "AppUser");

            if (reservations.Count == 0)
                return new ErrorDataResult<List<ReservationGetAllDto>>(new List<ReservationGetAllDto>(), "No reservations found for passenger");

            var dtos = reservations.Select(r => new ReservationGetAllDto
            {
                Id = r.Id,
                TotalPrice = r.TotalPrice,
                Status = r.Status,
                IsPaid = r.IsPaid,
                AppUserName = r.AppUser?.FullName,
                FlightNumber = r.Flight?.FlightNumber,
                TicketCount = r.Tickets?.Count ?? 0
            }).ToList();

            return new SuccessDataResult<List<ReservationGetAllDto>>(dtos, "Passenger reservations retrieved");
        }

        //    public async Task<IDataResult<ReservationWithTicketsDto>> GetReservationWithTicketsAsync(Guid reservationId)
        //    {
        //        var reservation = await _unitOfWork.ReservationRepository.GetAsync(
        //    r => r.Id == reservationId,
        //    includeDeleted: false,
        //    "Tickets.Passenger",
        //    "Tickets.Seat",
        //    "Flight"
        //);

        //        if (reservation == null)
        //            return new ErrorDataResult<ReservationWithTicketsDto>("Reservation not found");

        //        var dto = new ReservationWithTicketsDto
        //        {
        //            ReservationId = reservation.Id,
        //            FlightId = reservation.FlightId,
        //            AppUserId = reservation.AppUserId,
        //            TotalPrice = reservation.TotalPrice,
        //            Status = reservation.Status,
        //            Tickets = _mapper.Map<List<TicketGetDto>>(reservation.Tickets)
        //        };

        //        return new SuccessDataResult<ReservationWithTicketsDto>(dto, "Reservation with tickets retrieved");
        //    }

        public async Task<IResult> HardDeleteReservationAsync(Guid id)
        {
            var reservation = await _unitOfWork.ReservationRepository.GetAsync(r => r.Id == id, true);
            if (reservation == null)
                throw new NotFoundException(ExceptionMessage.ReservationNotFound);

            await _unitOfWork.ReservationRepository.HardDeleteAsync(reservation);
            var result = await _unitOfWork.SaveAsync();

            return result > 0 ? new SuccessResult("Reservation permanently deleted") : new ErrorResult("Hard delete failed");
        }

        public async Task<IResult> RecoverReservationAsync(Guid id)
        {
            var reservation = await _unitOfWork.ReservationRepository.GetAsync(r => r.Id == id, true);
            if (reservation == null)
                throw new NotFoundException(ExceptionMessage.ReservationNotFound);

            reservation.IsDeleted = false;
            var result = await _unitOfWork.SaveAsync();

            return result > 0 ? new SuccessResult("Reservation recovered") : new ErrorResult("Recovery failed");

        }

        public async Task<IResult> SoftDeleteReservationAsync(Guid id)
        {
            var reservation = await _unitOfWork.ReservationRepository.GetAsync(r => r.Id == id);
            if (reservation == null)
                throw new NotFoundException(ExceptionMessage.ReservationNotFound);

            reservation.IsDeleted = true;
            reservation.UpdatedAt = DateTime.UtcNow;

            var result = await _unitOfWork.SaveAsync();
            return result > 0 ? new SuccessResult("Reservation soft deleted") : new ErrorResult("Soft delete failed");
        }

        public async Task<IResult> UpdateReservationAsync(Guid id, ReservationUpdateDto updateDto)
        {
            var reservation = await _unitOfWork.ReservationRepository.GetAsync(r => r.Id == id);
            if (reservation == null)
                throw new NotFoundException("Reservation not found");

            reservation.Status = updateDto.Status ?? reservation.Status;
            reservation.ExpiresAt = updateDto.ExpiresAt != default ? updateDto.ExpiresAt : reservation.ExpiresAt;

            if (updateDto.IsPaid.HasValue)
                reservation.IsPaid = updateDto.IsPaid.Value;

            reservation.UpdatedAt = DateTime.UtcNow;


            await _unitOfWork.ReservationRepository.Update(reservation);
            var result = await _unitOfWork.SaveAsync();

            return result > 0 ? new SuccessResult("Reservation updated successfully") : new ErrorResult("Reservation not updated");

        }
    }
}
