using FlightReservation_Core.Business.Utilities.Results.Abstract;
using FlightReservation_Entities.DTOs.FlightDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Services.Abstracts
{
    public interface IFlightService
    {
        Task<IDataResult<List<FlightGetAllDto>>> GetAllFlightsAsync();

        Task<IDataResult<FlightGetDto>> GetFlightByIdAsync(Guid id);

        Task<IDataResult<List<FlightGetAllDto>>> GetAllFlightsPaginatedAsync(int page, int size);

        Task<IDataResult<List<FlightGetAllDto>>> GetAllDeletedFlightsAsync();

        Task<IResult> AddFlightAsync(FlightCreateDto createDto);

        Task<IResult> UpdateFlightAsync(Guid id, FlightUpdateDto updateDto);

        Task<IResult> ApproveFlightAsync(Guid id);

        Task<IResult> RejectFlightAsync(Guid id, string adminComment);

        Task<IResult> CancelFlightAsync(Guid id);

        Task<IResult> SoftDeleteFlightAsync(Guid id);

        Task<IResult> RecoverFlightAsync(Guid id);

        Task<IResult> HardDeleteFlightAsync(Guid id);
    }
}
