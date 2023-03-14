﻿using System;
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
        public async Task<ActionResult<IEnumerable<TourGuide>>> GetTourGuides([FromQuery] PagingRequest request)
        {
            var result = _tourGuideServices.GetAll(request);
            return Ok(result);
        }

        // GET: api/TourGuides/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TourGuide>> GetTourGuide(int id)
        {
            var result = _tourGuideServices.Get(id);
            return Ok(result);
        }

        // PUT: api/TourGuides/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTourGuide(int id, TourGuideUpdateRequest tourGuide)
        {
            var result = _tourGuideServices.Update(id, tourGuide).Result;
            if (result.Status.Code != HttpStatusCode.OK)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST: api/TourGuides
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
