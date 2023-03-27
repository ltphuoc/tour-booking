﻿using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using DataAccess.Services;
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


    }
}
