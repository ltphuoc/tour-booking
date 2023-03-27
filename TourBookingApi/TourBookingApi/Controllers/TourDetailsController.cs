using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using DataAccess.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TourBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourDetailsController : ControllerBase
    {
        private readonly ITourDetailSevices _tourdetailServices;

        public TourDetailsController(ITourDetailSevices tourdetailServices)
        {
            _tourdetailServices = tourdetailServices;
        }

        // GET: api/TourDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TourDetailResponse>>> GetTourDetails([FromQuery] PagingRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = _tourdetailServices.GetAll(request);
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // GET: api/TourDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TourDetailResponse>> GetTourDetail(int id)
        {
            var result = _tourdetailServices.Get(id);
            return Ok(result);
        }

        // PUT: api/TourDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTourDetail(int id, TourDetailUpdateRequest tourdetail)
        {
            if (ModelState.IsValid)
            {
                var result = _tourdetailServices.Update(id, tourdetail).Result;
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // POST: api/TourDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult<TourDetailResponse>> PostTourDetail(TourDetailCreateRequest tourdetail)
        {
            if (ModelState.IsValid)
            {
                var result = await _tourdetailServices.Create(tourdetail);
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/TourDetails/5
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTourDetail(int id)
        {
            var result = _tourdetailServices.Delete(id).Result;
            return StatusCode((int)result.Status.Code, result);
        }
    }
}
