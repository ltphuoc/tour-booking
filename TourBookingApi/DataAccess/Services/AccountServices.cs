using AutoMapper;
using BusinessObject.Models;
using BusinessObject.UnitOfWork;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Microsoft.EntityFrameworkCore;
using NTQ.Sdk.Core.Utilities;
using System.Net;
using DataAccess.Common;

namespace DataAccess.Services
{

    public interface IAccountServices
    {
        BaseResponsePagingViewModel<AccountResponse> GetAll(PagingRequest request);
        BaseResponseViewModel<AccountResponse> Get(int id);
        Task<BaseResponseViewModel<Account>> Update(int id, AccountUpdateResquest resquest);
        Task<BaseResponseViewModel<AccountResponse>> Delete(int id);
        Task<BaseResponseViewModel<AccountResponse>> Create(AccountCreateRequest request);
    }

    public class AccountServices : IAccountServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public AccountServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public BaseResponsePagingViewModel<AccountResponse> GetAll(PagingRequest request)
        {
            try
            {
                var query = _unitOfWork.Repository<Account>().GetAll();
                var accounts = query.Select(x => _mapper.Map<AccountResponse>(x))
                    .PagingQueryable(request.Page, request.PageSize, Constants.LimitPaging, Constants.DefaultPaging);

                return new BaseResponsePagingViewModel<AccountResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.OK,
                        Message = "OK",
                        IsSuccess = true
                    },
                    Metadata = new PagingsMetadata()
                    {
                        Page = request.Page,
                        Size = request.PageSize,
                        Total = accounts.Item1
                    },
                    Data = accounts.Item2.ToList(),
                };
            }
            catch
            {
                return new BaseResponsePagingViewModel<AccountResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.InternalServerError,
                        Message = "Server Error",
                        IsSuccess = true
                    },
                };
            }
        }

        public BaseResponseViewModel<AccountResponse> Get(int id)
        {
            var account = GetById(id);
            if (account == null)
            {
                return new BaseResponseViewModel<AccountResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.NotFound,
                        Message = "Not Found",
                        IsSuccess = false
                    },
                };
            }
            return new BaseResponseViewModel<AccountResponse>
            {
                Status = new StatusViewModel
                {
                    Code = HttpStatusCode.OK,
                    Message = "OK",
                    IsSuccess = true
                },
                Data = _mapper.Map<AccountResponse>(account)
            };
        }

        public async Task<BaseResponseViewModel<Account>> Update(int id, AccountUpdateResquest request)
        {
            var account = GetById(id);
            if (account == null)
            {
                return new BaseResponseViewModel<Account>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.NotFound,
                        Message = "Not Found",
                        IsSuccess = false
                    }
                };
            }
            try
            {
                var updateAccount = _mapper.Map<AccountUpdateResquest, Account>(request, account);
                await _unitOfWork.Repository<Account>().UpdateDetached(updateAccount);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<Account>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Updated",
                        IsSuccess = true
                    },
                    Data = updateAccount
                };
            }
            catch (DbUpdateConcurrencyException)
            {
                return new BaseResponseViewModel<Account>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.Conflict,
                        Message = "Conflict",
                        IsSuccess = false
                    }
                };
            }
            catch (DbUpdateException)
            {
                return new BaseResponseViewModel<Account>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.BadRequest,
                        Message = "Bad Request",
                        IsSuccess = false
                    }
                };
            }

        }

        public async Task<BaseResponseViewModel<AccountResponse>> Delete(int id)
        {
            var account = GetById(id);
            if (account == null)
            {
                return new BaseResponseViewModel<AccountResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.NotFound,
                        Message = "Account Not Found",
                        IsSuccess = false
                    },
                };
            }

            account.Status = Constants.Status.INT_DELETED_STATUS;

            try
            {
                await _unitOfWork.Repository<Account>().UpdateDetached(account);
                await _unitOfWork.CommitAsync();
                return new BaseResponseViewModel<AccountResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.OK,
                        Message = "OK",
                        IsSuccess = true
                    },
                };
            }
            catch (DbUpdateConcurrencyException)
            {
                return new BaseResponseViewModel<AccountResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.Conflict,
                        Message = "Conflict",
                        IsSuccess = false
                    },
                };
            }
            catch (DbUpdateException)
            {
                return new BaseResponseViewModel<AccountResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.BadRequest,
                        Message = "Bad Request",
                        IsSuccess = false
                    },
                };
            }
        }

        public async Task<BaseResponseViewModel<AccountResponse>> Create(AccountCreateRequest request)
        {
            var accountExisted = GetByEmail(request.Email);

            if (accountExisted != null)
            {
                return new BaseResponseViewModel<AccountResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.Conflict,
                        Message = "An account with this email already exists.",
                        IsSuccess = false
                    }
                };
            }

            var account = _mapper.Map<Account>(request);
            account.Status = Constants.Status.INT_ACTIVE_STATUS;
            account.Role = Constants.Role.INT_ROLE_USER;

            try
            {
                await _unitOfWork.Repository<Account>().InsertAsync(account);
                await _unitOfWork.CommitAsync();

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
            catch
            {
                return new BaseResponseViewModel<AccountResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.BadRequest,
                        Message = "Bad Request",
                        IsSuccess = false
                    },
                };
            }

        }

        private Account? GetByEmail(string email)
        {
            var account = _unitOfWork.Repository<Account>().GetWhere(x => x.Email.Equals(email)).Result.FirstOrDefault();
            return _mapper.Map<Account>(account);
        }

        private Account? GetById(int id)
        {
            var account = _unitOfWork.Repository<Account>().GetById(id)?.Result;
            return _mapper.Map<Account>(account);
        }

    }


}
