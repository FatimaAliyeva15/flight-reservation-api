using FlightReservation_Core.Business.Utilities.Results.Abstract;
using FlightReservation_Entities.DTOs.TicketDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Services.Abstracts
{
    public interface ITicketService
    {
        Task<IDataResult<List<TicketGetAllDto>>> GetAllTicketsAsync();
        Task<IDataResult<TicketGetDto>> GetTicketByIdAsync(Guid id);
        Task<IDataResult<List<TicketGetAllDto>>> GetAllTicketsPaginatedAsync(int page, int size);
        Task<IDataResult<List<TicketGetAllDto>>> GetAllDeletedTicketsAsync();
        Task<IResult> AddTicketAsync(TicketCreateDto createDto);
        Task<IResult> UpdateTicketAsync(Guid id, TicketUpdateDto updateDto);
        Task<IResult> SoftDeleteTicketAsync(Guid id);
        Task<IResult> HardDeleteTicketAsync(Guid id);
        Task<IResult> RecoverTicketAsync(Guid id);
        Task<IResult> AssignSeatToTicketAsync(Guid ticketId, Guid seatId);
        Task<IDataResult<List<TicketGetAllDto>>> GetTicketsByReservationAsync(Guid reservationId);
        Task<IResult> CancelTicketAsync(Guid ticketId);
        Task<IDataResult<List<TicketGetAllDto>>> GetTicketsByPassengerAsync(Guid passengerId);

    }
}
