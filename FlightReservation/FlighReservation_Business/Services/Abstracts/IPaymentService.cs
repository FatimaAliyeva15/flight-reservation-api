using FlightReservation_Core.Business.Utilities.Results.Abstract;
using FlightReservation_Entities.DTOs.PaymentDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Services.Abstracts
{
    public interface IPaymentService
    {
        Task<IDataResult<List<PaymentGetAllDto>>> GetAllPaymentsAsync();
        Task<IDataResult<PaymentGetDto>> GetPaymentByIdAsync(Guid id);
        Task<IDataResult<List<PaymentGetAllDto>>> GetAllPaymentsPaginatedAsync(int page, int size);
        Task<IDataResult<List<PaymentGetAllDto>>> GetAllDeletedPaymentsAsync();
        Task<IResult> AddPaymentAsync(PaymentCreateDto createDto);
        Task<IResult> UpdatePaymentAsync(Guid id, PaymentUpdateDto updateDto);
        Task<IResult> SoftDeletePaymentAsync(Guid id);
        Task<IResult> HardDeletePaymentAsync(Guid id);
        Task<IResult> RecoverPaymentAsync(Guid id);
        Task<IResult> ConfirmPaymentAsync(Guid paymentId); 
        Task<IResult> RefundPaymentAsync(Guid paymentId);  
        Task<IDataResult<List<PaymentGetAllDto>>> GetPaymentsByReservationAsync(Guid reservationId);
        Task<IDataResult<List<PaymentGetAllDto>>> GetPaymentsByPassengerAsync(string appUserId);
    }

}

