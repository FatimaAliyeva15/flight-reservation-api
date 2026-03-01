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
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdSeat(Guid id)
        {
            var result = await _seatService.GetSeatByIdAsync(id);
            return result.Success ? Ok(result.Message) : NotFound(result.Message);
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginatedSeat(int page, int size)
        {
            var result = await _seatService.GetAllSeatsPaginatedAsync(page, size);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpGet("deleted")]
        public async Task<IActionResult> GetDeletedSeat()
        {
            var result = await _seatService.GetAllDeletedSeatsAsync();
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> AddSeat(SeatCreateDto dto)
        {
            var result = await _seatService.AddSeatAsync(dto);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeat(Guid id, SeatUpdateDto dto)
        {
            var result = await _seatService.UpdateSeatAsync(id, dto);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpDelete("soft/{id}")]
        public async Task<IActionResult> SoftDeleteSeat(Guid id)
        {
            var result = await _seatService.SoftDeleteSeatAsync(id);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpDelete("hard/{id}")]
        public async Task<IActionResult> HardDeleteSeat(Guid id)
        {
            var result = await _seatService.HardDeleteSeatAsync(id);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPatch("recover/{id}")]
        public async Task<IActionResult> RecoverSeat(Guid id)
        {
            var result = await _seatService.RecoverSeatAsync(id);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }


        [HttpPatch("reserve")]
        public async Task<IActionResult> ReserveSeat(Guid seatId, Guid reservationId)
        {
            var result = await _seatService.ReserveSeatAsync(seatId, reservationId);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPatch("book")]
        public async Task<IActionResult> BookSeat(Guid seatId, Guid reservationId)
        {
            var result = await _seatService.BookSeatAsync(seatId, reservationId);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPatch("release/{seatId}")]
        public async Task<IActionResult> ReleaseSeat(Guid seatId)
        {
            var result = await _seatService.ReleaseSeatAsync(seatId);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpGet("by-flight/{flightId}")]
        public async Task<IActionResult> GetSeatsByFlight(Guid flightId, bool onlyAvailable = false)
        {
            var result = await _seatService.GetSeatsByFlightAsync(flightId, onlyAvailable);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }
    }
}
