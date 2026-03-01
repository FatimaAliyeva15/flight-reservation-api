using AutoMapper;
using FlighReservation_Business.Services.Abstracts;
using FlighReservation_Business.Utilities.Constants;
using FlightReservation_Core.Business.Utilities.Exceptions;
using FlightReservation_Core.Business.Utilities.Results.Abstract;
using FlightReservation_Core.Business.Utilities.Results.Concrete;
using FlightReservation_DataAccess.UnitOfWork.Abstract;
using FlightReservation_Entities.Concretes;
using FlightReservation_Entities.DTOs.AircraftDTOs;
using FlightReservation_Entities.DTOs.AirlineDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Services.Concretes
{
    public class AirlineService : IAirlineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AirlineService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResult> AddAirlineAsync(AirlineCreateDto createDTO)
        {
            var exists = await _unitOfWork.AirlineRepository.GetAsync(a =>
            a.Name.ToLower() == createDTO.Name.ToLower());

            if (exists != null)
                return new ErrorResult("This name already exists");

            var airline = _mapper.Map<Airline>(createDTO);
            airline.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.AirlineRepository.AddAsync(airline);
            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
            {
                return new ErrorResult("Airline not added");
            }

            return new SuccessResult("Airline added");
        }

        public async Task<IDataResult<AirlineGetDto>> GetAirlineByIdAsync(Guid id)
        {
            var existsAirline = await _unitOfWork.AirlineRepository.GetAsync(a => a.Id == id);
            if (existsAirline == null)
                return new ErrorDataResult<AirlineGetDto>(_mapper.Map<AirlineGetDto>(existsAirline), "Airline not founded");

            var dto = _mapper.Map<AirlineGetDto>(existsAirline);

            return new SuccessDataResult<AirlineGetDto>(dto, "Airline founded");
        }

        public async Task<IDataResult<List<AirlineGetAllDto>>> GetAllAirlinesPaginatedAsync(int page, int size)
        {
            if (page <= 0 || size <= 0)
                return new ErrorDataResult<List<AirlineGetAllDto>>(
                    new List<AirlineGetAllDto>(),
                    "Page or size invalid");

            var airlines = await _unitOfWork.AirlineRepository
                .GetAllPaginatedAsync(page, size, null);

            if (airlines.Count == 0)
                return new ErrorDataResult<List<AirlineGetAllDto>>(
                    new List<AirlineGetAllDto>(),
                    "No airline found");

            var dtos = _mapper.Map<List<AirlineGetAllDto>>(airlines);

            return new SuccessDataResult<List<AirlineGetAllDto>>(
                dtos,
                "Airlines listed with pagination");
        }

        public async Task<IDataResult<List<AirlineGetAllDto>>> GetAllAirlinesAsync()
        {
            var airlines = await _unitOfWork.AirlineRepository.GetAllAsync();
            if (airlines.Count == 0)
                return new ErrorDataResult<List<AirlineGetAllDto>>(new List<AirlineGetAllDto>(), "Airline not founded");

            var dtos = _mapper.Map<List<AirlineGetAllDto>>(airlines);
            return new SuccessDataResult<List<AirlineGetAllDto>>(dtos, "Airlines founded");
        }

        public async Task<IDataResult<List<AirlineGetAllDto>>> GetAllDeletedAirlinesAsync()
        {
            var deletedAirlines = await _unitOfWork.AirlineRepository.GetDeletedAsync();

            if (deletedAirlines == null || deletedAirlines.Count == 0)
                return new ErrorDataResult<List<AirlineGetAllDto>>(new List<AirlineGetAllDto>(), "Deleted airlines not found");

            var dtos = _mapper.Map<List<AirlineGetAllDto>>(deletedAirlines);
            return new SuccessDataResult<List<AirlineGetAllDto>>(dtos, "Deleted airlines retrieved successfully");
        }

        public async Task<IResult> HardDeleteAirlineAsync(Guid id)
        {
            var existsAirline = await _unitOfWork.AirlineRepository.GetAsync(a => a.Id == id, includeDeleted: true);
            if (existsAirline == null)
                throw new NotFoundException(ExceptionMessage.AirlineNotFound);

            await _unitOfWork.AirlineRepository.HardDeleteAsync(existsAirline);
            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
            {
                return new ErrorResult("Airline not deleted");
            }

            return new SuccessResult("Airline deleted");
        }

        public async Task<IResult> RecoverAirlineAsync(Guid id)
        {
            var airline = await _unitOfWork.AirlineRepository.GetAsync(a => a.Id == id, includeDeleted: true);

            if (airline == null)
                throw new NotFoundException(ExceptionMessage.AirlineNotFound);

            await _unitOfWork.AirlineRepository
                .RecoverAsync(airline);

            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Airline not recovered");

            return new SuccessResult("Airline recovered");
        }

        public async Task<IResult> SoftDeleteAirclineAsync(Guid id)
        {
            var existsAirline = await _unitOfWork.AirlineRepository.GetAsync(a => a.Id == id);

            if (existsAirline == null)
                throw new NotFoundException(ExceptionMessage.AirlineNotFound);

            await _unitOfWork.AirlineRepository.SoftDeleteAsync(existsAirline);

            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Airline not softdeleted");

            return new SuccessResult("Airline softdeleted");
        }

        public async Task<IResult> UpdateAirlineAsync(Guid id, AirlineUpdateDto updateDTO)
        {
            var existsAirline = await _unitOfWork.AirlineRepository.GetAsync(a => a.Id == id);
            if (existsAirline == null)
                throw new NotFoundException(ExceptionMessage.AirlineNotFound);

            existsAirline.Name = updateDTO.Name ?? existsAirline.Name;
            existsAirline.Code = updateDTO.Code ?? existsAirline.Code;
            existsAirline.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.AirlineRepository.Update(existsAirline);
            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Airline not updated");

            return new SuccessResult("Airline updated");
        }
    }
}

