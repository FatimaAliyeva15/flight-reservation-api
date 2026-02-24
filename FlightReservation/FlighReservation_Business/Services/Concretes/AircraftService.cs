using AutoMapper;
using FlighReservation_Business.Services.Abstracts;
using FlighReservation_Business.Utilities.Constants;
using FlightReservation_Core.Business.Utilities.Exceptions;
using FlightReservation_Core.Business.Utilities.Results.Abstract;
using FlightReservation_Core.Business.Utilities.Results.Concrete;
using FlightReservation_DataAccess.UnitOfWork.Abstract;
using FlightReservation_Entities.Concretes;
using FlightReservation_Entities.DTOs.AircraftDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Services.Concretes
{
    public class AircraftService : IAircraftService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AircraftService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResult> AddAircraftAsync(AircraftCreateDto createDTO)
        {
            var exists = await _unitOfWork.AircraftRepository.GetAsync(a =>
            a.Model.ToLower() == createDTO.Model.ToLower() &&
            a.AirlineId == createDTO.AirlineId);

            if (exists != null)
                return new ErrorResult("This model already exists");

            var aircraft = _mapper.Map<Aircraft>(createDTO);
            aircraft.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.AircraftRepository.AddAsync(aircraft);
            var result = await _unitOfWork.SaveAsync(); 

            if(result == 0)
            {
                return new ErrorResult("Aircraft not added");
            }

            return new SuccessResult("Aircraft added");


        }

        public async Task<IResult> HardDeleteAircraftAsync(Guid id)
        {
            var existsAircraft = await _unitOfWork.AircraftRepository.GetAsync(a => a.Id == id);
            if (existsAircraft == null)
                throw new NotFoundException(ExceptionMessage.AircraftNotFound);

            await _unitOfWork.AircraftRepository.HardDeleteAsync(existsAircraft);
            var result = await _unitOfWork.SaveAsync();

            if(result == 0)
            {
                return new ErrorResult("Aircraft not deleted");
            }

            return new SuccessResult("Aircraft deleted");
        }

        public async Task<IDataResult<AircraftGetDto>> GetAircraftByIdAsync(Guid id)
        {
            var existsAircraft = await _unitOfWork.AircraftRepository.GetAsync(a => a.Id == id, includeDeleted: false, "Airline");
            if (existsAircraft == null)
                return new ErrorDataResult<AircraftGetDto>(_mapper.Map<AircraftGetDto>(existsAircraft), "Aircraft not founded");

            var dto = _mapper.Map<AircraftGetDto>(existsAircraft);

            return new SuccessDataResult<AircraftGetDto>(dto, "Aircraft founded");
        }

        public async Task<IDataResult<List<AircraftGetAllDto>>> GetAllAircraftsAsync()
        {
            var aircrafts = await _unitOfWork.AircraftRepository.GetAllAsync();
            if (aircrafts.Count == 0)
                return new ErrorDataResult<List<AircraftGetAllDto>>(new List<AircraftGetAllDto>(), "Aircraft not founded");

            var dtos = _mapper.Map<List<AircraftGetAllDto>>(aircrafts);
            return new SuccessDataResult<List<AircraftGetAllDto>>(dtos, "Aircrafts founded");
        }

        public async Task<IResult> RecoverAircraftAsync(Guid id)
        {

            var aircraft = await _unitOfWork.AircraftRepository
                .GetAsync(a => a.Id == id);

            if (aircraft == null)
                throw new NotFoundException(ExceptionMessage.AircraftNotFound);

            await _unitOfWork.AircraftRepository
                .RecoverAsync(aircraft);

            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Aircraft not recovered");

            return new SuccessResult("Aircraft recovered");
        }

        public async Task<IResult> SoftDeleteAircraftAsync(Guid id)
        {
            var existsAircraft = await _unitOfWork.AircraftRepository.GetAsync(a => a.Id == id);

            if (existsAircraft == null)
                throw new NotFoundException(ExceptionMessage.AircraftNotFound);

            await _unitOfWork.AircraftRepository.SoftDeleteAsync(existsAircraft);

            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Aircraft not deleted");

            return new SuccessResult("Aircraft softdeleted");
        }

        public async Task<IResult> UpdateAircraftAsync(Guid id, AircraftUpdateDto updateDTO)
        {
            var existsAircraft = await _unitOfWork.AircraftRepository.GetAsync(a => a.Id == id);
            if (existsAircraft == null)
                throw new NotFoundException(ExceptionMessage.AircraftNotFound);

            existsAircraft.Model = updateDTO.Model ?? existsAircraft.Model;
            existsAircraft.Capacity = updateDTO.Capacity != 0 ? updateDTO.Capacity : existsAircraft.Capacity;
            existsAircraft.AirlineId = updateDTO.AirlineId != Guid.Empty ? updateDTO.AirlineId : existsAircraft.AirlineId;
            existsAircraft.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.AircraftRepository.Update(existsAircraft);
            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Aircraft not softdeleted");

            return new SuccessResult("Aircraft updated");
        }

        public async Task<IDataResult<List<AircraftGetAllDto>>> GetAllAircraftsPaginatedAsync(int page, int size)
        {
            if (page <= 0 || size <= 0)
                return new ErrorDataResult<List<AircraftGetAllDto>>(
                    new List<AircraftGetAllDto>(),
                    "Page or size invalid");

            var aircrafts = await _unitOfWork.AircraftRepository
                .GetAllPaginatedAsync(page, size, null, "Airline");

            if (aircrafts.Count == 0)
                return new ErrorDataResult<List<AircraftGetAllDto>>(
                    new List<AircraftGetAllDto>(),
                    "No aircraft found");

            var dtos = _mapper.Map<List<AircraftGetAllDto>>(aircrafts);

            return new SuccessDataResult<List<AircraftGetAllDto>>(
                dtos,
                "Aircrafts listed with pagination");
        }

        public async Task<IDataResult<List<AircraftGetAllDto>>> GetAllDeletedAircraftsAsync()
        {
            var deletedAircrafts = await _unitOfWork.AircraftRepository.GetDeletedAsync();

            if (deletedAircrafts == null || deletedAircrafts.Count == 0)
                return new ErrorDataResult<List<AircraftGetAllDto>>(new List<AircraftGetAllDto>(), "Deleted aircrafts not found");

            var dtos = _mapper.Map<List<AircraftGetAllDto>>(deletedAircrafts);
            return new SuccessDataResult<List<AircraftGetAllDto>>(dtos, "Deleted aircrafts retrieved successfully");
        }
    }
}
