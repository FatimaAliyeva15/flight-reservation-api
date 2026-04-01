using FlighReservation_Business.Services.Abstracts;
using FlightReservation_Entities.DTOs.FlightDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightsController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Operator, Customer")]
        public async Task<IActionResult> GetAllFlights()
        {
            var result = await _flightService.GetAllFlightsAsync();
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpGet("paginated")]
        [Authorize(Roles = "Admin, Operator, Customer")]
        public async Task<IActionResult> GetAllFlightsPaginated([FromQuery] int page, [FromQuery] int size)
        {
            var result = await _flightService.GetAllFlightsPaginatedAsync(page, size);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpGet("deleted")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllDeletedFlights()
        {
            var result = await _flightService.GetAllDeletedFlightsAsync();
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> GetFlightById(Guid id)
        {
            var result = await _flightService.GetFlightByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }


        [HttpPost]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> AddFlight([FromBody] FlightCreateDto createDto)
        {
            var result = await _flightService.AddFlightAsync(createDto);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> UpdateFlight(Guid id, [FromBody] FlightUpdateDto updateDto)
        {
            var result = await _flightService.UpdateFlightAsync(id, updateDto);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        

        
        [HttpPatch("recover/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RecoverFlight(Guid id)
        {
            var result = await _flightService.RecoverFlightAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> ApproveFlight(Guid id)
        {
            var result = await _flightService.ApproveFlightAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> RejectFlight(Guid id, [FromBody] string adminComment)
        {
            var result = await _flightService.RejectFlightAsync(id, adminComment);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPatch("{id}/cancel")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> CancelFlight(Guid id)
        {
            var result = await _flightService.CancelFlightAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpDelete("hard/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> HardDeleteFlight(Guid id)
        {
            var result = await _flightService.HardDeleteFlightAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        
        [HttpDelete("soft/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SoftDeleteFlight(Guid id)
        {
            var result = await _flightService.SoftDeleteFlightAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
