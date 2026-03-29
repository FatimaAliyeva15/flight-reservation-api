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
            var reservation = await _unitOfWork.ReservationRepository.GetAsync(r => r.Id == reservationId);
            if (reservation == null)
                throw new NotFoundException("Reservation not found");

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
            var reservation = await _unitOfWork.ReservationRepository.GetAsync(r => r.Id == reservationId);
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

        public async Task<IResult> CreateReservationWithTicketsAsync(ReservationCreateDto createDto, List<TicketCreateDto> ticketsDto)
        {
            var reservation = new Reservation
            {
                FlightId = createDto.FlightId,
                CreatedAt = DateTime.UtcNow,
                Status = ReservationStatus.PendingPayment,
                Tickets = new List<Ticket>()
            };

            await _unitOfWork.ReservationRepository.AddAsync(reservation);

            foreach (var ticketDto in ticketsDto)
            {
                var seat = await _unitOfWork.SeatRepository.GetAsync(s => createDto.SeatIds.Contains(s.Id) && s.Status == SeatStatus.Available);

                if (seat == null)
                    return new ErrorResult("One or more seats are not available");

                seat.Status = SeatStatus.Reserved;
                seat.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.SeatRepository.Update(seat);


                var ticket = new Ticket
                {
                    PassengerId = ticketDto.PassengerId,
                    FlightId = reservation.FlightId,
                    SeatId = seat.Id,
                    Reservation = reservation,
                    Price = 0,
                    CreatedAt = DateTime.UtcNow
                };

                reservation.Tickets.Add(ticket);
                await _unitOfWork.TicketRepository.AddAsync(ticket);
            }

            var result = await _unitOfWork.SaveAsync();
            return result > 0 ? new SuccessResult("Reservation with tickets created") : new ErrorResult("Creation failed");

        }

        public async Task<IDataResult<List<ReservationGetAllDto>>> GetAllDeletedReservationsAsync()
        {
            var deletedReservations = await _unitOfWork.ReservationRepository.GetDeletedAsync();
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

            var dtos = _mapper.Map<List<ReservationGetAllDto>>(reservations);
            return new SuccessDataResult<List<ReservationGetAllDto>>(dtos, "Reservations retrieved successfully");
        }

        public async Task<IDataResult<List<ReservationGetAllDto>>> GetAllReservationsPaginatedAsync(int page, int size)
        {
            if (page <= 0 || size <= 0)
                return new ErrorDataResult<List<ReservationGetAllDto>>(new List<ReservationGetAllDto>(), "Page or size invalid");

            var reservations = await _unitOfWork.ReservationRepository.GetAllPaginatedAsync(page, size, null, "Flight", "Tickets", "AppUser");
            if (reservations.Count == 0)
                return new ErrorDataResult<List<ReservationGetAllDto>>(new List<ReservationGetAllDto>(), "No reservations found");

            var dtos = _mapper.Map<List<ReservationGetAllDto>>(reservations);
            return new SuccessDataResult<List<ReservationGetAllDto>>(dtos, "Reservations retrieved with pagination");
        }

        public async Task<IDataResult<ReservationGetDto>> GetReservationByIdAsync(Guid id)
        {
            var reservation = await _unitOfWork.ReservationRepository.GetAsync(r => r.Id == id, includeDeleted: false, "Flight", "Tickets", "AppUser");
            if (reservation == null)
                return new ErrorDataResult<ReservationGetDto>("Reservation not found");

            var dto = _mapper.Map<ReservationGetDto>(reservation);
            return new SuccessDataResult<ReservationGetDto>(dto, "Reservation retrieved successfully");

        }

        public async Task<IDataResult<List<ReservationGetAllDto>>> GetReservationsByPassengerAsync(string userId)
        {
            var reservations = await _unitOfWork.ReservationRepository.GetAllAsync(r => r.AppUserId == userId, "Flight", "Tickets", "AppUser");
            if (reservations.Count == 0)
                return new ErrorDataResult<List<ReservationGetAllDto>>(new List<ReservationGetAllDto>(), "No reservations found for passenger");

            var dtos = _mapper.Map<List<ReservationGetAllDto>>(reservations);
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
