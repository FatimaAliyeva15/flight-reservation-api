using FlighReservation_Business.Services.Abstracts;
using FlightReservation_Entities.DTOs.AirportDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AirportsController : ControllerBase
    {
        private readonly IAirportService _airportService;

        public AirportsController(IAirportService airportService)
        {
            _airportService = airportService;
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAirport([FromBody] AirportCreateDto createDto)
        {
            var result = await _airportService.AddAirportAsync(createDto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> UpdateAirport(Guid id, [FromBody] AirportUpdateDto updateDto)
        {
            var result = await _airportService.UpdateAirportAsync(id, updateDto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpDelete("soft/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SoftDeleteAirport(Guid id)
        {
            var result = await _airportService.SoftDeleteAirportAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpDelete("hard/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> HardDeleteAirport(Guid id)
        {
            var result = await _airportService.HardDeleteAirportAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPatch("recover/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RecoverAirport(Guid id)
        {
            var result = await _airportService.RecoverAirportAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> GetAirportById(Guid id)
        {
            var result = await _airportService.GetAirportByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Operator, Customer")]
        public async Task<IActionResult> GetAllAirports()
        {
            var result = await _airportService.GetAllAirportsAsync();
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("paginated")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> GetAllAirportsPaginated([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _airportService.GetAllAirportsPaginatedAsync(page, size);
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("deleted")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllDeletedAirports()
        {
            var result = await _airportService.GetAllDeletedAirportsAsync();
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }
    }
}
