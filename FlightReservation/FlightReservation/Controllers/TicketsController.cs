using FlighReservation_Business.Services.Abstracts;
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
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdTicket(Guid id)
        {
            var result = await _ticketService.GetTicketByIdAsync(id);
            return result.Success ? Ok(result.Message) : NotFound(result.Message);
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginatedTicket(int page, int size)
        {
            var result = await _ticketService.GetAllTicketsPaginatedAsync(page, size);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpGet("deleted")]
        public async Task<IActionResult> GetDeletedTicket()
        {
            var result = await _ticketService.GetAllDeletedTicketsAsync();
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> AddTicket(TicketCreateDto dto)
        {
            var result = await _ticketService.AddTicketAsync(dto);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(Guid id, TicketUpdateDto dto)
        {
            var result = await _ticketService.UpdateTicketAsync(id, dto);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpDelete("soft/{id}")]
        public async Task<IActionResult> SoftDeleteTicket(Guid id)
        {
            var result = await _ticketService.SoftDeleteTicketAsync(id);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpDelete("hard/{id}")]
        public async Task<IActionResult> HardDeleteTicket(Guid id)
        {
            var result = await _ticketService.HardDeleteTicketAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("recover/{id}")]
        public async Task<IActionResult> Recover(Guid id)
        {
            var result = await _ticketService.RecoverTicketAsync(id);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }


        [HttpPatch("assign-seat")]
        public async Task<IActionResult> AssignSeatToTicket(Guid ticketId, Guid seatId)
        {
            var result = await _ticketService.AssignSeatToTicketAsync(ticketId, seatId);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPatch("cancel/{ticketId}")]
        public async Task<IActionResult> CancelTicket(Guid ticketId)
        {
            var result = await _ticketService.CancelTicketAsync(ticketId);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpGet("by-reservation/{reservationId}")]
        public async Task<IActionResult> GetTicketByReservation(Guid reservationId)
        {
            var result = await _ticketService.GetTicketsByReservationAsync(reservationId);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpGet("by-passenger/{passengerId}")]
        public async Task<IActionResult> GetTicketByPassenger(Guid passengerId)
        {
            var result = await _ticketService.GetTicketsByPassengerAsync(passengerId);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }
    }
}
