using FlightReservation_Core.Business.Utilities.Results.Abstract;
using FlightReservation_Entities.DTOs.AirlineDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Services.Abstracts
{
    public interface IAirlineService
    {
        Task<IDataResult<List<AirlineGetAllDto>>> GetAllAirlinesAsync();
        Task<IDataResult<AirlineGetDto>> GetAirlineByIdAsync(Guid id);
        Task<IDataResult<List<AirlineGetAllDto>>> GetAllAirlinesPaginatedAsync(int page, int size);
        Task<IDataResult<List<AirlineGetAllDto>>> GetAllDeletedAirlinesAsync();
        Task<IResult> AddAirlineAsync(AirlineCreateDto createDTO);
        Task<IResult> UpdateAirlineAsync(Guid id, AirlineUpdateDto updateDTO);
        Task<IResult> HardDeleteAirlineAsync(Guid id);
        Task<IResult> SoftDeleteAirclineAsync(Guid id);
        Task<IResult> RecoverAirlineAsync(Guid id);
    }
}
