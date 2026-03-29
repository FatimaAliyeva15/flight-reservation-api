using AutoMapper;
using FlighReservation_Business.Services.Abstracts;
using FlighReservation_Business.Utilities.Constants;
using FlightReservation_Core.Business.Utilities.Exceptions;
using FlightReservation_Core.Business.Utilities.Results.Abstract;
using FlightReservation_Core.Business.Utilities.Results.Concrete;
using FlightReservation_Core.Entities.Abstract;
using FlightReservation_DataAccess.UnitOfWork.Abstract;
using FlightReservation_Entities.Concretes;
using FlightReservation_Entities.DTOs.AircraftDTOs;
using FlightReservation_Entities.DTOs.ReservationDTOs;
using FlightReservation_Entities.DTOs.TicketDTOs;
using FlightReservation_Entities.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace FlighReservation_Business.Services.Concretes
{
    public class TicketService: ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TicketService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResult> AddTicketAsync(TicketCreateDto createDto)
        {
            var reservation = await _unitOfWork.ReservationRepository.GetAsync(r => r.Id == createDto.ReservationId, includeDeleted: false, "Tickets");

            if (reservation == null)
                throw new NotFoundException(ExceptionMessage.ReservationNotFound);


            var flight = await _unitOfWork.FlightRepository.GetAsync(f => f.Id == reservation.FlightId);
            if (flight == null)
                throw new NotFoundException(ExceptionMessage.FlightNotFound);

            var passenger = await _unitOfWork.PassengerRepository.GetAsync(p => p.Id == createDto.PassengerId);
            if (passenger == null)
                throw new NotFoundException(ExceptionMessage.PassengerNotFound);

            var seat = await _unitOfWork.SeatRepository.GetAsync(s => s.FlightId == reservation.FlightId && s.Status == SeatStatus.Reserved && s.TicketId == null);

            if (seat == null)
                return new ErrorResult("No reserved seat available");

            var ticket = new Ticket
            {
                ReservationId = reservation.Id,
                PassengerId = createDto.PassengerId,
                FlightId = reservation.FlightId,
                SeatId = seat.Id,
                Price = flight.Price,
                CreatedAt = DateTime.UtcNow

            };

            await _unitOfWork.TicketRepository.AddAsync(ticket);


            seat.Status = SeatStatus.Booked;
            seat.TicketId = ticket.Id;
            seat.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.SeatRepository.Update(seat);

            reservation.Tickets ??= new List<Ticket>();
            reservation.Tickets.Add(ticket);
            reservation.TotalPrice += ticket.Price;


            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
            {
                return new ErrorResult("Ticket not added");
            }

            return new SuccessResult("Ticket added");
        }

        public async Task<IResult> AssignSeatToTicketAsync(Guid ticketId, Guid seatId)
        {
            var ticket = await _unitOfWork.TicketRepository.GetAsync(t => t.Id == ticketId);
            if (ticket == null)
                throw new NotFoundException(ExceptionMessage.TicketNotFound);

            var seat = await _unitOfWork.SeatRepository.GetAsync(s => s.Id == seatId && s.Status == SeatStatus.Available);
            if (seat == null)
                return new ErrorResult("Seat not available");

            seat.Status = SeatStatus.Reserved;
            seat.UpdatedAt = DateTime.UtcNow;

            ticket.SeatId = seat.Id;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SeatRepository.Update(seat);
            await _unitOfWork.TicketRepository.Update(ticket);

            var result = await _unitOfWork.SaveAsync();
            return result > 0 ? new SuccessResult("Seat assigned to ticket") : new ErrorResult("Failed to assign seat");
        }

        public async Task<IResult> CancelTicketAsync(Guid ticketId)
        {
            var ticket = await _unitOfWork.TicketRepository.GetAsync(t => t.Id == ticketId);
            if (ticket == null)
                throw new NotFoundException(ExceptionMessage.TicketNotFound);

            var seat = await _unitOfWork.SeatRepository.GetAsync(s => s.Id == ticket.SeatId);
            if (seat != null)
            {
                seat.Status = SeatStatus.Available; 
                seat.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.SeatRepository.Update(seat);
            }

            ticket.IsDeleted = true;
            ticket.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.TicketRepository.Update(ticket);

            var result = await _unitOfWork.SaveAsync();
            return result > 0 ? new SuccessResult("Ticket cancelled") : new ErrorResult("Ticket cancellation failed");
        }

        public async Task<IDataResult<List<TicketGetAllDto>>> GetAllDeletedTicketsAsync()
        {
            var deletedTickets = await _unitOfWork.TicketRepository.GetDeletedAsync();

            if (deletedTickets == null || deletedTickets.Count == 0)
                return new ErrorDataResult<List<TicketGetAllDto>>(new List<TicketGetAllDto>(), "Deleted tickets not found");

            var dtos = _mapper.Map<List<TicketGetAllDto>>(deletedTickets);
            return new SuccessDataResult<List<TicketGetAllDto>>(dtos, "Deleted tickets retrieved successfully");
        }

        public async Task<IDataResult<List<TicketGetAllDto>>> GetAllTicketsAsync()
        {
            var tickets = await _unitOfWork.TicketRepository.GetAllAsync(null, "Flight", "Passenger", "Reservation","Seat");
            if (tickets.Count == 0)
                return new ErrorDataResult<List<TicketGetAllDto>>(new List<TicketGetAllDto>(), "Ticket not founded");

            var dtos = _mapper.Map<List<TicketGetAllDto>>(tickets);
            return new SuccessDataResult<List<TicketGetAllDto>>(dtos, "Tickets founded");
        }

        public async Task<IDataResult<List<TicketGetAllDto>>> GetAllTicketsPaginatedAsync(int page, int size)
        {
            if (page <= 0 || size <= 0)
                return new ErrorDataResult<List<TicketGetAllDto>>(
                    new List<TicketGetAllDto>(),
                    "Page or size invalid");

            var tickets = await _unitOfWork.TicketRepository.GetAllPaginatedAsync(page, size, null, "Flight", "Passenger", "Reservation", "Seat");

            if (tickets.Count == 0)
                return new ErrorDataResult<List<TicketGetAllDto>>(
                    new List<TicketGetAllDto>(),
                    "No ticket found");

            var dtos = _mapper.Map<List<TicketGetAllDto>>(tickets);

            return new SuccessDataResult<List<TicketGetAllDto>>(
                dtos,
                "Tickets listed with pagination");
        }

        public Task<IDataResult<TicketGetDto>> GetTicketByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<List<TicketGetAllDto>>> GetTicketsByPassengerAsync(Guid passengerId)
        {
            var tickets = await _unitOfWork.TicketRepository.GetAllAsync(t => t.PassengerId == passengerId, "Flight", "Seat", "Reservation");
            var dtos = _mapper.Map<List<TicketGetAllDto>>(tickets);
            return new SuccessDataResult<List<TicketGetAllDto>>(dtos, "Tickets retrieved for passenger");
        }

        public async Task<IDataResult<List<TicketGetAllDto>>> GetTicketsByReservationAsync(Guid reservationId)
        {
            var tickets = await _unitOfWork.TicketRepository.GetAllAsync(t => t.ReservationId == reservationId, "Passenger", "Seat", "Flight");
            var dtos = _mapper.Map<List<TicketGetAllDto>>(tickets);
            return new SuccessDataResult<List<TicketGetAllDto>>(dtos, "Tickets retrieved for reservation");
        }

        public async Task<IResult> HardDeleteTicketAsync(Guid id)
        {
            var existsTicket = await _unitOfWork.TicketRepository.GetAsync(a => a.Id == id, includeDeleted: true);
            if (existsTicket == null)
                throw new NotFoundException(ExceptionMessage.TicketNotFound);

            await _unitOfWork.TicketRepository.HardDeleteAsync(existsTicket);
            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
            {
                return new ErrorResult("Ticket not permanently deleted");
            }

            return new SuccessResult("Ticket permanently deleted");
        }

        public async Task<IResult> RecoverTicketAsync(Guid id)
        {
            var ticket = await _unitOfWork.TicketRepository.GetAsync(a => a.Id == id, includeDeleted: true);

            if (ticket == null)
                throw new NotFoundException(ExceptionMessage.TicketNotFound);

            await _unitOfWork.TicketRepository.RecoverAsync(ticket);

            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Ticket not recovered");

            return new SuccessResult("Ticket recovered");
        }

        public async Task<IResult> SoftDeleteTicketAsync(Guid id)
        {
            var existsTicket = await _unitOfWork.TicketRepository.GetAsync(a => a.Id == id);

            if (existsTicket == null)
                throw new NotFoundException(ExceptionMessage.TicketNotFound);

            await _unitOfWork.TicketRepository.SoftDeleteAsync(existsTicket);

            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Ticket not deleted");

            return new SuccessResult("Ticket softdeleted");
        }

        public async Task<IResult> UpdateTicketAsync(Guid id, TicketUpdateDto updateDto)
        {
            var ticket = await _unitOfWork.TicketRepository.GetAsync(a => a.Id == id);
            if (ticket == null)
                throw new NotFoundException(ExceptionMessage.TicketNotFound);

            ticket.SeatId = updateDto.SeatId != Guid.Empty ? updateDto.SeatId : ticket.SeatId;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.TicketRepository.Update(ticket);
            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Ticket not updated");

            return new SuccessResult("Ticket updated");
        }
    }
}
