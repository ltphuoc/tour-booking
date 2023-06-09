﻿using DataAccess.DTO.Request;
using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;

namespace TourBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult SendEmail(EmailRequest request)
        {
            _emailService.SendEmail(request);
            return Ok();
        }
    }
}
