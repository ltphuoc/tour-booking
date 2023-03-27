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
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        public AuthController(IAuthServices loginServices, IAccountServices accountServices, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _authServices = loginServices;
            _accountServices = accountServices;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet("info")]
        public IActionResult CheckToken()
        {
            try
            {
                string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("You can't access to this endpoint!");
                }

                if (_jwtAuthenticationManager.ValidateJwtToken(token))
                {
                    var userId = _jwtAuthenticationManager.GetUserIdFromJwtToken(token);
                    var result = _accountServices.Get(int.Parse(userId!));
                    return Ok(result);
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

        [HttpPost("Google-Login")]
        public async Task<ActionResult<JwtAuthResponse>> LoginWithGoogle([FromBody] ExternalAuthRequest data)
        {
            if (ModelState.IsValid)
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
            else
            {
                return BadRequest(ModelState);
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
        [HttpPut("password")]
        public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var accountId = _jwtAuthenticationManager.GetUserIdFromJwtToken(token);

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
