using FlighReservation_Business.Services.Abstracts.IAuthService;
using FlightReservation_Entities.DTOs.AuthDTOs;
using Microsoft.AspNetCore.Mvc;



namespace FlightReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Regiter(RegisterDTO register)
        {
            var result = await _service.Register(register);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            var result = await _service.Login(login);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            var result = await _service.ConfirmEmail(userId, token);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }


        //[ApiExplorerSettings(IgnoreApi = true)]
        //[Authorize(Roles = "SuperAdmin")]
        [HttpPost("add-admin")]
        public async Task<IActionResult> AddAdmin([FromBody] RegisterDTO registerDto)
        {
            var result = await _service.AddAdmin(registerDto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var result = await _service.ForgotPassword(email);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetDto)
        {
            var result = await _service.ResetPassword(resetDto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }
    }
}
