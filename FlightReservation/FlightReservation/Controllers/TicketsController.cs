using FlighReservation_Business.Services.Abstracts;
using FlighReservation_Business.Services.Concretes;
using FlightReservation_Entities.DTOs.TicketDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTickets()
        {
            var result = await _ticketService.GetAllTicketsAsync();
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdTicket(Guid id)
        {
            var result = await _ticketService.GetTicketByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginatedTicket(int page, int size)
        {
            var result = await _ticketService.GetAllTicketsPaginatedAsync(page, size);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpGet("deleted")]
        public async Task<IActionResult> GetDeletedTicket()
        {
            var result = await _ticketService.GetAllDeletedTicketsAsync();
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddTicket(TicketCreateDto dto)
        {
            var result = await _ticketService.AddTicketAsync(dto);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(Guid id, TicketUpdateDto dto)
        {
            var result = await _ticketService.UpdateTicketAsync(id, dto);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpDelete("soft/{id}")]
        public async Task<IActionResult> SoftDeleteTicket(Guid id)
        {
            var result = await _ticketService.SoftDeleteTicketAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpDelete("hard/{id}")]
        public async Task<IActionResult> HardDeleteTicket(Guid id)
        {
            var result = await _ticketService.HardDeleteTicketAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpPatch("recover/{id}")]
        public async Task<IActionResult> Recover(Guid id)
        {
            var result = await _ticketService.RecoverTicketAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }


        [HttpPatch("assign-seat")]
        public async Task<IActionResult> AssignSeatToTicket(Guid ticketId, Guid seatId)
        {
            var result = await _ticketService.AssignSeatToTicketAsync(ticketId, seatId); 
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpPatch("cancel/{ticketId}")]
        public async Task<IActionResult> CancelTicket(Guid ticketId)
        {
            var result = await _ticketService.CancelTicketAsync(ticketId);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpGet("by-reservation/{reservationId}")]
        public async Task<IActionResult> GetTicketByReservation(Guid reservationId)
        {
            var result = await _ticketService.GetTicketsByReservationAsync(reservationId);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpGet("by-passenger/{passengerId}")]
        public async Task<IActionResult> GetTicketByPassenger(Guid passengerId)
        {
            var result = await _ticketService.GetTicketsByPassengerAsync(passengerId);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }
    }
}
