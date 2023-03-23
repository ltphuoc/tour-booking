using BusinessObject.Models;
using BusinessObjects.Services;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using DataAccess.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace TourBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingSevices _bookingServices;
        private readonly TourBookingContext _context;
        private readonly IConfiguration _configuration;

        public BookingsController(TourBookingContext context, IBookingSevices bookingServices, IConfiguration configuration)
        {
            _context = context;
            _bookingServices = bookingServices;
            _configuration = configuration;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingResponse>>> GetBookings([FromQuery] PagingRequest request)
        {
            var result = _bookingServices.GetAll(request);
            return Ok(result);
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var result = _bookingServices.Get(id);
            return Ok(result);
        }

        // PUT: api/Bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, BookingUpdateRequest booking)
        {
            var result = _bookingServices.Update(id, booking).Result;
            if (result.Status.Code != HttpStatusCode.OK)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Booking>> PostBooking(BookingCreateRequest booking)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var customerId = JwtAuthenticationManager.GetUserIdFromJwtToken(token, _configuration);
            booking.CustomerId = int.Parse(customerId);

            var result = await _bookingServices.Create(booking);
            if (result.Status.Code != HttpStatusCode.Created)
            {
                return BadRequest(result);
            }
            return Created("", result);
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var result = _bookingServices.Delete(id).Result;
            if (result.Status.Code == HttpStatusCode.NotFound)
            {
                return NotFound(result);
            }
            else if (result.Status.Code == HttpStatusCode.BadRequest)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /*private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }*/
    }
}
