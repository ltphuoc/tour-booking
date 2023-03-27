using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObject.Models;
using BusinessObject.UnitOfWork;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using DataAccess.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DataAccess.Services
{
    public interface IPaymentSevices
    {
        BaseResponsePagingViewModel<PaymentResponse> GetAll(PagingRequest request);
        BaseResponseViewModel<PaymentResponse> Get(int id);
        Task<BaseResponseViewModel<PaymentResponse>> UpdateImage(int id, string image);
        Task<BaseResponseViewModel<PaymentResponse>> UpdateStatus(int id, int status);

        Task<BaseResponseViewModel<PaymentResponse>> Create(PaymentCreateRequest request);
        Task<BaseResponseViewModel<PaymentResponse>> Delete(int id);

    }

    public class PaymentServices : IPaymentSevices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public BaseResponsePagingViewModel<PaymentResponse> GetAll(PagingRequest request)
        {
            var query = _unitOfWork.Repository<Payment>()
                                .GetAll()
                                .Include(d => d.Booking);

            var dynamicQuery = DynamicQueryHelper.ApplySearchSortAndPaging(query, request);

            var payments = dynamicQuery.ProjectTo<PaymentResponse>(_mapper.ConfigurationProvider);


            return new BaseResponsePagingViewModel<PaymentResponse>
            {
                Metadata = new PagingsMetadata()
                {
                    Page = request.Page,
                    Size = request.PageSize,
                    Total = payments.Count()
                },
                Data = payments.ToList(),
            };
        }

        public BaseResponseViewModel<PaymentResponse> Get(int id)
        {
            var payment = GetById(id);
            if (payment == null)
            {
                return new BaseResponseViewModel<PaymentResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.NotFound,
                        Message = "Not Found",
                        IsSuccess = false
                    }
                };
            }
            return new BaseResponseViewModel<PaymentResponse>
            {
                Status = new StatusViewModel
                {
                    Code = HttpStatusCode.OK,
                    Message = "OK",
                    IsSuccess = true
                },
                Data = _mapper.Map<PaymentResponse>(payment)
            };
        }

        private Payment GetById(int id)
        {
            var payment = _unitOfWork.Repository<Payment>().GetById(id).Result;
            var result = _mapper.Map<Payment>(payment);
            return result;
        }

        public async Task<BaseResponseViewModel<PaymentResponse>> UpdateImage(int id, string image)
        {
            var payment = GetById(id);

            if (payment == null)
            {
                return new BaseResponseViewModel<PaymentResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.NotFound,
                        Message = "Not Found",
                        IsSuccess = false
                    },
                    Data = null!
                };
            }
            try
            {
                //var updatePayment = _mapper.Map<PaymentUpdateRequest, Payment>(request, payment);
                payment.PaymentImage = image;
                await _unitOfWork.Repository<Payment>().UpdateDetached(payment);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<PaymentResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Updated",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<PaymentResponse>(payment)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<PaymentResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.BadRequest,
                        Message = "Bad Request",
                        IsSuccess = false
                    },
                    Data = null!
                };
            }
        }

        public async Task<BaseResponseViewModel<PaymentResponse>> UpdateStatus(int id, int status)
        {
            var payment = GetById(id);

            if (payment == null)
            {
                return new BaseResponseViewModel<PaymentResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.NotFound,
                        Message = "Not Found",
                        IsSuccess = false
                    },
                    Data = null!
                };
            }
            try
            {
                //var updatePayment = _mapper.Map<PaymentUpdateRequest, Payment>(request, payment);
                payment.Status = status;
                await _unitOfWork.Repository<Payment>().UpdateDetached(payment);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<PaymentResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Updated",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<PaymentResponse>(payment)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<PaymentResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.BadRequest,
                        Message = "Bad Request",
                        IsSuccess = false
                    },
                    Data = null!
                };
            }
        }

        public async Task<BaseResponseViewModel<PaymentResponse>> Create(PaymentCreateRequest request)
        {
            try
            {
                var payment = _mapper.Map<Payment>(request);

                try
                {
                    await _unitOfWork.Repository<Payment>().InsertAsync(payment);
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                return new BaseResponseViewModel<PaymentResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.Created,
                        Message = "Created",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<PaymentResponse>(payment)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<PaymentResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.BadRequest,
                        Message = "Bad Request",
                        IsSuccess = false
                    },
                    Data = null!
                };
            }
        }

        public async Task<BaseResponseViewModel<PaymentResponse>> Delete(int id)
        {
            var payment = GetById(id);
            if (payment == null)
            {
                return new BaseResponseViewModel<PaymentResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.NotFound,
                        Message = "Not Found",
                        IsSuccess = false
                    },
                    Data = null!
                };
            }
            try
            {
                var bookings = payment.Booking;
                if (bookings != null)
                {
                    _unitOfWork.Repository<Booking>().Delete(bookings);
                }

                _unitOfWork.Repository<Payment>().Delete(payment);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<PaymentResponse>
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
                return new BaseResponseViewModel<PaymentResponse>
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
    }
}
