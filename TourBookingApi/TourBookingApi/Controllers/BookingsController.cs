﻿using BusinessObject.Models;
using BusinessObjects.Services;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using DataAccess.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TourBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingSevices _bookingServices;
        private readonly IConfiguration _configuration;

        public BookingsController(IBookingSevices bookingServices, IConfiguration configuration)
        {
            _bookingServices = bookingServices;
            _configuration = configuration;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingResponse>>> GetBookings([FromQuery] PagingRequest request)
        {
            var result = _bookingServices.GetAll(request);
            return StatusCode((int)result.Status.Code, result);
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
