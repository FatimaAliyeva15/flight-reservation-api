using FlightReservation_Core.Business.Utilities.Results.Abstract;
using FlightReservation_Entities.DTOs.AirportDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Services.Abstracts
{
    public interface IAirportService
    {
        Task<IDataResult<List<AirportGetAllDto>>> GetAllAirportsAsync();
        Task<IDataResult<AirportGetDto>> GetAirportByIdAsync(Guid id);
        Task<IDataResult<List<AirportGetAllDto>>> GetAllAirportsPaginatedAsync(int page, int size);
        Task<IDataResult<List<AirportGetAllDto>>> GetAllDeletedAirportsAsync();
        Task<IResult> AddAirportAsync(AirportCreateDto createDTO);
        Task<IResult> UpdateAirportAsync(Guid id, AirportUpdateDto updateDTO);
        Task<IResult> HardDeleteAirportAsync(Guid id);
        Task<IResult> SoftDeleteAirportAsync(Guid id);
        Task<IResult> RecoverAirportAsync(Guid id);
    }
}
