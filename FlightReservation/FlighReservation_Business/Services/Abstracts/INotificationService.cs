using FlightReservation_Core.Business.Utilities.Results.Abstract;
using FlightReservation_Entities.DTOs.NotificationDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlighReservation_Business.Services.Abstracts
{
    public interface INotificationService
    {
        Task<IDataResult<List<NotificationGetAllDto>>> GetAllNotificationsAsync();
        Task<IDataResult<NotificationGetDto>> GetNotificationByIdAsync(Guid id);
        Task<IDataResult<List<NotificationGetAllDto>>> GetAllNotificationsPaginatedAsync(int page, int size);
        Task<IDataResult<List<NotificationGetAllDto>>> GetAllDeletedNotificationsAsync();
        Task<IResult> AddNotificationAsync(NotificationCreateDto createDTO);
        Task<IResult> UpdateNotificationAsync(Guid id, NotificationUpdateDto updateDTO);
        Task<IResult> HardDeleteNotificationAsync(Guid id);
        Task<IResult> SoftDeleteNotificationAsync(Guid id);
        Task<IResult> RecoverNotificationAsync(Guid id);
    }
}
