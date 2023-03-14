using DataAccess.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessObjects.Services
{
    public class JwtAuthenticationManager
    {
        private readonly IAccountServices _accountServices;

        public JwtAuthenticationManager(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }

        //public BaseResponseViewModel<JwtAuthResponse> Authenticate(LoginModel loginModel)
        //{
        //    try
        //    {
        //        //Validate for user name and password
        //        var account = _accountServices.Get(x => x.Email.Equals(loginModel.Email)).FirstOrDefault();
        //        if (account == null)
        //        {
        //            return new BaseResponse<JwtAuthResponse>
        //            {
        //                Code = "404",
        //                Message = "Account not existed!",
        //                Data = new JwtAuthResponse()
        //                {
        //                    Token = null,
        //                    UserName = null
        //                }
        //            };
        //        }
        //        else if (!account.Password.Equals(loginModel.Password))
        //        {
        //            return new BaseResponse<JwtAuthResponse>
        //            {
        //                Code = "400",
        //                Message = "Wrong password!",
        //            };
        //        }

        //        var expiresTimeStamp = DateTime.Now.AddMinutes(Contants.JWT_TOKEN_VALIDITY_MINS);
        //        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        //        var tokenKey = Encoding.ASCII.GetBytes(Contants.JWT_SECURITY_KEY);

        //        var securityTokenDescriptor = new SecurityTokenDescriptor
        //        {
        //            Subject = new System.Security.Claims.ClaimsIdentity(new List<Claim>
        //            {
        //                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //                new Claim(ClaimTypes.Name, loginModel.Email),
        //                new Claim(ClaimTypes.NameIdentifier, loginModel.Password),
        //            }),
        //            Expires = expiresTimeStamp,
        //            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature),
        //        };

        //        var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
        //        var token = jwtSecurityTokenHandler.WriteToken(securityToken);

        //        return new BaseResponse<JwtAuthResponse>
        //        {
        //            Code = "200",
        //            Message = "Login success!",
        //            Data = new JwtAuthResponse()
        //            {
        //                Token = token,
        //                UserName = loginModel.Email,
        //                ExpiresIn = (int)expiresTimeStamp.Subtract(DateTime.Now).TotalSeconds
        //            }
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new BaseResponse<JwtAuthResponse>
        //        {
        //            Code = "400",
        //            Message = "Something wrong || " + ex.Message,
        //        };
        //    }

        //    //var responseAccount = _mapper.CreateMapper().Map<AccountViewModel>(account);
        //}
        public static string GenerateJwtToken(string username, string roles, int? userId, IConfiguration configuration)
        {
            var tokenConfig = configuration.GetSection("Token");
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenConfig["SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var permClaims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };
            if (roles != null)
            {
                permClaims.Add(new Claim(ClaimTypes.Role, roles));
            }

            var token = new JwtSecurityToken(tokenConfig["Issuer"],
                tokenConfig["Issuer"],
                permClaims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
