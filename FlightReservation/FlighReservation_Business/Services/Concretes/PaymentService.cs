using AutoMapper;
using FlighReservation_Business.Services.Abstracts;
using FlighReservation_Business.Utilities.Constants;
using FlightReservation_Core.Business.Utilities.Exceptions;
using FlightReservation_Core.Business.Utilities.Results.Abstract;
using FlightReservation_Core.Business.Utilities.Results.Concrete;
using FlightReservation_DataAccess.UnitOfWork.Abstract;
using FlightReservation_Entities.Concretes;
using FlightReservation_Entities.DTOs.AircraftDTOs;
using FlightReservation_Entities.DTOs.PaymentDTOs;
using FlightReservation_Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Services.Concretes
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResult> AddPaymentAsync(PaymentCreateDto createDto)
        {
            var reservation = await _unitOfWork.ReservationRepository.GetAsync(r => r.Id == createDto.ReservationId, includeDeleted: false, "Tickets");
            if (reservation == null)
                throw new NotFoundException(ExceptionMessage.ReservationNotFound);

            if(reservation.Tickets == null || reservation.Tickets.Count == 0)
                return new ErrorResult("Cannot make payment: Reservation has no tickets.");

            if (reservation.IsPaid)
                return new ErrorResult("Reservation already paid.");

            if (reservation.Status != ReservationStatus.PendingPayment)
                return new ErrorResult($"Payment cannot be made when reservation status is {reservation.Status}");

            if (createDto.Amount != reservation.TotalPrice)
                return new ErrorResult($"Payment amount must be exactly {reservation.TotalPrice}");

            bool paymentSuccess = true;
            if (!paymentSuccess)
                return new ErrorResult("Payment failed.");


            var payment = new Payment
            {
                ReservationId = createDto.ReservationId,
                AppUserId = reservation.AppUserId,
                Amount = createDto.Amount,
                Status = PaymentStatus.Pending,
                PaidAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.PaymentRepository.AddAsync(payment);

            var result = await _unitOfWork.SaveAsync();

            return result > 0 ? new SuccessResult("Payment created") : new ErrorResult("Payment creation failed");

        }

        public async Task<IResult> ConfirmPaymentAsync(Guid paymentId)
        {
            var payment = await _unitOfWork.PaymentRepository.GetAsync(p => p.Id == paymentId);

            if (payment == null)
                throw new NotFoundException(ExceptionMessage.PaymentNotFound);

            if (payment.Status == PaymentStatus.Completed)
                return new ErrorResult("Payment already completed");

            
            var reservation = await _unitOfWork.ReservationRepository.GetAsync(r => r.Id == payment.ReservationId, false, "Tickets");

            if (reservation == null)
                throw new NotFoundException(ExceptionMessage.ReservationNotFound);

            
            if (reservation.Tickets == null || reservation.Tickets.Count == 0)
                return new ErrorResult("Reservation has no tickets");

            payment.Status = PaymentStatus.Completed;
            payment.PaidAt = DateTime.UtcNow;

            
            reservation.Status = ReservationStatus.Confirmed;
            reservation.IsPaid = true;
            reservation.PaidAt = DateTime.UtcNow;

            await _unitOfWork.PaymentRepository.Update(payment);
            await _unitOfWork.ReservationRepository.Update(reservation);

            var result = await _unitOfWork.SaveAsync();

            return result > 0
                ? new SuccessResult("Payment confirmed and reservation completed")
                : new ErrorResult("Payment confirmation failed");
        }

        public async Task<IDataResult<List<PaymentGetAllDto>>> GetAllDeletedPaymentsAsync()
        {
            var deletedPayments = await _unitOfWork.PaymentRepository.GetDeletedAsync();

            if (deletedPayments == null || deletedPayments.Count == 0)
                return new ErrorDataResult<List<PaymentGetAllDto>>(new List<PaymentGetAllDto>(), "Deleted payments not found");

            var dtos = _mapper.Map<List<PaymentGetAllDto>>(deletedPayments);
            return new SuccessDataResult<List<PaymentGetAllDto>>(dtos, "Deleted payments retrieved successfully");
        }

        public async Task<IDataResult<List<PaymentGetAllDto>>> GetAllPaymentsAsync()
        {
            var payments = await _unitOfWork.PaymentRepository.GetAllAsync(p => !p.IsDeleted, "Reservation", "AppUser");
            if (payments.Count == 0)
                return new ErrorDataResult<List<PaymentGetAllDto>>(new List<PaymentGetAllDto>(), "Payment not founded");

            var dtos = _mapper.Map<List<PaymentGetAllDto>>(payments);
            return new SuccessDataResult<List<PaymentGetAllDto>>(dtos, "Payments founded");
        }

        public async Task<IDataResult<List<PaymentGetAllDto>>> GetAllPaymentsPaginatedAsync(int page, int size)
        {
            if (page <= 0 || size <= 0)
                return new ErrorDataResult<List<PaymentGetAllDto>>(
                    new List<PaymentGetAllDto>(),
                    "Page or size invalid");

            var payments = await _unitOfWork.PaymentRepository.GetAllPaginatedAsync(page, size, null, "Reservation", "AppUser");

            if (payments.Count == 0)
                return new ErrorDataResult<List<PaymentGetAllDto>>(
                    new List<PaymentGetAllDto>(),
                    "No payment found");

            var dtos = _mapper.Map<List<PaymentGetAllDto>>(payments);

            return new SuccessDataResult<List<PaymentGetAllDto>>(
                dtos,
                "payments listed with pagination");
        }

        public async Task<IDataResult<PaymentGetDto>> GetPaymentByIdAsync(Guid id)
        {
            var existsPayment = await _unitOfWork.PaymentRepository.GetAsync(a => a.Id == id, includeDeleted: false, "Reservation", "AppUser");
            if (existsPayment == null)
                return new ErrorDataResult<PaymentGetDto>(_mapper.Map<PaymentGetDto>(existsPayment), "Payment not founded");

            var dto = _mapper.Map<PaymentGetDto>(existsPayment);

            return new SuccessDataResult<PaymentGetDto>(dto, "Payment founded");
        }

        public async Task<IDataResult<List<PaymentGetAllDto>>> GetPaymentsByPassengerAsync(string appUserId)
        {
            var payments = await _unitOfWork.PaymentRepository.GetAllAsync(p => p.Reservation.AppUserId == appUserId && !p.IsDeleted, "Reservation","AppUser");
            var dtos = _mapper.Map<List<PaymentGetAllDto>>(payments);

            return new SuccessDataResult<List<PaymentGetAllDto>>(dtos);

        }

        public async Task<IDataResult<List<PaymentGetAllDto>>> GetPaymentsByReservationAsync(Guid reservationId)
        {
            var payments = await _unitOfWork.PaymentRepository.GetAllAsync(p => p.ReservationId == reservationId && !p.IsDeleted,  "Reservation", "AppUser");

            if (payments.Count == 0)
                return new ErrorDataResult<List<PaymentGetAllDto>>(new List<PaymentGetAllDto>(),"No payment found");

            var dtos = _mapper.Map<List<PaymentGetAllDto>>(payments);

            return new SuccessDataResult<List<PaymentGetAllDto>>(dtos);

        }

        public async Task<IResult> HardDeletePaymentAsync(Guid id)
        {
            var payment = await _unitOfWork.PaymentRepository.GetAsync(p => p.Id == id, includeDeleted: true);
            if (payment == null) throw new NotFoundException(ExceptionMessage.PaymentNotFound);

            await _unitOfWork.PaymentRepository.HardDeleteAsync(payment);

            var result = await _unitOfWork.SaveAsync();
            return result > 0 ? new SuccessResult("Payment permanently deleted") : new ErrorResult("Hard delete failed");
        }

        public async Task<IResult> RecoverPaymentAsync(Guid id)
        {
            var payment = await _unitOfWork.PaymentRepository.GetAsync(p => p.Id == id, includeDeleted: true);
            if (payment == null) throw new NotFoundException(ExceptionMessage.PaymentNotFound);

            await _unitOfWork.PaymentRepository.RecoverAsync(payment);

            var result = await _unitOfWork.SaveAsync();
            return result > 0 ? new SuccessResult("Payment recovered") : new ErrorResult("Recovered failed");
        }

        public async Task<IResult> RefundPaymentAsync(Guid paymentId)
        {
            var payment = await _unitOfWork.PaymentRepository.GetAsync(p => p.Id == paymentId);
            if (payment == null)
                throw new NotFoundException(ExceptionMessage.PaymentNotFound);

            if (payment.Status != PaymentStatus.Completed)
                return new ErrorResult("Only completed payments can be refunded");

            payment.Status = PaymentStatus.Refunded;
            payment.RefundedAt = DateTime.UtcNow;


            var reservation = await _unitOfWork.ReservationRepository.GetAsync(r => r.Id == payment.ReservationId);
            if (reservation != null)
            {
                reservation.Status = ReservationStatus.Refunded;
                reservation.IsPaid = false;
                reservation.RefundedAt = DateTime.UtcNow;
            }

            await _unitOfWork.PaymentRepository.Update(payment);
            var result = await _unitOfWork.SaveAsync();

            return result > 0 ? new SuccessResult("Payment refunded") : new ErrorResult("Payment refund failed");

        }

        public async Task<IResult> SoftDeletePaymentAsync(Guid id)
        {
            var payment = await _unitOfWork.PaymentRepository.GetAsync(p => p.Id == id);
            if (payment == null) throw new NotFoundException(ExceptionMessage.PaymentNotFound);

            await _unitOfWork.PaymentRepository.SoftDeleteAsync(payment);

            var result = await _unitOfWork.SaveAsync();
            return result > 0 ? new SuccessResult("Payment deleted") : new ErrorResult("Soft delete failed");
        }

        public async Task<IResult> UpdatePaymentAsync(Guid id, PaymentUpdateDto updateDTO)
        {
            var payment = await _unitOfWork.PaymentRepository.GetAsync(p => p.Id == id && !p.IsDeleted);
            if (payment == null) throw new NotFoundException("Payment not found");


            payment.Amount = updateDTO.Amount != 0 ? updateDTO.Amount : payment.Amount;
            payment.Status = updateDTO.Status != null ? updateDTO.Status.Value : payment.Status;
            payment.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.PaymentRepository.Update(payment);
            var result = await _unitOfWork.SaveAsync();
            return result > 0 ? new SuccessResult("Payment updated") : new ErrorResult("Payment update failed");

        }
    }
}
