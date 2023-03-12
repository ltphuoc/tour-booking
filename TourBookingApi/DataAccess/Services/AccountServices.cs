using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObject.Models;
using BusinessObject.UnitOfWork;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using NTQ.Sdk.Core.Utilities;
using System.Net;
using Constants = DataAccess.Common.Constants;

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
            var accounts = _unitOfWork.Repository<Account>().GetAll().ProjectTo<AccountResponse>(_mapper.ConfigurationProvider)
                .PagingQueryable(request.Page, request.PageSize, Common.Constants.LimitPaging, Common.Constants.DefaultPaging); ;
            return new BaseResponsePagingViewModel<AccountResponse>
            {
                Metadata = new PagingsMetadata()
                {
                    Page = request.Page,
                    Size = request.PageSize,
                    Total = accounts.Item1
                },
                Data = accounts.Item2.ToList(),
            };
        }

        public BaseResponseViewModel<AccountResponse> Get(int id)
        {
            var account = _unitOfWork.Repository<Account>().GetById(id);
            if (account.Result == null)
            {
                return new BaseResponseViewModel<AccountResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.NotFound,
                        Message = "Not Found",
                        IsSuccess = false
                    },
                    Data = null
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
                Data = _mapper.Map<AccountResponse>(account.Result)
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
                    },
                    Data = null
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
            catch (Exception ex)
            {
                return new BaseResponseViewModel<Account>
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
                        Message = "Not Found",
                        IsSuccess = false
                    },
                    Data = null
                };
            }
            try
            {
                account.Status = Constants.Status.INT_DELETED_STATUS;
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
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<AccountResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.BadRequest,
                        Message = "Bad Request\n " + ex.Message,
                        IsSuccess = false
                    },
                    Data = null
                };
            }
        }
        public async Task<BaseResponseViewModel<AccountResponse>> Create(AccountCreateRequest request)
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


        private Account GetByEmail(string email)
        {
            var account = _unitOfWork.Repository<Account>().GetWhere(x => x.Email.Equals(email)).Result.FirstOrDefault();
            var result = _mapper.Map<Account>(account);
            return result;
        }

        private Account GetById(int id)
        {
            var account = _unitOfWork.Repository<Account>().GetById(id).Result;
            var result = _mapper.Map<Account>(account);
            return result;
        }

    }


}
