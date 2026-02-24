using FlightReservation_Core.Business.Utilities.Results.Abstract;
using FlightReservation_Entities.DTOs.SeatDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Services.Abstracts
{
    public interface ISeatService
    {
        Task<IDataResult<List<SeatGetAllDto>>> GetAllSeatsAsync();
        Task<IDataResult<SeatGetDto>> GetSeatByIdAsync(Guid id);
        Task<IDataResult<List<SeatGetAllDto>>> GetAllSeatsPaginatedAsync(int page, int size);
        Task<IDataResult<List<SeatGetAllDto>>> GetAllDeletedSeatsAsync();
        Task<IResult> AddSeatAsync(SeatCreateDto createDTO);
        Task<IResult> UpdateSeatAsync(Guid id, SeatUpdateDto updateDTO);
        Task<IResult> HardDeleteSeatAsync(Guid id);
        Task<IResult> SoftDeleteSeatAsync(Guid id);
        Task<IResult> RecoverSeatAsync(Guid id);
        Task<IResult> ReserveSeatAsync(Guid seatId, Guid reservationId);
        Task<IResult> BookSeatAsync(Guid seatId, Guid reservationId);
        Task<IResult> ReleaseSeatAsync(Guid seatId);
        Task<IDataResult<List<SeatGetAllDto>>> GetSeatsByFlightAsync(Guid flightId, bool onlyAvailable = false);

    }
}
