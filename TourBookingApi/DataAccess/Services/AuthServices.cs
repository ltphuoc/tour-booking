﻿using AutoMapper;
using BusinessObject.Models;
using BusinessObject.UnitOfWork;
using BusinessObjects.ResponseModels.Authentication;
using BusinessObjects.Services;
using Castle.Core.Internal;
using DataAccess.Common;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
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

        private async Task<Account> GetByEmail(string email)
        {
            var account = await _unitOfWork.Repository<Account>().GetAll().Where(x => x.Email.Equals(email)).FirstOrDefaultAsync();
            return account!;
        }

        public async Task<BaseResponseViewModel<JwtAuthResponse>> Login(LoginRequest request)
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
                    Data = null,
                };
            }

            //string role;

            //if (account.Role == Constants.Role.INT_ROLE_ADMIN)
            //{
            //    role = Constants.Role.STRING_ROLE_ADMIN;
            //}
            //else if (account.Role == Constants.Role.INT_ROLE_USER)
            //{
            //    role = Constants.Role.STRING_ROLE_USER;
            //}
            //else
            //{
            //    role = Constants.Role.STRING_ROLE_USER;
            //}

            // generate token
            var newToken = JwtAuthenticationManager.GenerateJwtToken(account.Email, account.Role.ToString(), account.Id.ToString(), _configuration);

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

        public async Task<BaseResponseViewModel<AccountResponse>> ChangePassword(ChangePasswordRequest request)
        {
            var account = GetByEmail(request.Email).Result;
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