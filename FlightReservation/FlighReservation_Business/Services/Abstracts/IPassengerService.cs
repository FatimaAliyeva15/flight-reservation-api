using FlightReservation_Core.Business.Utilities.Results.Abstract;
using FlightReservation_Entities.DTOs.PassengerDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Services.Abstracts
{
    public interface IPassengerService
    {
        Task<IDataResult<List<PassengerGetAllDto>>> GetAllPassengersAsync();
        Task<IDataResult<PassengerGetDto>> GetPassengerByIdAsync(Guid id);
        Task<IDataResult<List<PassengerGetAllDto>>> GetAllPassengersPaginatedAsync(int page, int size);
        Task<IDataResult<List<PassengerGetAllDto>>> GetAllDeletedPassengersAsync();
        Task<IResult> AddPassengerAsync(PassengerCreateDto createDTO);
        Task<IResult> UpdatePassengerAsync(Guid id, PassengerUpdateDto updateDTO);
        Task<IResult> HardDeletePassengerAsync(Guid id);
        Task<IResult> SoftDeletePassengerAsync(Guid id);
        Task<IResult> RecoverPassengerAsync(Guid id);
    }
}
