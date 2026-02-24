using AutoMapper;
using FlighReservation_Business.Services.Abstracts;
using FlighReservation_Business.Utilities.Constants;
using FlightReservation_Core.Business.Utilities.Exceptions;
using FlightReservation_Core.Business.Utilities.Results.Abstract;
using FlightReservation_Core.Business.Utilities.Results.Concrete;
using FlightReservation_DataAccess.UnitOfWork.Abstract;
using FlightReservation_Entities.DTOs.SeatDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Services.Concretes
{
    public class SeatService : ISeatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SeatService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<IResult> AddSeatAsync(SeatCreateDto createDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult> BookSeatAsync(Guid seatId, Guid reservationId)
        {
            return await ReserveSeatAsync(seatId, reservationId);
        }

        public Task<IDataResult<List<SeatGetAllDto>>> GetAllDeletedSeatsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<List<SeatGetAllDto>>> GetAllSeatsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<List<SeatGetAllDto>>> GetAllSeatsPaginatedAsync(int page, int size)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SeatGetDto>> GetSeatByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<List<SeatGetAllDto>>> GetSeatsByFlightAsync(Guid flightId, bool onlyAvailable = false)
        {
            var seats = await _unitOfWork.SeatRepository.GetAllAsync(s => s.FlightId == flightId);
            if (onlyAvailable)
                seats = seats.Where(s => !s.IsBooked).ToList();

            var dtos = _mapper.Map<List<SeatGetAllDto>>(seats);
            return new SuccessDataResult<List<SeatGetAllDto>>(dtos, "Seats retrieved for flight");
        }

        public Task<IResult> HardDeleteSeatAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> RecoverSeatAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult> ReleaseSeatAsync(Guid seatId)
        {
            var seat = await _unitOfWork.SeatRepository.GetAsync(s => s.Id == seatId);
            if (seat == null) 
                throw new NotFoundException(ExceptionMessage.SeatNotFound);

            seat.TicketId = null;
            seat.IsBooked = false;
            seat.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SeatRepository.Update(seat);
            var result = await _unitOfWork.SaveAsync();
            return result > 0 ? new SuccessResult("Seat released") : new ErrorResult("Seat not released");
        }

        public async Task<IResult> ReserveSeatAsync(Guid seatId, Guid reservationId)
        {
            var seat = await _unitOfWork.SeatRepository.GetAsync(s => s.Id == seatId);
            if (seat == null) 
                throw new NotFoundException(ExceptionMessage.SeatNotFound);

            if (seat.IsBooked)
                return new ErrorResult("Seat already booked");

            seat.TicketId = reservationId;
            seat.IsBooked = true;
            seat.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SeatRepository.Update(seat);
            var result = await _unitOfWork.SaveAsync();
            return result > 0 ? new SuccessResult("Seat reserved") : new ErrorResult("Seat not reserved");
        }

        public Task<IResult> SoftDeleteSeatAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> UpdateSeatAsync(Guid id, SeatUpdateDto updateDTO)
        {
            throw new NotImplementedException();
        }
    }
}
