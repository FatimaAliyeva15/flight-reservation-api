using FlighReservation_Business.Services.Abstracts;
using FlighReservation_Business.Services.Concretes;
using FlightReservation_Entities.DTOs.ReservationDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlightReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllReservations()
        {
            var result = await _reservationService.GetAllReservationsAsync();
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }
        

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> GetReservationById(Guid id)
        {
            var result = await _reservationService.GetReservationByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpGet("paginated")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPaginatedReservation([FromQuery] int page, [FromQuery] int size)
        {
            var result = await _reservationService.GetAllReservationsPaginatedAsync(page, size);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpGet("deleted")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDeletedReservation()
        {
            var result = await _reservationService.GetAllDeletedReservationsAsync();
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        
        [HttpPost]
        [Authorize(Roles = "Customer, Admin")]
        public async Task<IActionResult> AddReservation([FromBody] ReservationCreateDto createDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _reservationService.AddReservationAsync(createDto, userId);

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }
            

        [HttpPut("{id}")]
        [Authorize(Roles = "Customer,Operator")]
        public async Task<IActionResult> UpdateReservation(Guid id, [FromBody] ReservationUpdateDto updateDto)
        {
            var result = await _reservationService.UpdateReservationAsync(id, updateDto);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpDelete("soft-delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SoftDeleteReservation(Guid id)
        {
            var result = await _reservationService.SoftDeleteReservationAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> HardDeleteReservation(Guid id)
        {
            var result = await _reservationService.HardDeleteReservationAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpPatch("recover/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RecoverReservation(Guid id)
        {
            var result = await _reservationService.RecoverReservationAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }
            

        [HttpPost("create-with-tickets")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateReservationWithTickets([FromBody] ReservationWithTicketsDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _reservationService.CreateReservationWithTicketsAsync(dto.Reservation, dto.Tickets, userId);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpPatch("cancel/{id}")]
        [Authorize(Roles = "Customer,Operator, Admin")]
        public async Task<IActionResult> CancelReservation(Guid id)
        {
            var result = await _reservationService.CancelReservationAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }


        [HttpGet("my-reservations")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyReservations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _reservationService.GetReservationsByPassengerAsync(userId);

            return Ok(result);
        }


        [HttpPatch("confirm-after-payment/{id}")]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> ConfirmReservationAfterPayment(Guid id)
        {
            var result = await _reservationService.ConfirmReservationAfterPaymentAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

    }
}
