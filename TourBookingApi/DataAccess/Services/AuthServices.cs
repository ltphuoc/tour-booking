using AutoMapper;
using BusinessObject.Models;
using BusinessObject.UnitOfWork;
using BusinessObjects.ResponseModels.Authentication;
using BusinessObjects.Services;
using Castle.Core.Internal;
using DataAccess.Common;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using FirebaseAdmin.Auth;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace DataAccess.Services
{
    public interface IAuthServices
    {
        Task<BaseResponseViewModel<AccountResponse>> LoginWithGoogle(ExternalAuthRequest data);
        Task<BaseResponseViewModel<JwtAuthResponse>> Login(LoginRequest request);
        Task<BaseResponseViewModel<AccountResponse>> Register(RegisterRequest request);
        Task<BaseResponseViewModel<AccountResponse>> ChangePassword(ChangePasswordRequest request);
    }

    public class AuthServices : IAuthServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAccountServices _accountServices;
        private readonly IConfiguration _configuration;

        public AuthServices(IUnitOfWork unitOfWork, IMapper mapper, IAccountServices accountServices, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accountServices = accountServices;
            _configuration = configuration;
        }

        public async Task<BaseResponseViewModel<AccountResponse>> LoginWithGoogle(ExternalAuthRequest data)
        {
            var auth = FirebaseAuth.DefaultInstance;
            FirebaseToken decodeToken = await auth.VerifyIdTokenAsync(data.IdToken);
            UserRecord userRecord = await auth.GetUserAsync(decodeToken.Uid);

            var account = await GetAccountByEmail(userRecord.Email);

            if (account.Data == null)
            {
                AccountCreateRequest model = new AccountCreateRequest()
                {
                    FirstName = userRecord.DisplayName,
                    LastName = userRecord.DisplayName,
                    Email = userRecord.Email,
                    Phone = userRecord.PhoneNumber,
                    Avatar = userRecord.PhotoUrl

                };
                account = await _accountServices.Create(model);
            }
            else if (account.Status.Code == HttpStatusCode.NotFound)
            {
                return account;
            }
            return new BaseResponseViewModel<AccountResponse>()
            {
                Status = new StatusViewModel()
                {
                    Message = "Success",
                    IsSuccess = true,
                    Code = HttpStatusCode.OK,
                },
                Data = _mapper.Map<AccountResponse>(account.Data)
            };
        }

        public async Task<BaseResponseViewModel<AccountResponse>> GetAccountByEmail(string email)
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

        private Account GetByEmail(string email)
        {
            var account = _unitOfWork.Repository<Account>().GetWhere(x => x.Email.Equals(email)).Result.FirstOrDefault();
            var result = _mapper.Map<Account>(account);
            return result;
        }

        public async Task<BaseResponseViewModel<JwtAuthResponse>> Login(LoginRequest request)
        {
            var account = GetByEmail(request.Email);
            if (account == null)
            {
                return new BaseResponseViewModel<JwtAuthResponse>
                {
                    Status = new StatusViewModel()
                    {
                        Message = "Account not existed!",
                        IsSuccess = false,
                        Code = HttpStatusCode.NotFound,
                    },
                    Data = new JwtAuthResponse
                    {
                        UserName = request.Email,
                        Token = null,
                        ExpiresIn = 0
                    }
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
                    Data = new JwtAuthResponse
                    {
                        UserName = request.Email,
                        Token = null,
                        ExpiresIn = 0
                    }
                };
            }

            string role;

            if (account.Role == Common.Constants.Role.INT_ROLE_ADMIN)
            {
                role = Common.Constants.Role.STRING_ROLE_ADMIN;
            }
            else if (account.Role == Common.Constants.Role.INT_ROLE_USER)
            {
                role = Common.Constants.Role.STRING_ROLE_USER;
            }
            else
            {
                role = null;
            }
            // generate token
            var newToken = JwtAuthenticationManager.GenerateJwtToken(string.IsNullOrEmpty(account.Email) ? "" : account.Email, role!, account.Id, _configuration);

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
                    UserName = string.IsNullOrEmpty(account.Email) ? "" : account.Email,
                }
            };
        }

        public async Task<BaseResponseViewModel<AccountResponse>> Register(RegisterRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email))
                {
                    throw new Exception();
                }

                var account = _mapper.Map<Account>(request);

                account.Status = Constants.Status.INT_ACTIVE_STATUS;
                account.Role = Constants.Role.INT_ROLE_USER;

                try
                {
                    await _unitOfWork.Repository<Account>().InsertAsync(account);
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
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
            catch (Exception ex)
            {
                return new BaseResponseViewModel<AccountResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.BadRequest,
                        Message = "Bad Request",
                        IsSuccess = false
                    },
                    Data = null
                };
            }

        }

        public async Task<BaseResponseViewModel<AccountResponse>> ChangePassword(ChangePasswordRequest request)
        {
            var account = GetByEmail(request.Email);
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
    }
}
