using FlighReservation_Business.Services.Abstracts;
using FlighReservation_Business.Services.Concretes;
using FlightReservation_Entities.DTOs.PaymentDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlightReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }


        [HttpGet]
        //[Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> GetAllPayments()
        {
            var result = await _paymentService.GetAllPaymentsAsync();
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> GetPaymentById(Guid id)
        {
            var result = await _paymentService.GetPaymentByIdAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }


        [HttpGet("reservation/{reservationId}")]
        public async Task<IActionResult> GetPaymentsByReservation(Guid reservationId)
        {
            var result = await _paymentService.GetPaymentsByReservationAsync(reservationId);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpGet("deleted")]
        public async Task<IActionResult> GetAllDeletedPayments()
        {
            var result = await _paymentService.GetAllDeletedPaymentsAsync();
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> GetAllPaymentsPaginated([FromQuery] int page, [FromQuery] int size)
        {
            var result = await _paymentService.GetAllPaymentsPaginatedAsync(page, size);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }



        [HttpGet("passenger")]
        public async Task<IActionResult> GetPaymentsByCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _paymentService.GetPaymentsByPassengerAsync(userId);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentCreateDto createDto)
        {
            var result = await _paymentService.AddPaymentAsync(createDto);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }


        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> UpdatePayment(Guid id, [FromBody] PaymentUpdateDto updateDto)
        {
            var result = await _paymentService.UpdatePaymentAsync(id, updateDto);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }


        [HttpPatch("confirm/{id}")]
        public async Task<IActionResult> ConfirmPayment(Guid id)
        {
            var result = await _paymentService.ConfirmPaymentAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        
        [HttpPatch("refund/{id}")]
        public async Task<IActionResult> RefundPayment(Guid id)
        {
            var result = await _paymentService.RefundPaymentAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }


        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> SoftDeletePayment(Guid id)
        {
            var result = await _paymentService.SoftDeletePaymentAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }


        [HttpDelete("hard/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> HardDeletePayment(Guid id)
        {
            var result = await _paymentService.HardDeletePaymentAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        [HttpPatch("recover/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> RecoverPayment(Guid id)
        {
            var result = await _paymentService.RecoverPaymentAsync(id);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

    }
}
