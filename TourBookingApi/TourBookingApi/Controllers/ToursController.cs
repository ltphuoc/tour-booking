using BusinessObject.Models;
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
    public class ToursController : ControllerBase
    {
        private readonly ITourSevices _tourServices;

        public ToursController(ITourSevices tourServices)
        {
            _tourServices = tourServices;
        }

        // GET: api/Tours
        [HttpGet]
        public ActionResult<IEnumerable<TourResponse>> GetTours([FromQuery] PagingRequest request, [FromQuery] int destinationId)
        {
            if (ModelState.IsValid)
            {
                var result = _tourServices.GetAll(request, destinationId);
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // GET: api/Tours/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TourResponse>> GetTour(int id)
        {
            var result = _tourServices.Get(id);
            return StatusCode((int)result.Status.Code, result);
        }

        // PUT: api/Tours/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTour(int id, TourUpdateRequest tour)
        {
            if (ModelState.IsValid)
            {
                var result = _tourServices.Update(id, tour).Result;
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // POST: api/Tours
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult<TourResponse>> PostTour(TourCreateRequest tour)
        {
            if (ModelState.IsValid)
            {
                var result = await _tourServices.Create(tour);
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Tours/5
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTour(int id)
        {
            var result = _tourServices.Delete(id).Result;
            return StatusCode((int)result.Status.Code, result);
        }


    }
}
