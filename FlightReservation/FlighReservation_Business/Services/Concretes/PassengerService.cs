using AutoMapper;
using FlighReservation_Business.Services.Abstracts;
using FlighReservation_Business.Utilities.Constants;
using FlightReservation_Core.Business.Utilities.Exceptions;
using FlightReservation_Core.Business.Utilities.Results.Abstract;
using FlightReservation_Core.Business.Utilities.Results.Concrete;
using FlightReservation_DataAccess.UnitOfWork.Abstract;
using FlightReservation_Entities.Concretes;
using FlightReservation_Entities.DTOs.AirlineDTOs;
using FlightReservation_Entities.DTOs.PassengerDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Services.Concretes
{
    public class PassengerService : IPassengerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PassengerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<IResult> AddPassengerAsync(PassengerCreateDto createDTO)
        {
            var exists = await _unitOfWork.PassengerRepository.GetAsync(p =>
            p.PassportNumber.ToLower() == createDTO.PassportNumber.ToLower());

            if (exists != null)
                return new ErrorResult("This passport number already exists");

            var passenger = _mapper.Map<Passenger>(createDTO);
            passenger.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.PassengerRepository.AddAsync(passenger);
            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
            {
                return new ErrorResult("Passenger not added");
            }

            return new SuccessResult("Passenger added");
        }

        public async Task<IDataResult<List<PassengerGetAllDto>>> GetAllDeletedPassengersAsync()
        {
            var deletedPassengers = await _unitOfWork.PassengerRepository.GetDeletedAsync();

            if (deletedPassengers == null || deletedPassengers.Count == 0)
                return new ErrorDataResult<List<PassengerGetAllDto>>(new List<PassengerGetAllDto>(), "Deleted passengers not found");

            var dtos = _mapper.Map<List<PassengerGetAllDto>>(deletedPassengers);
            return new SuccessDataResult<List<PassengerGetAllDto>>(dtos, "Deleted passengers retrieved successfully");
        }

        public async Task<IDataResult<List<PassengerGetAllDto>>> GetAllPassengersAsync()
        {
            var passengers = await _unitOfWork.PassengerRepository.GetAllAsync();
            if (passengers.Count == 0)
                return new ErrorDataResult<List<PassengerGetAllDto>>(new List<PassengerGetAllDto>(), "Passenger not founded");

            var dtos = _mapper.Map<List<PassengerGetAllDto>>(passengers);
            return new SuccessDataResult<List<PassengerGetAllDto>>(dtos, "Passengers founded");
        }

        public async Task<IDataResult<List<PassengerGetAllDto>>> GetAllPassengersPaginatedAsync(int page, int size)
        {
            if (page <= 0 || size <= 0)
                return new ErrorDataResult<List<PassengerGetAllDto>>(
                    new List<PassengerGetAllDto>(),
                    "Page or size invalid");

            var passengers = await _unitOfWork.PassengerRepository
                .GetAllPaginatedAsync(page, size, null);

            if (passengers.Count == 0)
                return new ErrorDataResult<List<PassengerGetAllDto>>(
                    new List<PassengerGetAllDto>(),
                    "No passenger found");

            var dtos = _mapper.Map<List<PassengerGetAllDto>>(passengers);

            return new SuccessDataResult<List<PassengerGetAllDto>>(
                dtos,
                "Passengers listed with pagination");
        }

        public async Task<IDataResult<PassengerGetDto>> GetPassengerByIdAsync(Guid id)
        {
            var existsPassenger = await _unitOfWork.PassengerRepository.GetAsync(a => a.Id == id);
            if (existsPassenger == null)
                return new ErrorDataResult<PassengerGetDto>(_mapper.Map<PassengerGetDto>(existsPassenger), "Passenger not founded");

            var dto = _mapper.Map<PassengerGetDto>(existsPassenger);

            return new SuccessDataResult<PassengerGetDto>(dto, "Passenger founded");
        }

        public async Task<IResult> HardDeletePassengerAsync(Guid id)
        {
            var existsPassenger = await _unitOfWork.PassengerRepository.GetAsync(a => a.Id == id, includeDeleted: true);
            if (existsPassenger == null)
                throw new NotFoundException(ExceptionMessage.PassengerNotFound);

            await _unitOfWork.PassengerRepository.HardDeleteAsync(existsPassenger);
            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
            {
                return new ErrorResult("Passenger not deleted");
            }

            return new SuccessResult("Passenger deleted");
        }

        public async Task<IResult> RecoverPassengerAsync(Guid id)
        {
            var passenger = await _unitOfWork.PassengerRepository
                .GetAsync(a => a.Id == id, includeDeleted: true);

            if (passenger == null)
                throw new NotFoundException(ExceptionMessage.PassengerNotFound);

            await _unitOfWork.PassengerRepository
                .RecoverAsync(passenger);

            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Passenger not recovered");

            return new SuccessResult("Passenger recovered");
        }

        public async Task<IResult> SoftDeletePassengerAsync(Guid id)
        {
            var existsPassenger = await _unitOfWork.PassengerRepository.GetAsync(a => a.Id == id);

            if (existsPassenger == null)
                throw new NotFoundException(ExceptionMessage.PassengerNotFound);

            await _unitOfWork.PassengerRepository.SoftDeleteAsync(existsPassenger);

            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Passenger not softdeleted");

            return new SuccessResult("Passenger softdeleted");
        }

        public async Task<IResult> UpdatePassengerAsync(Guid id, PassengerUpdateDto updateDTO)
        {
            var existsPassenger = await _unitOfWork.PassengerRepository.GetAsync(a => a.Id == id);
            if (existsPassenger == null)
                throw new NotFoundException(ExceptionMessage.PassengerNotFound);

            existsPassenger.FirstName = updateDTO.FirstName ?? existsPassenger.FirstName;
            existsPassenger.LastName = updateDTO.LastName ?? existsPassenger.LastName;
            existsPassenger.PassportNumber = updateDTO.PassportNumber ?? existsPassenger.PassportNumber;
            existsPassenger.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.PassengerRepository.Update(existsPassenger);
            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Passenger not updated");

            return new SuccessResult("Passenger updated");
        }
    }
}
