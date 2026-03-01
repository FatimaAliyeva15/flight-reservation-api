using AutoMapper;
using FlighReservation_Business.Services.Abstracts;
using FlighReservation_Business.Utilities.Constants;
using FlightReservation_Core.Business.Utilities.Exceptions;
using FlightReservation_Core.Business.Utilities.Results.Abstract;
using FlightReservation_Core.Business.Utilities.Results.Concrete;
using FlightReservation_DataAccess.UnitOfWork.Abstract;
using FlightReservation_Entities.Concretes;
using FlightReservation_Entities.DTOs.AircraftDTOs;
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

        public async Task<IResult> AddSeatAsync(SeatCreateDto createDTO)
        {
            var exists = await _unitOfWork.SeatRepository.GetAsync(a =>
            a.SeatNumber.ToLower() == createDTO.SeatNumber.ToLower() &&
            a.FlightId == createDTO.FlightId);

            if (exists != null)
                return new ErrorResult("This seat number already exists");

            var seat = _mapper.Map<Seat>(createDTO);
            seat.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.SeatRepository.AddAsync(seat);
            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
            {
                return new ErrorResult("Seat not added");
            }

            return new SuccessResult("Seat added");
        }

        public async Task<IResult> BookSeatAsync(Guid seatId, Guid reservationId)
        {
            return await ReserveSeatAsync(seatId, reservationId);
        }

        public async Task<IDataResult<List<SeatGetAllDto>>> GetAllDeletedSeatsAsync()
        {
            var deletedSeats = await _unitOfWork.SeatRepository.GetDeletedAsync();

            if (deletedSeats == null || deletedSeats.Count == 0)
                return new ErrorDataResult<List<SeatGetAllDto>>(new List<SeatGetAllDto>(), "Deleted seats not found");

            var dtos = _mapper.Map<List<SeatGetAllDto>>(deletedSeats);
            return new SuccessDataResult<List<SeatGetAllDto>>(dtos, "Deleted seats retrieved successfully");
        }

        public async Task<IDataResult<List<SeatGetAllDto>>> GetAllSeatsAsync()
        {
            var seats = await _unitOfWork.SeatRepository.GetAllAsync();
            if (seats.Count == 0)
                return new ErrorDataResult<List<SeatGetAllDto>>(new List<SeatGetAllDto>(), "Seat not founded");

            var dtos = _mapper.Map<List<SeatGetAllDto>>(seats);
            return new SuccessDataResult<List<SeatGetAllDto>>(dtos, "Seats founded");
        }

        public async Task<IDataResult<List<SeatGetAllDto>>> GetAllSeatsPaginatedAsync(int page, int size)
        {
            if (page <= 0 || size <= 0)
                return new ErrorDataResult<List<SeatGetAllDto>>(
                    new List<SeatGetAllDto>(),
                    "Page or size invalid");

            var seats = await _unitOfWork.SeatRepository
                .GetAllPaginatedAsync(page, size, null, "Flight", "Ticket");

            if (seats.Count == 0)
                return new ErrorDataResult<List<SeatGetAllDto>>(
                    new List<SeatGetAllDto>(),
                    "No seat found");

            var dtos = _mapper.Map<List<SeatGetAllDto>>(seats);

            return new SuccessDataResult<List<SeatGetAllDto>>(
                dtos,
                "Seats listed with pagination");
        }

        public async Task<IDataResult<SeatGetDto>> GetSeatByIdAsync(Guid id)
        {
            var existsSeat = await _unitOfWork.SeatRepository.GetAsync(a => a.Id == id, includeDeleted: false, "Flight", "Ticket");
            if (existsSeat == null)
                return new ErrorDataResult<SeatGetDto>(_mapper.Map<SeatGetDto>(existsSeat), "Seat not founded");

            var dto = _mapper.Map<SeatGetDto>(existsSeat);

            return new SuccessDataResult<SeatGetDto>(dto, "Seat founded");
        }

        public async Task<IDataResult<List<SeatGetAllDto>>> GetSeatsByFlightAsync(Guid flightId, bool onlyAvailable = false)
        {
            var seats = await _unitOfWork.SeatRepository.GetAllAsync(s => s.FlightId == flightId);
            if (onlyAvailable)
                seats = seats.Where(s => !s.IsBooked).ToList();

            var dtos = _mapper.Map<List<SeatGetAllDto>>(seats);
            return new SuccessDataResult<List<SeatGetAllDto>>(dtos, "Seats retrieved for flight");
        }

        public async Task<IResult> HardDeleteSeatAsync(Guid id)
        {
            var existsSeat = await _unitOfWork.SeatRepository.GetAsync(a => a.Id == id, includeDeleted: true);
            if (existsSeat == null)
                throw new NotFoundException(ExceptionMessage.SeatNotFound);

            await _unitOfWork.SeatRepository.HardDeleteAsync(existsSeat);
            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
            {
                return new ErrorResult("Seat not permanently deleted");
            }

            return new SuccessResult("Seat permanently deleted");
        }

        public async Task<IResult> RecoverSeatAsync(Guid id)
        {
            var seat = await _unitOfWork.SeatRepository.GetAsync(a => a.Id == id, includeDeleted: true);

            if (seat == null)
                throw new NotFoundException(ExceptionMessage.SeatNotFound);

            await _unitOfWork.SeatRepository.RecoverAsync(seat);

            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Seat not recovered");

            return new SuccessResult("Seat recovered");
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

        public async Task<IResult> SoftDeleteSeatAsync(Guid id)
        {
            var existsSeat = await _unitOfWork.SeatRepository.GetAsync(a => a.Id == id);

            if (existsSeat == null)
                throw new NotFoundException(ExceptionMessage.SeatNotFound);

            await _unitOfWork.SeatRepository.SoftDeleteAsync(existsSeat);

            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Seat not deleted");

            return new SuccessResult("Seat softdeleted");
        }

        public async Task<IResult> UpdateSeatAsync(Guid id, SeatUpdateDto updateDTO)
        {
            var existsSeat = await _unitOfWork.SeatRepository.GetAsync(a => a.Id == id);
            if (existsSeat == null)
                throw new NotFoundException(ExceptionMessage.SeatNotFound);

            existsSeat.SeatNumber = updateDTO.SeatNumber ?? existsSeat.SeatNumber;
            existsSeat.Class = updateDTO.Class != null ? updateDTO.Class.Value : existsSeat.Class;
            existsSeat.FlightId = updateDTO.FlightId != Guid.Empty ? updateDTO.FlightId : existsSeat.FlightId;

            if (updateDTO.IsBooked.HasValue)
                existsSeat.IsBooked = updateDTO.IsBooked.Value;

            existsSeat.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SeatRepository.Update(existsSeat);
            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Seat not updated");

            return new SuccessResult("Seat updated");
        }
    }
}
