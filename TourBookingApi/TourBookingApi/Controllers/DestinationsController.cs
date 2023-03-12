using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using DataAccess.Services;
using System.Net;
using System.Security.Principal;

namespace TourBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationsController : ControllerBase
    {

        private readonly IDestinationServices _destinationServices;
        private readonly TourBookingContext _context;

        public DestinationsController(TourBookingContext context, IDestinationServices destinationServices)
        {
            _context = context;
            _destinationServices = destinationServices;
        }

        // GET: api/Destinations
        [HttpGet]
        public ActionResult<IEnumerable<DestinationResponse>> GetDestinations([FromQuery] PagingRequest request)
        {
            var result = _destinationServices.GetAll(request);
            return Ok(result);
        }

        // GET: api/Destinations/5
        [HttpGet("{id}")]
        public ActionResult<DestinationResponse> GetDestination(int id)
        {
            var result = _destinationServices.Get(id);
            return Ok(result);
        }

        // PUT: api/Destinations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDestination(int id, DestinationUpdateRequest destination)
        {
            var result = _destinationServices.Update(id, destination).Result;
            if (result.Status.Code != HttpStatusCode.OK)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST: api/Destinations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DestinationResponse>> PostDestination(DestinationCreateRequest destination)
        {
            var result = await _destinationServices.Create(destination);
            if (result.Status.Code != HttpStatusCode.Created)
            {
                return BadRequest(result);
            }
            return Created("", result);
        }

        // DELETE: api/Destinations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDestination(int id)
        {
            var result = _destinationServices.Delete(id).Result;
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
