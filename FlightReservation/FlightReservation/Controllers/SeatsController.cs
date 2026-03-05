using FlighReservation_Business.Services.Abstracts;
using FlighReservation_Business.Services.Concretes;
using FlightReservation_Entities.DTOs.SeatDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FlightReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatsController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSeats()
        {
            var result = await _seatService.GetAllSeatsAsync();
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdSeat(Guid id)
        {
            var result = await _seatService.GetSeatByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginatedSeat(int page, int size)
        {
            var result = await _seatService.GetAllSeatsPaginatedAsync(page, size);
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("deleted")]
        public async Task<IActionResult> GetDeletedSeat()
        {
            var result = await _seatService.GetAllDeletedSeatsAsync();
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> AddSeat(SeatCreateDto dto)
        {
            var result = await _seatService.AddSeatAsync(dto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeat(Guid id, SeatUpdateDto dto)
        {
            var result = await _seatService.UpdateSeatAsync(id, dto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpDelete("soft/{id}")]
        public async Task<IActionResult> SoftDeleteSeat(Guid id)
        {
            var result = await _seatService.SoftDeleteSeatAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpDelete("hard/{id}")]
        public async Task<IActionResult> HardDeleteSeat(Guid id)
        {
            var result = await _seatService.HardDeleteSeatAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPatch("recover/{id}")]
        public async Task<IActionResult> RecoverSeat(Guid id)
        {
            var result = await _seatService.RecoverSeatAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }


        [HttpPatch("reserve")]
        public async Task<IActionResult> ReserveSeat(Guid seatId, Guid reservationId)
        {
            var result = await _seatService.ReserveSeatAsync(seatId, reservationId);
            if(!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPatch("book")]
        public async Task<IActionResult> BookSeat(Guid seatId, Guid reservationId)
        {
            var result = await _seatService.BookSeatAsync(seatId, reservationId);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPatch("release/{seatId}")]
        public async Task<IActionResult> ReleaseSeat(Guid seatId)
        {
            var result = await _seatService.ReleaseSeatAsync(seatId);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpGet("by-flight/{flightId}")]
        public async Task<IActionResult> GetSeatsByFlight(Guid flightId, bool onlyAvailable = false)
        {
            var result = await _seatService.GetSeatsByFlightAsync(flightId, onlyAvailable);
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }
    }
}
