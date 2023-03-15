using AutoMapper;
using BusinessObject.Models;
using BusinessObject.UnitOfWork;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using NTQ.Sdk.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public interface IPaymentSevices
    {
        BaseResponsePagingViewModel<Payment> GetAll(PagingRequest request);
        BaseResponseViewModel<Payment> Get(int id);
        Task<BaseResponseViewModel<Payment>> Update(int id, PaymentUpdateRequest request);
        Task<BaseResponseViewModel<Payment>> Create(PaymentCreateRequest request);
        Task<BaseResponseViewModel<Payment>> Delete(int id);

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
        public BaseResponsePagingViewModel<Payment> GetAll(PagingRequest request)
        {
            var payments = _unitOfWork.Repository<Payment>()
                                .GetAll()
                                /*.Include(d => d.DestinationImages)*/
                                /*.ProjectTo<DestinationResponse>(_mapper.ConfigurationProvider)*/
                                .PagingQueryable(request.Page, request.PageSize, Common.Constants.LimitPaging, Common.Constants.DefaultPaging);

            return new BaseResponsePagingViewModel<Payment>
            {
                Metadata = new PagingsMetadata()
                {
                    Page = request.Page,
                    Size = request.PageSize,
                    Total = payments.Item1
                },
                Data = payments.Item2.ToList(),
            };
        }

        public async Task<BaseResponseViewModel<Payment>> Create(PaymentRequest request)
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

                return new BaseResponseViewModel<Payment>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.Created,
                        Message = "Created",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<Payment>(payment)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<Payment>
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

        public BaseResponseViewModel<Payment> Get(int id)
        {
            var payment = GetById(id);
            if (payment == null)
            {
                return new BaseResponseViewModel<Payment>
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
            return new BaseResponseViewModel<Payment>
            {
                Status = new StatusViewModel
                {
                    Code = HttpStatusCode.OK,
                    Message = "OK",
                    IsSuccess = true
                },
                Data = _mapper.Map<Payment>(payment)
            };
        }

        private Payment GetById(int id)
        {
            var payment = _unitOfWork.Repository<Payment>().GetById(id).Result;
            var result = _mapper.Map<Payment>(payment);
            return result;
        }

        public async Task<BaseResponseViewModel<Payment>> Update(int id, PaymentUpdateRequest request)
        {
            var payment = GetById(id);
            var paymentResponse = _mapper.Map<Payment>(payment);
            if (payment == null)
            {
                return new BaseResponseViewModel<Payment>
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
                var updatePayment = _mapper.Map<PaymentUpdateRequest, Payment>(request, paymentResponse);
                await _unitOfWork.Repository<Payment>().UpdateDetached(updatePayment);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<Payment>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Updated",
                        IsSuccess = true
                    },
                    Data = updatePayment
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<Payment>
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

        public async Task<BaseResponseViewModel<Payment>> Create(PaymentCreateRequest request)
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

                return new BaseResponseViewModel<Payment>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.Created,
                        Message = "Created",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<Payment>(payment)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<Payment>
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

        public async Task<BaseResponseViewModel<Payment>> Delete(int id)
        {
            var payment = GetById(id);
            if (payment == null)
            {
                return new BaseResponseViewModel<Payment>
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
                var bookings = payment.Booking;
                if (bookings != null)
                {
                    _unitOfWork.Repository<Booking>().Delete(bookings);
                }

                _unitOfWork.Repository<Payment>().Delete(payment);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<Payment>
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
                return new BaseResponseViewModel<Payment>
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
