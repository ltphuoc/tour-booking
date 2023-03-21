using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using DataAccess.Services;
using DataAccess.DTO.Request;
using System.Net;
using DataAccess.DTO.Response;

namespace TourBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToursController : ControllerBase
    {
        private readonly ITourSevices _tourServices;
        private readonly TourBookingContext _context;

        public ToursController(TourBookingContext context, ITourSevices tourServices)
        {
            _context = context;
            _tourServices = tourServices;
        }

        // GET: api/Tours
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TourResponse>>> GetTours([FromQuery] PagingRequest request, [FromQuery] int destinationId)
        {
            var result = _tourServices.GetAll(request, destinationId);
            return Ok(result);
        }

        // GET: api/Tours/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TourResponse>> GetTour(int id)
        {
            var result = _tourServices.Get(id);
            return Ok(result);
        }

        // PUT: api/Tours/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTour(int id, TourUpdateRequest tour)
        {
            var result = _tourServices.Update(id, tour).Result;
            if (result.Status.Code != HttpStatusCode.OK)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST: api/Tours
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TourResponse>> PostTour(TourCreateRequest tour)
        {
            var result = await _tourServices.Create(tour);
            if (result.Status.Code != HttpStatusCode.Created)
            {
                return BadRequest(result);
            }
            return Created("", result);
        }

        // DELETE: api/Tours/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTour(int id)
        {
            var result = _tourServices.Delete(id).Result;
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

        /*private bool TourExists(int id)
        {
            return _context.Tours.Any(e => e.Id == id);
        }*/
    }
}
