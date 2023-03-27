using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using DataAccess.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TourBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationImagesController : ControllerBase
    {

        private readonly IDestinationImageServices _destinationImageServices;

        public DestinationImagesController(IDestinationImageServices destinationImageServices)
        {
            _destinationImageServices = destinationImageServices;
        }

        // GET: api/Destinations
        [HttpGet]
        public ActionResult<IEnumerable<DestinationImageResponse>> GetDestinationImages([FromQuery] PagingRequest request, [FromQuery] int destinationId)
        {
            if (ModelState.IsValid)
            {
                var result = _destinationImageServices.GetAll(request, destinationId);

                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // GET: api/Destinations/5
        [HttpGet("{id}")]
        public ActionResult<DestinationImageResponse> GetDestinationImage(int id)
        {
            if (ModelState.IsValid)
            {
                var result = _destinationImageServices.Get(id);
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // PUT: api/Destinations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDestinationImage(int id, DestinationImageUpdateRequest destination)
        {
            if (ModelState.IsValid)
            {
                var result = _destinationImageServices.Update(id, destination).Result;
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // POST: api/Destinations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult<DestinationImageResponse>> PostDestination(DestinationImageCreateRequest destination)
        {
            if (ModelState.IsValid)
            {
                var result = await _destinationImageServices.Create(destination);
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Destinations/5
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDestination(int id)
        {
            if (ModelState.IsValid)
            {
                var result = _destinationImageServices.Delete(id).Result;
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
