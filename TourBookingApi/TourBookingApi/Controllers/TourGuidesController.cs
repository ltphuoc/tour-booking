using BusinessObject.Models;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using DataAccess.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace TourBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourGuidesController : ControllerBase
    {
        private readonly ITourGuideSevices _tourGuideServices;
        private readonly TourBookingContext _context;

        public TourGuidesController(TourBookingContext context, ITourGuideSevices tourGuideServices)
        {
            _context = context;
            _tourGuideServices = tourGuideServices;
        }

        // GET: api/TourGuides
        [HttpGet]
        public async Task<ActionResult<List<TourGuideReponse>>> GetTourGuides([FromQuery] PagingRequest request)
        {
            var result = _tourGuideServices.GetAll(request);
            return Ok(result);
        }

        // GET: api/TourGuides/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TourGuideReponse>> GetTourGuide(int id)
        {
            var result = _tourGuideServices.Get(id);
            return Ok(result);
        }

        // PUT: api/TourGuides/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTourGuide(int id, TourGuideUpdateRequest tourGuide)
        {
            var result = _tourGuideServices.Update(id, tourGuide).Result;
            if (result.Status.Code != HttpStatusCode.NoContent)
            {
                return BadRequest(result);
            }
            return NoContent();
        }

        // POST: api/TourGuides
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult<TourGuide>> PostTourGuide(TourGuideCreateRequest tourGuide)
        {
            var result = await _tourGuideServices.Create(tourGuide);
            if (result.Status.Code != HttpStatusCode.Created)
            {
                return BadRequest(result);
            }
            return Created("", result);
        }

        // DELETE: api/TourGuides/5
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTourGuide(int id)
        {
            var result = _tourGuideServices.Delete(id).Result;
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
