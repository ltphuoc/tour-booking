using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using DataAccess.Services;
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
            var result = _destinationImageServices.GetAll(request, destinationId);
            return StatusCode((int)result.Status.Code, result);
        }

        // GET: api/Destinations/5
        [HttpGet("{id}")]
        public ActionResult<DestinationImageResponse> GetDestinationImage(int id)
        {
            var result = _destinationImageServices.Get(id);
            return StatusCode((int)result.Status.Code, result);
        }

        // PUT: api/Destinations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDestinationImage(int id, DestinationImageUpdateRequest destination)
        {
            var result = _destinationImageServices.Update(id, destination).Result;
            return StatusCode((int)result.Status.Code, result);
        }

        // POST: api/Destinations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DestinationImageResponse>> PostDestination(DestinationImageCreateRequest destination)
        {
            var result = await _destinationImageServices.Create(destination);
            return StatusCode((int)result.Status.Code, result);
        }

        // DELETE: api/Destinations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDestination(int id)
        {
            var result = _destinationImageServices.Delete(id).Result;
            return StatusCode((int)result.Status.Code, result);
        }
    }
}
