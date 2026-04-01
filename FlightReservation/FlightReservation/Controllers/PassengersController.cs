using FlighReservation_Business.Services.Abstracts;
using FlightReservation_Entities.DTOs.PassengerDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlightReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PassengersController : ControllerBase
    {
        private readonly IPassengerService _passengerService;

        public PassengersController(IPassengerService passengerService)
        {
            _passengerService = passengerService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Operator, Customer")]
        public async Task<IActionResult> AddPassenger([FromBody] PassengerCreateDto createDto)
        {
            var result = await _passengerService.AddPassengerAsync(createDto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        //[HttpPut("{id:guid}")]
        //public async Task<IActionResult> UpdatePassenger(Guid id, [FromBody] PassengerUpdateDto updateDto)
        //{
        //    var result = await _passengerService.UpdatePassengerAsync(id, updateDto);
        //    if (!result.Success)
        //        return BadRequest(result.Message);
        //    return Ok(result.Message);
        //}
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin,Operator,Customer")]
        public async Task<IActionResult> UpdatePassenger(Guid id, [FromBody] PassengerUpdateDto updateDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (role == "Customer" && id.ToString() != userId)
                return Forbid("You can only update your own profile");

            var result = await _passengerService.UpdatePassengerAsync(id, updateDto);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpDelete("soft/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SoftDeletePassenger(Guid id)
        {
            var result = await _passengerService.SoftDeletePassengerAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpDelete("hard/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> HardDeletePassenger(Guid id)
        {
            var result = await _passengerService.HardDeletePassengerAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPatch("recover/{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RecoverPassenger(Guid id)
        {
            var result = await _passengerService.RecoverPassengerAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> GetPassengerById(Guid id)
        {
            var result = await _passengerService.GetPassengerByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Operator, Customer")]
        public async Task<IActionResult> GetAllPassengers()
        {

            var result = await _passengerService.GetAllPassengersAsync();
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("paginated")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<IActionResult> GetAllPassengersPaginated([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var result = await _passengerService.GetAllPassengersPaginatedAsync(page, size);
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("deleted")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllDeletedPassengers()
        {
            var result = await _passengerService.GetAllDeletedPassengersAsync();
            if (!result.Success)
                return NotFound(result.Message);
            return Ok(result.Data);
        }
    }
}
