using FlighReservation_Business.Services.Abstracts;
using FlighReservation_Business.Services.Concretes;
using FlightReservation_Entities.DTOs.NotificationDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlightReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Operator, Customer")]
        public async Task<IActionResult> AddNotification([FromBody] NotificationCreateDto createDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _notificationService.AddNotificationAsync(createDto, userId);
            if (!result.Success)


                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> UpdateNotification(Guid id, [FromBody] NotificationUpdateDto updateDto)
        {
            var result = await _notificationService.UpdateNotificationAsync(id, updateDto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpDelete("soft/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SoftDeleteNotification(Guid id)
        {
            var result = await _notificationService.SoftDeleteNotificationAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpDelete("hard/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> HardDeleteNotification(Guid id)
        {
            var result = await _notificationService.HardDeleteNotificationAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPatch("recover/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RecoverNotification(Guid id)
        {
            var result = await _notificationService.RecoverNotificationAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> GetNotificationById(Guid id)
        {
            var result = await _notificationService.GetNotificationByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> GetAllNotifications()
        {
            var result = await _notificationService.GetAllNotificationsAsync();
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("my-notifications")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyNotifications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _notificationService.GetNotificationsByPassengerAsync(userId);

            return Ok(result);
        }

        [HttpGet("paginated")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> GetAllNotificationsPaginated([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _notificationService.GetAllNotificationsPaginatedAsync(page, size);
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("deleted")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllDeletedNotifications()
        {
            var result = await _notificationService.GetAllDeletedNotificationsAsync();
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }
    }
}
