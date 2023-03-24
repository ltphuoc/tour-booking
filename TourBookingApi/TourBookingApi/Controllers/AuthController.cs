using BusinessObjects.ResponseModels.Authentication;
using BusinessObjects.Services;
using DataAccess.DTO.Request;
using DataAccess.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TourBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;
        private readonly IAccountServices _accountServices;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthServices loginServices, IAccountServices accountServices, IConfiguration configuration)
        {
            _authServices = loginServices;
            _accountServices = accountServices;
            _configuration = configuration;
        }

        [HttpPost("Token")]
        public IActionResult CheckToken()
        {
            try
            {
                string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (token == "")
                {
                    return Unauthorized("You can't access to this endpoint!");
                }

                if (JwtAuthenticationManager.ValidateJwtToken(token, _configuration))
                {
                    return Ok("You have access to this endpoint!");
                }
                else
                {
                    return Forbid();
                }
            }
            catch (Exception)
            {
                return Unauthorized("You can't access to this endpoint!");
            }
        }

        [HttpPost("LoginWithGoogle")]
        public async Task<ActionResult<JwtAuthResponse>> LoginWithGoogle([FromBody] ExternalAuthRequest data)
        {
            try
            {
                var result = await _authServices.LoginWithGoogle(data);
                return StatusCode((int)result.Status.Code, result);
            }
            catch
            {
                return BadRequest("Invalid External Authentication.");
            }
        }

        [HttpPost("Login")]
        public ActionResult Login([FromBody] LoginRequest loginRequest)
        {
            if (ModelState.IsValid)
            {
                var result = _authServices.Login(loginRequest);
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }


        }

        [HttpPost("admin/Login")]
        public ActionResult LoginAdmin([FromBody] LoginAdminRequest loginRequest)
        {
            var result = _authServices.LoginAdmin(loginRequest);
            return StatusCode((int)result.Status.Code, result);
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Post([FromBody] RegisterRequest request)
        {
            var result = _authServices.Register(request).Result;
            return StatusCode((int)result.Status.Code, result);
        }

        [Authorize(Policy = "UserOnly")]
        [HttpPut("ChangePassword")]
        public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var accountId = JwtAuthenticationManager.GetUserIdFromJwtToken(token, _configuration);

            if (ModelState.IsValid)
            {
                if (request.NewPassword.Equals(request.Password))
                {
                    return BadRequest("Old password and new password must be diffent");
                }

                var result = _authServices.ChangePassword(request, int.Parse(accountId)).Result;
                return StatusCode((int)result.Status.Code, result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
