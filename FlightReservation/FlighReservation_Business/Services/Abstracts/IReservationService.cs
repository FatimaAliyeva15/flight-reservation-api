using FlightReservation_Core.Business.Utilities.Results.Abstract;
using FlightReservation_Entities.DTOs.ReservationDTOs;
using FlightReservation_Entities.DTOs.TicketDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Services.Abstracts
{
    public interface IReservationService
    {
        Task<IDataResult<List<ReservationGetAllDto>>> GetAllReservationsAsync();
        Task<IDataResult<ReservationGetDto>> GetReservationByIdAsync(Guid id);
        Task<IDataResult<List<ReservationGetAllDto>>> GetAllReservationsPaginatedAsync(int page, int size);
        Task<IDataResult<List<ReservationGetAllDto>>> GetAllDeletedReservationsAsync();
        Task<IResult> AddReservationAsync(ReservationCreateDto createDto, string userId);
        Task<IResult> UpdateReservationAsync(Guid id, ReservationUpdateDto updateDto);
        Task<IResult> SoftDeleteReservationAsync(Guid id);
        Task<IResult> HardDeleteReservationAsync(Guid id);
        Task<IResult> RecoverReservationAsync(Guid id);
        Task<IResult> CreateReservationWithTicketsAsync(ReservationCreateDto createDto, List<ReservationCreateWithTicketDto> ticketsDto, string userId);
        Task<IDataResult<List<ReservationGetAllDto>>> GetReservationsByPassengerAsync(string userId);
        Task<IResult> CancelReservationAsync(Guid reservationId);
        Task<IResult> ConfirmReservationAfterPaymentAsync(Guid reservationId);
       // Task<IDataResult<ReservationWithTicketsDto>> GetReservationWithTicketsAsync(Guid reservationId);

    }
}
