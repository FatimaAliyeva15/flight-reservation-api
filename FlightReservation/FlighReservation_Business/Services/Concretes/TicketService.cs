using AutoMapper;
using FlighReservation_Business.Services.Abstracts;
using FlighReservation_Business.Utilities.Constants;
using FlightReservation_Core.Business.Utilities.Exceptions;
using FlightReservation_Core.Business.Utilities.Results.Abstract;
using FlightReservation_Core.Business.Utilities.Results.Concrete;
using FlightReservation_DataAccess.UnitOfWork.Abstract;
using FlightReservation_Entities.DTOs.TicketDTOs;
using System;
using System.Collections.Generic;
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

        public Task<IResult> AddTicketAsync(TicketCreateDto createDto)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult> AssignSeatToTicketAsync(Guid ticketId, Guid seatId)
        {
            var ticket = await _unitOfWork.TicketRepository.GetAsync(t => t.Id == ticketId);
            if (ticket == null)
                throw new NotFoundException(ExceptionMessage.TicketNotFound);

            var seat = await _unitOfWork.SeatRepository.GetAsync(s => s.Id == seatId && !s.IsBooked);
            if (seat == null)
                return new ErrorResult("Seat not available");

            seat.IsBooked = true;
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
                seat.IsBooked = false;
                await _unitOfWork.SeatRepository.Update(seat);
            }

            ticket.IsDeleted = true;
            ticket.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.TicketRepository.Update(ticket);

            var result = await _unitOfWork.SaveAsync();
            return result > 0 ? new SuccessResult("Ticket cancelled") : new ErrorResult("Ticket cancellation failed");
        }

        public Task<IDataResult<List<TicketGetAllDto>>> GetAllDeletedTicketsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<List<TicketGetAllDto>>> GetAllTicketsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<List<TicketGetAllDto>>> GetAllTicketsPaginatedAsync(int page, int size)
        {
            throw new NotImplementedException();
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

        public Task<IResult> HardDeleteTicketAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> RecoverTicketAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> SoftDeleteTicketAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> UpdateTicketAsync(Guid id, TicketUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
