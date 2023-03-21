﻿using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;

namespace TourBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationsController : ControllerBase
    {

        private readonly IDestinationServices _destinationServices;

        public DestinationsController(IDestinationServices destinationServices)
        {
            _destinationServices = destinationServices;
        }

        // GET: api/Destinations
        [HttpGet]
        public ActionResult<IEnumerable<DestinationResponse>> GetDestinations([FromQuery] PagingRequest request)
        {
            var result = _destinationServices.GetAll(request);
            return StatusCode((int)result.Status.Code, result);
        }

        // GET: api/Destinations/5
        [HttpGet("{id}")]
        public ActionResult<DestinationResponse> GetDestination(int id)
        {
            var result = _destinationServices.Get(id);
            return StatusCode((int)result.Status.Code, result);
        }

        // PUT: api/Destinations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDestination(int id, DestinationUpdateRequest destination)
        {
            var result = _destinationServices.Update(id, destination).Result;
            return StatusCode((int)result.Status.Code, result);
        }

        // POST: api/Destinations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DestinationResponse>> PostDestination(DestinationCreateRequest destination)
        {
            var result = await _destinationServices.Create(destination);
            return StatusCode((int)result.Status.Code, result);
        }

        // DELETE: api/Destinations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDestination(int id)
        {
            var result = _destinationServices.Delete(id).Result;
            return StatusCode((int)result.Status.Code, result);
        }
    }
}
