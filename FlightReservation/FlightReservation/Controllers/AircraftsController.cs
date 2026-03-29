using FlighReservation_Business.Services.Abstracts;
using FlightReservation_Entities.DTOs.AircraftDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AircraftsController : ControllerBase
    {
        private readonly IAircraftService _aircraftService;

        public AircraftsController(IAircraftService aircraftService)
        {
            _aircraftService = aircraftService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAircraft([FromBody] AircraftCreateDto createDto)
        {
            var result = await _aircraftService.AddAircraftAsync(createDto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAircraft(Guid id, [FromBody] AircraftUpdateDto updateDto)
        {
            var result = await _aircraftService.UpdateAircraftAsync(id, updateDto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpDelete("soft/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SoftDeleteAircraft(Guid id)
        {
            var result = await _aircraftService.SoftDeleteAircraftAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpDelete("hard/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> HardDeleteAircraft(Guid id)
        {
            var result = await _aircraftService.HardDeleteAircraftAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPatch("recover/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RecoverAircraft(Guid id)
        {
            var result = await _aircraftService.RecoverAircraftAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> GetAircraftById(Guid id)
        {
            var result = await _aircraftService.GetAircraftByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> GetAllAircrafts()
        {
            var result = await _aircraftService.GetAllAircraftsAsync();
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("paginated")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> GetAllAircraftsPaginated([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _aircraftService.GetAllAircraftsPaginatedAsync(page, size);
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("deleted")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllDeletedAircrafts()
        {
            var result = await _aircraftService.GetAllDeletedAircraftsAsync();
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }
    }
}
