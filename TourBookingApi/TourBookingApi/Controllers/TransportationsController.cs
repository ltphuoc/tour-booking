using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;

namespace TourBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransportationsController : ControllerBase
    {
        private readonly ITransportationSevices _transportationServices;

        public TransportationsController(ITransportationSevices transportationServices)
        {
            _transportationServices = transportationServices;
        }

        // GET: api/Tours
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransportationResponse>>> GetTransportations([FromQuery] PagingRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = _transportationServices.GetAll(request);
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // GET: api/Tours/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransportationResponse>> GetTransportation(int id)
        {
            var result = _transportationServices.Get(id);
            return StatusCode((int)result.Status.Code, result);
        }

        // PUT: api/Tours/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransportation(int id, TransportationUpdateRequest transportation)
        {
            if (ModelState.IsValid)
            {
                var result = _transportationServices.Update(id, transportation).Result;
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // POST: api/Tours
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TransportationResponse>> PostTransportation(TransportationCreateRequest transportation)
        {
            if (ModelState.IsValid)
            {
                var result = await _transportationServices.Create(transportation);
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Tours/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransportation(int id)
        {
            var result = _transportationServices.Delete(id).Result;
            return StatusCode((int)result.Status.Code, result);
        }
    }
}
