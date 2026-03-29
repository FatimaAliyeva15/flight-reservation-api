using AutoMapper;
using FlighReservation_Business.Services.Abstracts;
using FlighReservation_Business.Utilities.Constants;
using FlightReservation_Core.Business.Utilities.Exceptions;
using FlightReservation_Core.Business.Utilities.Results.Abstract;
using FlightReservation_Core.Business.Utilities.Results.Concrete;
using FlightReservation_DataAccess.UnitOfWork.Abstract;
using FlightReservation_Entities.Concretes;
using FlightReservation_Entities.DTOs.FlightDTOs;
using FlightReservation_Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Services.Concretes
{
    public class FlightService : IFlightService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FlightService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResult> AddFlightAsync(FlightCreateDto createDto)
        {
            var aircraft = await _unitOfWork.AircraftRepository.GetAsync(a => a.Id == createDto.AircraftId);

            if (aircraft == null)
                throw new NotFoundException(ExceptionMessage.AircraftNotFound);

            var flight = _mapper.Map<Flight>(createDto);

            await _unitOfWork.FlightRepository.AddAsync(flight);

            for (int i = 1; i <= aircraft.Capacity; i++)
            {
                SeatClass seatClass;

                if (i <= 10)
                    seatClass = SeatClass.First;
                else if (i <= 30)
                    seatClass = SeatClass.Business;
                else
                    seatClass = SeatClass.Economy;

                var seat = new Seat
                {
                    FlightId = flight.Id,
                    SeatNumber = i.ToString(),
                    Class = seatClass,
                    Status = SeatStatus.Available,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.SeatRepository.AddAsync(seat);
            }

            var result = await _unitOfWork.SaveAsync();
            if (result == 0)
                return new ErrorResult("Flights not created");

            return new SuccessResult("Flight created successfully");
        }

        public async Task<IResult> ApproveFlightAsync(Guid id)
        {
            var flight = await _unitOfWork.FlightRepository.GetAsync(f => f.Id == id);

            if (flight == null)
                throw new NotFoundException(ExceptionMessage.FlightNotFound);

            if (flight.Status == FlightStatus.Approved)
                return new ErrorResult("Flight already approved");

            if (flight.Status == FlightStatus.Cancelled)
                return new ErrorResult("Cancelled flight cannot be approved");

            flight.Status = FlightStatus.Approved;
            flight.AdminComment = null;
            flight.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.FlightRepository.Update(flight);
            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Flight not approved");

            return new SuccessResult("Flight approved");
        }

        public async Task<IResult> CancelFlightAsync(Guid id)
        {
            var flight = await _unitOfWork.FlightRepository.GetAsync(f => f.Id == id);

            if (flight == null)
                throw new NotFoundException(ExceptionMessage.FlightNotFound);

            if (flight.Status == FlightStatus.Cancelled)
                return new ErrorResult("Flights already cancelled");

            flight.Status = FlightStatus.Cancelled;
            flight.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.FlightRepository.Update(flight);

            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Flight not cancelled");

            return new SuccessResult("Flight cancelled");
        }

        public async Task<IDataResult<List<FlightGetAllDto>>> GetAllDeletedFlightsAsync()
        {
            var deletedFlights = await _unitOfWork.FlightRepository.GetDeletedAsync();

            if (deletedFlights.Count == 0)
                return new ErrorDataResult<List<FlightGetAllDto>>(new List<FlightGetAllDto>(), "Deleted flights not found");

            var dtos = _mapper.Map<List<FlightGetAllDto>>(deletedFlights);

            return new SuccessDataResult<List<FlightGetAllDto>>(dtos, "Deleted flights retrieved successfully");
        }

        public async Task<IDataResult<List<FlightGetAllDto>>> GetAllFlightsAsync()
        {
            var flights = await _unitOfWork.FlightRepository.GetAllAsync(null,
                 "Airline",
                 "Aircraft",
                 "DepartureAirport",
                 "ArrivalAirport",
                 "Seats");

            if (flights.Count == 0)
                return new ErrorDataResult<List<FlightGetAllDto>>(new List<FlightGetAllDto>(), "Flights not found");

            var dtos = _mapper.Map<List<FlightGetAllDto>>(flights);
            return new SuccessDataResult<List<FlightGetAllDto>>(dtos, "Flights retrieved successfully");
        }

        public async Task<IDataResult<List<FlightGetAllDto>>> GetAllFlightsPaginatedAsync(int page, int size)
        {
            if (page <= 0 || size <= 0)
                return new ErrorDataResult<List<FlightGetAllDto>>(new List<FlightGetAllDto>(), "Page or size invalid");

            var flights = await _unitOfWork.FlightRepository.GetAllPaginatedAsync(page, size, null,
                "Airline",
                "Aircraft",
                "DepartureAirport",
                "ArrivalAirport",
                "Seats"
            );

            if (flights.Count == 0)
                return new ErrorDataResult<List<FlightGetAllDto>>(new List<FlightGetAllDto>(), "No flights found");

            var dtos = _mapper.Map<List<FlightGetAllDto>>(flights);

            return new SuccessDataResult<List<FlightGetAllDto>>(dtos, "Flights retrieved with pagination");
        }

        public async Task<IDataResult<FlightGetDto>> GetFlightByIdAsync(Guid id)
        {
            var flight = await _unitOfWork.FlightRepository.GetAsync( f => f.Id == id, includeDeleted: false,
             "Airline",
             "Aircraft",
             "DepartureAirport",
             "ArrivalAirport",
             "Seats",
             "Tickets");

            if (flight == null)
                return new ErrorDataResult<FlightGetDto>("Flight not found");

            var dto = _mapper.Map<FlightGetDto>(flight);

            dto.AirlineName = flight.Airline?.Name;
            dto.AircraftModel = flight.Aircraft?.Model;
            dto.DepartureAirportName = flight.DepartureAirport?.Name;
            dto.ArrivalAirportName = flight.ArrivalAirport?.Name;
            dto.TotalSeats = flight.Seats?.Count ?? 0;
            dto.AvailableSeats = flight.Seats?.Count(s => s.Status == SeatStatus.Available) ?? 0;

            return new SuccessDataResult<FlightGetDto>(dto, "Flight found");
        }

        public async Task<IResult> HardDeleteFlightAsync(Guid id)
        {
            var flight = await _unitOfWork.FlightRepository.GetAsync(f => f.Id == id, true);

            if (flight == null)
                throw new NotFoundException(ExceptionMessage.FlightNotFound);

            await _unitOfWork.FlightRepository.HardDeleteAsync(flight);

            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Flight not permanently deleted");

            return new SuccessResult("Flight permanently deleted");
        }

        public async Task<IResult> RecoverFlightAsync(Guid id)
        {
            var flight = await _unitOfWork.FlightRepository.GetAsync(f => f.Id == id, true);

            if (flight == null)
                throw new NotFoundException(ExceptionMessage.FlightNotFound);

            flight.IsDeleted = false;

            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Flight not recovered");

            return new SuccessResult("Flight recovered");
        }

        public async Task<IResult> RejectFlightAsync(Guid id, string adminComment)
        {
            var flight = await _unitOfWork.FlightRepository.GetAsync(f => f.Id == id);

            if (flight == null)
                throw new NotFoundException(ExceptionMessage.FlightNotFound);

            if (flight.Status == FlightStatus.Approved)
                return new ErrorResult("Approved flight cannot be rejected");

            if (flight.Status == FlightStatus.Cancelled)
                return new ErrorResult("Cancelled flight cannot be rejected");

            if (string.IsNullOrWhiteSpace(adminComment))
                return new ErrorResult("Admin comment is required");

            flight.Status = FlightStatus.Rejected;
            flight.AdminComment = adminComment;
            flight.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.FlightRepository.Update(flight);
            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Flight not rejected");

            return new SuccessResult("Flight rejected");
        }

        public async Task<IResult> SoftDeleteFlightAsync(Guid id)
        {
            var flight = await _unitOfWork.FlightRepository.GetAsync(f => f.Id == id);

            if (flight == null)
                throw new NotFoundException(ExceptionMessage.FlightNotFound);

            await _unitOfWork.FlightRepository.SoftDeleteAsync(flight);
            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Flight not deleted");

            return new SuccessResult("Flight deleted");
        }

        public async Task<IResult> UpdateFlightAsync(Guid id, FlightUpdateDto updateDTO)
        {
            var flight = await _unitOfWork.FlightRepository.GetAsync(f => f.Id == id);

            if (flight == null)
                throw new NotFoundException(ExceptionMessage.FlightNotFound);

            flight.FlightNumber = updateDTO.FlightNumber ?? flight.FlightNumber;
            flight.Price = updateDTO.Price != 0 ? updateDTO.Price : flight.Price;
            flight.AdminComment = updateDTO.AdminComment ?? flight.AdminComment;
            flight.DepartureTime = updateDTO.DepartureTime != default ? updateDTO.DepartureTime : flight.DepartureTime;
            flight.ArrivalTime = updateDTO.ArrivalTime != default ? updateDTO.ArrivalTime : flight.ArrivalTime;
            flight.Status = updateDTO.Status != null ? updateDTO.Status.Value : flight.Status;

            flight.AirlineId = updateDTO.AirlineId != Guid.Empty ? updateDTO.AirlineId : flight.AirlineId;
            flight.AircraftId = updateDTO.AircraftId != Guid.Empty ? updateDTO.AircraftId : flight.AircraftId;
            flight.DepartureAirportId = updateDTO.DepartureAirportId != Guid.Empty ? updateDTO.DepartureAirportId : flight.DepartureAirportId;
            flight.ArrivalAirportId = updateDTO.ArrivalAirportId != Guid.Empty ? updateDTO.ArrivalAirportId : flight.ArrivalAirportId;

            flight.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.FlightRepository.Update(flight);
            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Flight not updated");

            return new SuccessResult("Flight updated successfully");
        }
    }
}
