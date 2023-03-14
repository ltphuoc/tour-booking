using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using DataAccess.Services;
using DataAccess.DTO.Request;
using System.Net;

namespace TourBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourPricesController : ControllerBase
    {
        private readonly ITourPriceSevices _tourPriceServices;
        private readonly TourBookingContext _context;

        public TourPricesController(TourBookingContext context, ITourPriceSevices tourPriceServices)
        {
            _context = context;
            _tourPriceServices = tourPriceServices;
        }

        // GET: api/TourPrices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TourPrice>>> GetTourPrices([FromQuery] PagingRequest request)
        {
            var result = _tourPriceServices.GetAll(request);
            return Ok(result);
        }

        // GET: api/TourPrices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TourPrice>> GetTourPrice(int id)
        {
            var result = _tourPriceServices.Get(id);
            return Ok(result);
        }

        // PUT: api/TourPrices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTourPrice(int id, TourPriceUpdateRequest tourPrice)
        {
            var result = _tourPriceServices.Update(id, tourPrice).Result;
            if (result.Status.Code != HttpStatusCode.OK)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST: api/TourPrices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TourPrice>> PostTourPrice(TourPriceCreateRequest tourPrice)
        {
            var result = await _tourPriceServices.Create(tourPrice);
            if (result.Status.Code != HttpStatusCode.Created)
            {
                return BadRequest(result);
            }
            return Created("", result);
        }

        // DELETE: api/TourPrices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTourPrice(int id)
        {
            var result = _tourPriceServices.Delete(id).Result;
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
