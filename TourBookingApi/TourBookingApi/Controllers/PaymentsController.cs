using BusinessObject.Models;
using DataAccess.DTO.Request;
using DataAccess.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TourBookingApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {

        private readonly IPaymentSevices _paymentServices;

        public PaymentsController(IPaymentSevices paymentServices)
        {
            _paymentServices = paymentServices;
        }

        // GET: api/Tours
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments([FromQuery] PagingRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = _paymentServices.GetAll(request);
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // GET: api/Tours/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            if (ModelState.IsValid)
            {
                var result = _paymentServices.Get(id);
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // PUT: api/Tours/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment(int id, [FromBody] string image)
        {
            var result = _paymentServices.UpdateImage(id, image).Result;
            return StatusCode((int)result.Status.Code, result);
        }

        [HttpPut("{id}/update-status/{status}")]
        public async Task<IActionResult> PutPaymentStatus(int id, int status)
        {
            var result = _paymentServices.UpdateStatus(id, status).Result;
            return StatusCode((int)result.Status.Code, result);
        }

        // POST: api/Tours
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Payment>> PostPayment(PaymentCreateRequest payment)
        {
            var result = await _paymentServices.Create(payment);
            return StatusCode((int)result.Status.Code, result);
        }

        // DELETE: api/Tours/5
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var result = _paymentServices.Delete(id).Result;
            return StatusCode((int)result.Status.Code, result);
        }
    }
}
