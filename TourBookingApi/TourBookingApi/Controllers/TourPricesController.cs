using BusinessObject.Models;
using DataAccess.DTO.Request;
using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;

namespace TourBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourPricesController : ControllerBase
    {
        private readonly ITourPriceSevices _tourPriceServices;

        public TourPricesController(ITourPriceSevices tourPriceServices)
        {
            _tourPriceServices = tourPriceServices;
        }

        // GET: api/TourPrices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TourPrice>>> GetTourPrices([FromQuery] PagingRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = _tourPriceServices.GetAll(request);
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // GET: api/TourPrices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TourPrice>> GetTourPrice(int id)
        {
            var result = _tourPriceServices.Get(id);
            return StatusCode((int)result.Status.Code, result);
        }

        // PUT: api/TourPrices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTourPrice(int id, TourPriceUpdateRequest tourPrice)
        {
            if (ModelState.IsValid)
            {
                var result = _tourPriceServices.Update(id, tourPrice).Result;
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // POST: api/TourPrices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TourPrice>> PostTourPrice(TourPriceCreateRequest tourPrice)
        {
            if (ModelState.IsValid)
            {
                var result = await _tourPriceServices.Create(tourPrice);
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/TourPrices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTourPrice(int id)
        {
            var result = _tourPriceServices.Delete(id).Result;
            return StatusCode((int)result.Status.Code, result);
        }
    }
}
