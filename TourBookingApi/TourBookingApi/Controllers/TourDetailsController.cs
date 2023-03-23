using BusinessObject.Models;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace TourBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourDetailsController : ControllerBase
    {
        private readonly ITourDetailSevices _tourdetailServices;
        private readonly TourBookingContext _context;

        public TourDetailsController(TourBookingContext context, ITourDetailSevices tourdetailServices)
        {
            _context = context;
            _tourdetailServices = tourdetailServices;
        }

        // GET: api/TourDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TourDetailResponse>>> GetTourDetails([FromQuery] PagingRequest request)
        {
            var result = _tourdetailServices.GetAll(request);
            return Ok(result);
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTourDetail(int id, TourDetailUpdateRequest tourdetail)
        {
            var result = _tourdetailServices.Update(id, tourdetail).Result;
            if (result.Status.Code != HttpStatusCode.OK)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST: api/TourDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TourDetailResponse>> PostTourDetail(TourDetailCreateRequest tourdetail)
        {
            var result = await _tourdetailServices.Create(tourdetail);
            if (result.Status.Code != HttpStatusCode.Created)
            {
                return BadRequest(result);
            }
            return Created("", result);
        }

        // DELETE: api/TourDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTourDetail(int id)
        {
            var result = _tourdetailServices.Delete(id).Result;
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
