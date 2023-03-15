using BusinessObject.Models;
using DataAccess.DTO.Request;
using DataAccess.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace TourBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransportationsController : ControllerBase
    {
        private readonly ITransportationSevices _transportationServices;
        private readonly TourBookingContext _context;

        public TransportationsController(TourBookingContext context, ITransportationSevices transportationServices)
        {
            _context = context;
            _transportationServices = transportationServices;
        }

        // GET: api/Tours
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transportation>>> GetTransportations([FromQuery] PagingRequest request)
        {
            var result = _transportationServices.GetAll(request);
            return Ok(result);
        }

        // GET: api/Tours/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transportation>> GetTransportation(int id)
        {
            var result = _transportationServices.Get(id);
            return Ok(result);
        }

        // PUT: api/Tours/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransportation(int id, TransportationUpdateRequest transportation)
        {
            var result = _transportationServices.Update(id, transportation).Result;
            if (result.Status.Code != HttpStatusCode.OK)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST: api/Tours
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Transportation>> PostTransportation(TransportationCreateRequest transportation)
        {
            var result = await _transportationServices.Create(transportation);
            if (result.Status.Code != HttpStatusCode.Created)
            {
                return BadRequest(result);
            }
            return Created("", result);
        }

        // DELETE: api/Tours/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransportation(int id)
        {
            var result = _transportationServices.Delete(id).Result;
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
