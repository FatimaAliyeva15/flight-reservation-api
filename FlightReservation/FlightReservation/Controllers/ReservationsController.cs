using FlighReservation_Business.Services.Abstracts;
using FlighReservation_Business.Services.Concretes;
using FlightReservation_Entities.DTOs.ReservationDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FlightReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReservations()
        {
            var result = await _reservationService.GetAllReservationsAsync();
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }
        

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservationById(Guid id)
        {
            var result = await _reservationService.GetReservationByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginatedReservation([FromQuery] int page, [FromQuery] int size)
        {
            var result = await _reservationService.GetAllReservationsPaginatedAsync(page, size);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpGet("deleted")]
        public async Task<IActionResult> GetDeletedReservation()
        {
            var result = await _reservationService.GetAllDeletedReservationsAsync();
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddReservation([FromBody] ReservationCreateDto createDto)
        {
            var result = await _reservationService.AddReservationAsync(createDto);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }
            

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservation(Guid id, [FromBody] ReservationUpdateDto updateDto)
        {
            var result = await _reservationService.UpdateReservationAsync(id, updateDto);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpPatch("soft-delete/{id}")]
        public async Task<IActionResult> SoftDeleteReservation(Guid id)
        {
            var result = await _reservationService.SoftDeleteReservationAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> HardDeleteReservation(Guid id) =>
            Ok(await _reservationService.HardDeleteReservationAsync(id));

        [HttpPatch("recover/{id}")]
        public async Task<IActionResult> RecoverReservation(Guid id)
        {
            var result = await _reservationService.RecoverReservationAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }
            

        [HttpPost("create-with-tickets")]
        public async Task<IActionResult> CreateReservationWithTickets([FromBody] ReservationWithTicketsDto dto)
        {
            var result = await _reservationService.CreateReservationWithTicketsAsync(dto.Reservation, dto.Tickets);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpPost("cancel/{id}")]
        public async Task<IActionResult> CancelReservation(Guid id)
        {
            var result = await _reservationService.CancelReservationAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }


        [HttpGet("by-passenger/{userId}")]
        public async Task<IActionResult> GetReservationByPassenger(string userId)
        {
            var result = await _reservationService.GetReservationsByPassengerAsync(userId);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpPost("confirm-after-payment/{id}")]
        public async Task<IActionResult> ConfirmReservationAfterPayment(Guid id)
        {
            var result = await _reservationService.ConfirmReservationAfterPaymentAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

    }
}
