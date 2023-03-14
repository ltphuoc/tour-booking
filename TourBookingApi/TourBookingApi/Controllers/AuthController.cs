using BusinessObject.Models;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using DataAccess.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace TourBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;
        private readonly IAccountServices _accountServices;

        public AuthController(IAuthServices loginServices, IAccountServices accountServices)
        {

            _authServices = loginServices;
            _accountServices = accountServices;
        }

        [HttpPost("LoginWithGoogle")]
        public async Task<ActionResult<AccountResponse>> LoginWithGoogle([FromBody] ExternalAuthRequest data)
        {
            try
            {
                var result = await _authServices.LoginWithGoogle(data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Invalid External Authentication.");
            }
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginRequest loginRequest)
        {

            if (string.IsNullOrEmpty(loginRequest.Email.Trim()) || string.IsNullOrEmpty(loginRequest.Password.Trim()))
            {
                return BadRequest("Email and password not allow null");
            }
            //var authResult = _jwtAuthen.Authenticate(loginRequest);
            //if (authResult.Code != "200")
            //{
            //    return BadRequest(authResult);
            //}
            var result = _authServices.Login(loginRequest).Result;
            if (!result.Status.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Post([FromBody] RegisterRequest request)
        {
            var response = _authServices.Register(request);
            if (response.Result.Status.Code != HttpStatusCode.Created)
            {
                return BadRequest(response.Result);
            }
            return Created("", response.Result);
        }

        [HttpPut("change-password")]
        public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.NewPassword) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Email, password and NewPassword not allow null or empty");
            }
            if (request.NewPassword.Equals(request.Password))
            {
                return BadRequest("Old password and new password must be diffent");
            }
            request.Email.Trim();
            request.Password.Trim();
            request.NewPassword.Trim();
            var result = _authServices.ChangePassword(request).Result;
            if (result.Status.Code != HttpStatusCode.OK)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
