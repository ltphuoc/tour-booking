using BusinessObject.Models;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using DataAccess.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TourBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourGuidesController : ControllerBase
    {
        private readonly ITourGuideSevices _tourGuideServices;
        public TourGuidesController(ITourGuideSevices tourGuideServices)
        {
            _tourGuideServices = tourGuideServices;
        }

        // GET: api/TourGuides
        [HttpGet]
        public async Task<ActionResult<List<TourGuideReponse>>> GetTourGuides([FromQuery] PagingRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = _tourGuideServices.GetAll(request);
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // GET: api/TourGuides/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TourGuideReponse>> GetTourGuide(int id)
        {
            var result = _tourGuideServices.Get(id);
            return StatusCode((int)result.Status.Code, result);
        }

        // PUT: api/TourGuides/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTourGuide(int id, TourGuideUpdateRequest tourGuide)
        {
            if (ModelState.IsValid)
            {
                var result = _tourGuideServices.Update(id, tourGuide).Result;
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // POST: api/TourGuides
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult<TourGuide>> PostTourGuide(TourGuideCreateRequest tourGuide)
        {
            if (ModelState.IsValid)
            {
                var result = await _tourGuideServices.Create(tourGuide);
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/TourGuides/5
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTourGuide(int id)
        {
            var result = _tourGuideServices.Delete(id).Result;
            return StatusCode((int)result.Status.Code, result);
        }
    }
}
