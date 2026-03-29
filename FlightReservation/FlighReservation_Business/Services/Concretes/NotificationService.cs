
using AutoMapper;
using FlighReservation_Business.Services.Abstracts;
using FlighReservation_Business.Utilities.Constants;
using FlightReservation_Core.Business.Utilities.Exceptions;
using FlightReservation_Core.Business.Utilities.Results.Abstract;
using FlightReservation_Core.Business.Utilities.Results.Concrete;
using FlightReservation_DataAccess.UnitOfWork.Abstract;
using FlightReservation_Entities.Concretes;
using FlightReservation_Entities.DTOs.NotificationDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Services.Concretes
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResult> AddNotificationAsync(NotificationCreateDto createDTO)
        {
            var notification = _mapper.Map<Notification>(createDTO);

            notification.CreatedAt = DateTime.UtcNow;
            notification.SentAt = DateTime.UtcNow;
            notification.IsRead = false;

            await _unitOfWork.NotificationRepository.AddAsync(notification);

            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Notification not added");

            return new SuccessResult("Notification added");
        }

        public async Task<IDataResult<List<NotificationGetAllDto>>> GetAllDeletedNotificationsAsync()
        {
            var notifications = await _unitOfWork.NotificationRepository.GetDeletedAsync("AppUser");

            if (notifications.Count == 0)
                return new ErrorDataResult<List<NotificationGetAllDto>>(new List<NotificationGetAllDto>(),"Deleted notifications not found");

            var dtos = _mapper.Map<List<NotificationGetAllDto>>(notifications);

            return new SuccessDataResult<List<NotificationGetAllDto>>(dtos, "Deleted notifications listed");
        }

        public async Task<IDataResult<List<NotificationGetAllDto>>> GetAllNotificationsAsync()
        {
            var notifications = await _unitOfWork.NotificationRepository.GetAllAsync(null, "AppUser");

            if (notifications.Count == 0)
                return new ErrorDataResult<List<NotificationGetAllDto>>(
                    new List<NotificationGetAllDto>(),
                    "Notifications not found");

            var dtos = _mapper.Map<List<NotificationGetAllDto>>(notifications);

            return new SuccessDataResult<List<NotificationGetAllDto>>(dtos, "Notifications listed");
        }

        public async Task<IDataResult<List<NotificationGetAllDto>>> GetAllNotificationsPaginatedAsync(int page, int size)
        {
            var notifications = await _unitOfWork.NotificationRepository
            .GetAllPaginatedAsync(page, size, null, "AppUser");

            if (notifications.Count == 0)
                return new ErrorDataResult<List<NotificationGetAllDto>>(
                    new List<NotificationGetAllDto>(),
                    "Notifications not found");

            var dtos = _mapper.Map<List<NotificationGetAllDto>>(notifications);

            return new SuccessDataResult<List<NotificationGetAllDto>>(dtos, "Notifications listed");
        }

        public async Task<IDataResult<NotificationGetDto>> GetNotificationByIdAsync(Guid id)
        {
            var notification = await _unitOfWork.NotificationRepository.GetAsync(n => n.Id == id, includeDeleted: false, "AppUser");

            if (notification == null)
                return new ErrorDataResult<NotificationGetDto>(null, "Notification not found");

            var dto = _mapper.Map<NotificationGetDto>(notification);

            dto.UserName = notification.AppUser?.UserName;

            return new SuccessDataResult<NotificationGetDto>(dto, "Notification found");
        }

        public async Task<IResult> HardDeleteNotificationAsync(Guid id)
        {
            var notification = await _unitOfWork.NotificationRepository.GetAsync(n => n.Id == id, includeDeleted: true);

            if (notification == null)
                throw new NotFoundException(ExceptionMessage.NotificationNotFound);

            await _unitOfWork.NotificationRepository
                .HardDeleteAsync(notification);

            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Notification not deleted");

            return new SuccessResult("Notification permanently deleted");
        }

        public async Task<IResult> RecoverNotificationAsync(Guid id)
        {
            var notification = await _unitOfWork.NotificationRepository
            .GetDeletedAsync()
            .ContinueWith(t => t.Result.FirstOrDefault(n => n.Id == id));

            if (notification == null)
                throw new NotFoundException(ExceptionMessage.NotificationNotFound);

            await _unitOfWork.NotificationRepository
                .RecoverAsync(notification);

            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Notification not recovered");

            return new SuccessResult("Notification recovered");
        }

        public async Task<IResult> SoftDeleteNotificationAsync(Guid id)
        {
            var notification = await _unitOfWork.NotificationRepository
            .GetAsync(n => n.Id == id);

            if (notification == null)
                throw new NotFoundException(ExceptionMessage.NotificationNotFound);

            await _unitOfWork.NotificationRepository
                .SoftDeleteAsync(notification);

            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Notification not deleted");

            return new SuccessResult("Notification deleted");

        }

        public async Task<IResult> UpdateNotificationAsync(Guid id, NotificationUpdateDto updateDTO)
        {
            var notification = await _unitOfWork.NotificationRepository
            .GetAsync(n => n.Id == id);

            if (notification == null)
                throw new NotFoundException(ExceptionMessage.NotificationNotFound);

            notification.Title = updateDTO.Title ?? notification.Title;
            notification.Message = updateDTO.Message ?? notification.Message;

            if (updateDTO.IsRead.HasValue)
                notification.IsRead = updateDTO.IsRead.Value;

            notification.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.NotificationRepository.Update(notification);

            var result = await _unitOfWork.SaveAsync();

            if (result == 0)
                return new ErrorResult("Notification not updated");

            return new SuccessResult("Notification updated");
        }
    }
}
