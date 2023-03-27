using AutoMapper;
using BusinessObject.Models;
using BusinessObject.UnitOfWork;
using BusinessObjects.ResponseModels.Authentication;
using BusinessObjects.Services;
using DataAccess.Common;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace DataAccess.Services
{
    public interface IAuthServices
    {
        Task<BaseResponseViewModel<JwtAuthResponse>> LoginWithGoogle(ExternalAuthRequest data);
        BaseResponseViewModel<JwtAuthResponse> Login(LoginRequest request);
        BaseResponseViewModel<JwtAuthResponse> LoginAdmin(LoginAdminRequest request);
        Task<BaseResponseViewModel<AccountResponse>> Register(RegisterRequest request);
        Task<BaseResponseViewModel<AccountResponse>> ChangePassword(ChangePasswordRequest request, int accountId);
    }

    public class AuthServices : IAuthServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAccountServices _accountServices;
        private readonly IConfiguration _configuration;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        public AuthServices(IUnitOfWork unitOfWork, IMapper mapper, IAccountServices accountServices, IConfiguration configuration, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accountServices = accountServices;
            _configuration = configuration;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        public async Task<BaseResponseViewModel<JwtAuthResponse>> LoginWithGoogle(ExternalAuthRequest data)
        {
            var auth = FirebaseAuth.DefaultInstance;
            FirebaseToken decodeToken = await auth.VerifyIdTokenAsync(data.IdToken);
            UserRecord userRecord = await auth.GetUserAsync(decodeToken.Uid);

            var accountDb = GetAccountByEmail(userRecord.Email);

            if (accountDb.Data == null)
            {
                AccountCreateRequest model = new AccountCreateRequest()
                {
                    FirstName = userRecord.DisplayName,
                    LastName = userRecord.DisplayName,
                    Email = userRecord.Email,
                    Phone = userRecord.PhoneNumber ?? "",
                    Avatar = userRecord.PhotoUrl,
                    Address = "",
                    Password = ""
                };

                accountDb = await _accountServices.Create(model);
            }
            else if (accountDb.Status.Code == HttpStatusCode.NotFound)
            {
                return new BaseResponseViewModel<JwtAuthResponse>
                {
                    Status = new StatusViewModel()
                    {
                        Message = "Login success",
                        IsSuccess = false,
                        Code = HttpStatusCode.NotFound,
                    },
                }; ;
            }

            var account = _mapper.Map<AccountResponse>(accountDb.Data);

            var newToken = _jwtAuthenticationManager.GenerateJwtToken(account.Email, account.Role.ToString(), account.Id.ToString());

            return new BaseResponseViewModel<JwtAuthResponse>
            {
                Status = new StatusViewModel()
                {
                    Message = "Login success",
                    IsSuccess = true,
                    Code = HttpStatusCode.OK,
                },
                Data = new JwtAuthResponse
                {
                    Token = newToken,
                    UserName = account.Email,
                }
            };
        }

        public BaseResponseViewModel<AccountResponse> GetAccountByEmail(string email)
        {
            Account? account;
            account = _unitOfWork.Repository<Account>()
                .GetAll()
                .Where(x => x.Email.Contains(email))
                .FirstOrDefault();

            if (account != null && account.Status != 1)
            {
                return new BaseResponseViewModel<AccountResponse>()
                {
                    Status = new StatusViewModel()
                    {
                        Message = "Account was banned",
                        IsSuccess = false,
                        Code = HttpStatusCode.BadRequest,
                    },
                    Data = _mapper.Map<AccountResponse>(account)
                };
            }
            return new BaseResponseViewModel<AccountResponse>()
            {
                Status = new StatusViewModel()
                {
                    Message = "Success",
                    IsSuccess = true,
                    Code = HttpStatusCode.OK
                },
                Data = _mapper.Map<AccountResponse>(account)
            };
        }

        private async Task<Account> GetByEmail(string email)
        {
            var account = await _unitOfWork.Repository<Account>().GetAll().Where(x => x.Email.Equals(email)).FirstOrDefaultAsync();
            return account!;
        }

        public BaseResponseViewModel<JwtAuthResponse> Login(LoginRequest request)
        {
            var account = GetByEmail(request.Email).Result;
            if (account == null || account.Status != Constants.Status.INT_ACTIVE_STATUS)
            {
                return new BaseResponseViewModel<JwtAuthResponse>
                {
                    Status = new StatusViewModel()
                    {
                        Message = "Account not existed!",
                        IsSuccess = false,
                        Code = HttpStatusCode.NotFound,
                    },
                };
            }
            else if (!account.Password.Equals(request.Password))
            {
                return new BaseResponseViewModel<JwtAuthResponse>
                {
                    Status = new StatusViewModel()
                    {
                        Message = "Wrong password",
                        IsSuccess = false,
                        Code = HttpStatusCode.BadRequest,
                    },
                };
            }

            // generate token
            var newToken = _jwtAuthenticationManager.GenerateJwtToken(account.Email, account.Role.ToString(), account.Id.ToString());

            return new BaseResponseViewModel<JwtAuthResponse>
            {
                Status = new StatusViewModel()
                {
                    Message = "Login success",
                    IsSuccess = true,
                    Code = HttpStatusCode.OK,
                },
                Data = new JwtAuthResponse
                {
                    Token = newToken,
                    UserName = account.Email,
                }
            };
        }

        public async Task<BaseResponseViewModel<AccountResponse>> Register(RegisterRequest request)
        {
            // Check if the request object is null or the email is empty
            if (request == null || string.IsNullOrEmpty(request.Email))
            {
                return new BaseResponseViewModel<AccountResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.BadRequest,
                        Message = "Bad Request: Request object cannot be null and email cannot be empty",
                        IsSuccess = false
                    }

                };
            }

            // Check if the email is already taken
            var check = await GetByEmail(request.Email);

            if (check != null)
            {
                return new BaseResponseViewModel<AccountResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.BadRequest,
                        Message = "Email already taken",
                        IsSuccess = false
                    },
                    Data = null
                };
            }

            var account = _mapper.Map<Account>(request);

            // Check if the password is already encoded
            if (!Utils.IsPasswordEncoded(account.Password))
            {
                // Encode the password
                var hashedPassword = Utils.EncodePassword(account.Password);
                account.Password = hashedPassword;
            }

            account.Status = Constants.Status.INT_ACTIVE_STATUS;
            account.Role = Constants.Role.INT_ROLE_USER;

            try
            {
                await _unitOfWork.Repository<Account>().InsertAsync(account);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<AccountResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.BadRequest,
                        Message = ex.Message,
                        IsSuccess = false
                    },
                    Data = null
                };
            }

            return new BaseResponseViewModel<AccountResponse>
            {
                Status = new StatusViewModel
                {
                    Code = HttpStatusCode.Created,
                    Message = "Created",
                    IsSuccess = true
                },
                Data = _mapper.Map<AccountResponse>(account)
            };
        }

        public async Task<BaseResponseViewModel<AccountResponse>> ChangePassword(ChangePasswordRequest request, int accountId)
        {
            //var account = GetByEmail(request.Email).Result;
            var account = _unitOfWork.Repository<Account>().GetById(accountId)?.Result;
            if (account == null)
            {
                return new BaseResponseViewModel<AccountResponse>
                {
                    Status = new StatusViewModel()
                    {
                        Message = "Account not existed!",
                        IsSuccess = false,
                        Code = HttpStatusCode.NotFound,
                    },
                    Data = null,
                };
            }
            if (!account.Password.Equals(request.Password))
            {
                return new BaseResponseViewModel<AccountResponse>
                {
                    Status = new StatusViewModel()
                    {
                        Message = "Your password was not true for this account",
                        IsSuccess = false,
                        Code = HttpStatusCode.BadRequest,
                    },
                    Data = null,

                };
            }
            account.Password = request.NewPassword;

            await _unitOfWork.Repository<Account>().UpdateDetached(account);
            await _unitOfWork.CommitAsync();

            return new BaseResponseViewModel<AccountResponse>
            {
                Status = new StatusViewModel()
                {
                    Message = "Password changed",
                    IsSuccess = true,
                    Code = HttpStatusCode.OK,
                },
                Data = null,
            };
        }

        public BaseResponseViewModel<JwtAuthResponse> LoginAdmin(LoginAdminRequest request)
        {
            var username = _configuration["Admin:UserName"];
            var password = _configuration["Admin:Password"];
            if (request.UserName != username || request.Password != password)
            {
                return new BaseResponseViewModel<JwtAuthResponse>
                {
                    Status = new StatusViewModel()
                    {
                        Message = "Username or password not correct!",
                        IsSuccess = false,
                        Code = HttpStatusCode.NotFound,
                    },
                };
            }
            // generate token
            var newToken = _jwtAuthenticationManager.GenerateJwtToken(username, Constants.Role.INT_ROLE_ADMIN.ToString());

            return new BaseResponseViewModel<JwtAuthResponse>
            {
                Status = new StatusViewModel()
                {
                    Message = "Login success",
                    IsSuccess = true,
                    Code = HttpStatusCode.OK,
                },
                Data = new JwtAuthResponse
                {
                    Token = newToken,
                    UserName = username,
                }
            };
        }
    }
}
