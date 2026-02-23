using AutoMapper;
using FlighReservation_Business.Services.Abstracts;
using FlighReservation_Business.Utilities.Constants;
using FlightReservation_Core.Business.Utilities.Exceptions;
using FlightReservation_Core.Business.Utilities.Results.Abstract;
using FlightReservation_Core.Business.Utilities.Results.Concrete;
using FlightReservation_DataAccess.UnitOfWork.Abstract;
using FlightReservation_Entities.Concretes;
using FlightReservation_Entities.DTOs.AircraftDTOs;
using FlightReservation_Entities.DTOs.AirportDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Services.Concretes
{ 

    public class AirportService : IAirportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AirportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<IResult> AddAirportAsync(AirportCreateDto createDTO)
        {
            var exists = await _unitOfWork.AirportRepository.GetAsync(a =>
            a.Name.ToLower() == createDTO.Name.ToLower());

            if (exists != null)
                return new ErrorResult("This airport already exists");

            var airport = _mapper.Map<Airport>(createDTO);
            airport.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.AirportRepository.AddAsync(airport);
            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
            {
                return new ErrorResult("Airport not added");
            }

            return new SuccessResult("Airport added");
        }

        public async Task<IDataResult<AirportGetDto>> GetAirportByIdAsync(Guid id)
        {
            var existsAirport = await _unitOfWork.AirportRepository.GetAsync(a => a.Id == id);
            if (existsAirport == null)
                return new ErrorDataResult<AirportGetDto>(_mapper.Map<AirportGetDto>(existsAirport), "Airport not founded");

            var dto = _mapper.Map<AirportGetDto>(existsAirport);

            return new SuccessDataResult<AirportGetDto>(dto, "Airport founded");
        }

        public async Task<IDataResult<List<AirportGetAllDto>>> GetAllAirportsAsync()
        {
            var airports = await _unitOfWork.AirportRepository.GetAllAsync();
            if (airports.Count == 0)
                return new ErrorDataResult<List<AirportGetAllDto>>(new List<AirportGetAllDto>(), "Airport not founded");

            var dtos = _mapper.Map<List<AirportGetAllDto>>(airports);
            return new SuccessDataResult<List<AirportGetAllDto>>(dtos, "Airports founded");
        }

        public async Task<IDataResult<List<AirportGetAllDto>>> GetAllAirportsPaginatedAsync(int page, int size)
        {
            if (page <= 0 || size <= 0)
                return new ErrorDataResult<List<AirportGetAllDto>>(
                    new List<AirportGetAllDto>(),
                    "Page or size invalid");

            var airports = await _unitOfWork.AirportRepository
                .GetAllPaginatedAsync(page, size, null);

            if (airports.Count == 0)
                return new ErrorDataResult<List<AirportGetAllDto>>(
                    new List<AirportGetAllDto>(),
                    "No airport found");

            var dtos = _mapper.Map<List<AirportGetAllDto>>(airports);

            return new SuccessDataResult<List<AirportGetAllDto>>(
                dtos,
                "Airports listed with pagination");
        }

        public async Task<IDataResult<List<AirportGetAllDto>>> GetAllDeletedAirportsAsync()
        {
            var deletedAirports = await _unitOfWork.AirportRepository.GetDeletedAsync();

            if (deletedAirports == null || deletedAirports.Count == 0)
                return new ErrorDataResult<List<AirportGetAllDto>>(new List<AirportGetAllDto>(), "Deleted airports not found");

            var dtos = _mapper.Map<List<AirportGetAllDto>>(deletedAirports);
            return new SuccessDataResult<List<AirportGetAllDto>>(dtos, "Deleted airports retrieved successfully");
        }

        public async Task<IResult> HardDeleteAirportAsync(Guid id)
        {
            var existsAirport = await _unitOfWork.AirportRepository.GetAsync(a => a.Id == id);
            if (existsAirport == null)
                throw new NotFoundException(ExceptionMessage.AirportNotFound);

            await _unitOfWork.AirportRepository.HardDeleteAsync(existsAirport);
            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
            {
                return new ErrorResult("Airport not deleted");
            }

            return new SuccessResult("Airport deleted");
        }

        public async Task<IResult> RecoverAirportAsync(Guid id)
        {
            var airport = await _unitOfWork.AirportRepository
                .GetAsync(a => a.Id == id);

            if (airport == null)
                throw new NotFoundException(ExceptionMessage.AirportNotFound);

            await _unitOfWork.AirportRepository
                .RecoverAsync(airport);

            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Airport not recovered");

            return new SuccessResult("Airport recovered");
        }

        public async Task<IResult> SoftDeleteAirportAsync(Guid id)
        {
            var existsAirport = await _unitOfWork.AirportRepository.GetAsync(a => a.Id == id);

            if (existsAirport == null)
                throw new NotFoundException(ExceptionMessage.AirportNotFound);

            await _unitOfWork.AirportRepository.SoftDeleteAsync(existsAirport);

            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Airport not deleted");

            return new SuccessResult("Airport softdeleted");
        }

        public async Task<IResult> UpdateAirportAsync(Guid id, AirportUpdateDto updateDTO)
        {
            var existsAirport = await _unitOfWork.AirportRepository.GetAsync(a => a.Id == id);
            if (existsAirport == null)
                throw new NotFoundException(ExceptionMessage.AirportNotFound);

            existsAirport.Name = updateDTO.Name ?? existsAirport.Name;
            existsAirport.City = updateDTO.City ?? existsAirport.City;
            existsAirport.Country = updateDTO.Country ?? existsAirport.Country;
            existsAirport.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.AirportRepository.Update(existsAirport);
            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Airport not updated");

            return new SuccessResult("Airport updated");
        }
    }
}
