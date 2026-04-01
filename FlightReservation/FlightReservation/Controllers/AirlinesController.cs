using FlighReservation_Business.Services.Abstracts;
using FlightReservation_Entities.DTOs.AirlineDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AirlinesController : ControllerBase
    {
        private readonly IAirlineService _airlineService;

        public AirlinesController(IAirlineService airlineService)
        {
            _airlineService = airlineService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAirline([FromBody] AirlineCreateDto createDto)
        {
            var result = await _airlineService.AddAirlineAsync(createDto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> UpdateAirline(Guid id, [FromBody] AirlineUpdateDto updateDto)
        {
            var result = await _airlineService.UpdateAirlineAsync(id, updateDto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpDelete("soft/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SoftDeleteAirline(Guid id)
        {
            var result = await _airlineService.SoftDeleteAirclineAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpDelete("hard/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> HardDeleteAirline(Guid id)
        {
            var result = await _airlineService.HardDeleteAirlineAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPatch("recover/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RecoverAirline(Guid id)
        {
            var result = await _airlineService.RecoverAirlineAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> GetAirlineById(Guid id)
        {
            var result = await _airlineService.GetAirlineByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Operator, Customer")]
        public async Task<IActionResult> GetAllAirlines()
        {
            var result = await _airlineService.GetAllAirlinesAsync();
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("paginated")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> GetAllAirlinesPaginated([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _airlineService.GetAllAirlinesPaginatedAsync(page, size);
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("deleted")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllDeletedAirlines()
        {
            var result = await _airlineService.GetAllDeletedAirlinesAsync();
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }
    }
}
