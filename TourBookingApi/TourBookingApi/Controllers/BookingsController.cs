using BusinessObject.Models;
using BusinessObjects.Services;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using DataAccess.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TourBookingApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingSevices _bookingServices;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        public BookingsController(IBookingSevices bookingServices, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _bookingServices = bookingServices;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        // GET: api/Bookings
        [HttpGet]

        public async Task<ActionResult<IEnumerable<BookingResponse>>> GetBookings([FromQuery] PagingRequest request)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = _jwtAuthenticationManager.GetUserIdFromJwtToken(token);
            if (string.IsNullOrEmpty(userId))
            {
                var result = _bookingServices.GetAll(request);
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                var result = _bookingServices.GetAll(request, userId);
                return StatusCode((int)result.Status.Code, result);
            }
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var result = _bookingServices.Get(id);
            return StatusCode((int)result.Status.Code, result);
        }

        // PUT: api/Bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, BookingUpdateRequest booking)
        {
            var result = _bookingServices.Update(id, booking).Result;
            return StatusCode((int)result.Status.Code, result);
        }

        [HttpPut("{id}/payment/{payment_id}")]
        public async Task<IActionResult> PutPaymentStatus(int id, int payment_id, [FromBody] int status = 1)
        {
            var result = await _bookingServices.UpdatePaymentStatus(id, payment_id, status);
            return StatusCode((int)result.Status.Code, result);
        }

        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(BookingCreateRequest booking)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var customerId = _jwtAuthenticationManager.GetUserIdFromJwtToken(token);
            booking.CustomerId = int.Parse(customerId!);

            var result = await _bookingServices.Create(booking);
            return StatusCode((int)result.Status.Code, result);
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var result = _bookingServices.Delete(id).Result;
            return StatusCode((int)result.Status.Code, result);
        }
    }
}
