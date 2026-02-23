using FlightReservation_Core.Business.Utilities.Results.Abstract;
using FlightReservation_Entities.DTOs.AircraftDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Services.Abstracts
{
    public interface IAircraftService
    {
        Task<IDataResult<List<AircraftGetAllDto>>> GetAllAircraftsAsync();
        Task<IDataResult<AircraftGetDto>> GetAircraftByIdAsync(Guid id);
        Task<IDataResult<List<AircraftGetAllDto>>> GetAllAircraftsPaginatedAsync(int page, int size);
        Task<IDataResult<List<AircraftGetAllDto>>> GetAllDeletedAircraftsAsync();
        Task<IResult> AddAircraftAsync(AircraftCreateDto createDTO);
        Task<IResult> UpdateAircraftAsync(Guid id, AircraftUpdateDto updateDTO);
        Task<IResult> HardDeleteAircraftAsync(Guid id);
        Task<IResult> SoftDeleteAircraftAsync(Guid id);
        Task<IResult> RecoverAircraftAsync(Guid id);
    }
}
