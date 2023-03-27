using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObject.Models;
using BusinessObject.UnitOfWork;
using Castle.Core.Internal;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using DataAccess.Helpers;
using Microsoft.EntityFrameworkCore;
using NTQ.Sdk.Core.Utilities;
using System.Net;
using static DataAccess.Common.Constants;
using static DataAccess.Helpers.Enum;

namespace DataAccess.Services
{
    public interface IBookingSevices
    {
        BaseResponsePagingViewModel<BookingResponse> GetAll(PagingRequest request, string userId = "");
        BaseResponseViewModel<BookingResponse> Get(int id);
        Task<BaseResponseViewModel<BookingResponse>> Update(int id, BookingUpdateRequest request);
        Task<BaseResponseViewModel<BookingResponse>> UpdatePaymentStatus(int id, int paymentId, int status);
        Task<BaseResponseViewModel<BookingResponse>> Create(BookingCreateRequest request);
        Task<BaseResponseViewModel<BookingResponse>> Delete(int id);

    }
    public class BookingServices : IBookingSevices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public BaseResponsePagingViewModel<BookingResponse> GetAll(PagingRequest request, string userId = "")
        {
            int userIdInt = 0;
            if (!string.IsNullOrEmpty(userId.Trim()))
            {
                userIdInt = int.Parse(userId.Trim());
            }

            var query = _unitOfWork.Repository<Booking>()
                .GetAll()
                .Where(b => string.IsNullOrEmpty(userId.Trim()) || b.CustomerId == userIdInt)
                .Include(d => d.Customer)
                .Include(d => d.Tour);

            var dynamicQuery = DynamicQueryHelper.ApplySearchSortAndPaging(query, request);

            var bookings = dynamicQuery.ProjectTo<BookingResponse>(_mapper.ConfigurationProvider);

            return new BaseResponsePagingViewModel<BookingResponse>
            {
                Metadata = new PagingsMetadata()
                {
                    Page = request.Page,
                    Size = request.PageSize,
                    Total = bookings.Count()
                },
                Data = bookings.ToList(),
            };
        }

        public async Task<BaseResponseViewModel<Booking>> Create(BookingRequest request)
        {
            try
            {
                var booking = _mapper.Map<Booking>(request);

                try
                {
                    await _unitOfWork.Repository<Booking>().InsertAsync(booking);
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                return new BaseResponseViewModel<Booking>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.Created,
                        Message = "Created",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<Booking>(booking)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<Booking>
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

        public BaseResponseViewModel<BookingResponse> Get(int id)
        {
            var booking = GetById(id).Result;
            if (booking == null)
            {
                return new BaseResponseViewModel<BookingResponse>
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
            return new BaseResponseViewModel<BookingResponse>
            {
                Status = new StatusViewModel
                {
                    Code = HttpStatusCode.OK,
                    Message = "OK",
                    IsSuccess = true
                },
                Data = _mapper.Map<BookingResponse>(booking)
            };
        }

        private async Task<Booking> GetById(int id)
        {
            var booking = await _unitOfWork.Repository<Booking>().GetById(id);
            var result = _mapper.Map<Booking>(booking);
            return result;
        }

        public async Task<BaseResponseViewModel<BookingResponse>> Update(int id, BookingUpdateRequest request)
        {
            var booking = GetById(id).Result;
            if (booking == null)
            {
                return new BaseResponseViewModel<BookingResponse>
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
                var updateBooking = _mapper.Map<BookingUpdateRequest, Booking>(request, booking);
                await _unitOfWork.Repository<Booking>().UpdateDetached(updateBooking);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<BookingResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Updated",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<BookingResponse>(updateBooking)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<BookingResponse>
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
        public async Task<BaseResponseViewModel<BookingResponse>> UpdatePaymentStatuss(int id, int status, int a)
        {
            var payment = _unitOfWork.Repository<Booking>()
                .GetById(id).Result.Payments.Where(p => p.Status.HasValue &&
                                                p.Status != (int)PaymentStatusEnum.Expired).SingleOrDefault();

            if (payment == null)
            {
                return new BaseResponseViewModel<BookingResponse>
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
                payment.Status = status;
                await _unitOfWork.Repository<Payment>().UpdateDetached(payment);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<BookingResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Updated",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<BookingResponse>(payment.Booking)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<BookingResponse>
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

        public async Task<BaseResponseViewModel<BookingResponse>> UpdatePaymentStatus(int id, int paymentId, int status)
        {
            var booking = await _unitOfWork.Repository<Booking>().GetById(id);
            if (booking == null)
            {
                return new BaseResponseViewModel<BookingResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.NotFound,
                        Message = "Booking not found",
                        IsSuccess = false
                    },
                    Data = null!
                };
            }

            var payment = booking.Payments.FirstOrDefault(p => p.Id == paymentId && p.Status.HasValue && p.Status != (int)PaymentStatusEnum.Expired);
            if (payment == null)
            {
                return new BaseResponseViewModel<BookingResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.NotFound,
                        Message = "Payment not found",
                        IsSuccess = false
                    },
                    Data = null!
                };
            }

            //if (payment.PaymentType != PaymentTypeEnum.Full)
            //{
            //    return new BaseResponseViewModel<BookingResponse>
            //    {
            //        Status = new StatusViewModel
            //        {
            //            Code = HttpStatusCode.BadRequest,
            //            Message = "Invalid payment type",
            //            IsSuccess = false
            //        },
            //        Data = null!
            //    };
            //}

            payment.Status = status;
            await _unitOfWork.Repository<Payment>().UpdateDetached(payment);
            await _unitOfWork.CommitAsync();

            var bookingResponse = _mapper.Map<BookingResponse>(booking);
            return new BaseResponseViewModel<BookingResponse>
            {
                Status = new StatusViewModel
                {
                    Code = HttpStatusCode.OK,
                    Message = "Payment status updated",
                    IsSuccess = true
                },
                Data = bookingResponse
            };
        }


        //public async Task<BaseResponseViewModel<BookingResponse>> UpdatePaymentStatus(int id, int paymentId, int status)
        //{
        //    var booking = await _unitOfWork.Repository<Booking>().GetById(id);
        //    if (booking == null)
        //    {
        //        return new BaseResponseViewModel<BookingResponse>
        //        {
        //            Status = new StatusViewModel
        //            {
        //                Code = HttpStatusCode.NotFound,
        //                Message = "Not Found",
        //                IsSuccess = false
        //            },
        //            Data = null!
        //        };
        //    }

        //    var payment = booking.Payments.FirstOrDefault(p => p.Status.HasValue && p.Status != (int)PaymentStatusEnum.Expired);
        //    if (payment == null)
        //    {
        //        return new BaseResponseViewModel<BookingResponse>
        //        {
        //            Status = new StatusViewModel
        //            {
        //                Code = HttpStatusCode.NotFound,
        //                Message = "Payment not found",
        //                IsSuccess = false
        //            },
        //            Data = null!
        //        };
        //    }

        //    // Check payment type and update payment status accordingly
        //    //if (payment.Type == PaymentTypeEnum.Half)
        //    //{
        //    //    if (status == (int)PaymentStatusEnum.Paid)
        //    //    {
        //    //        payment.Status = status;
        //    //        await _unitOfWork.Repository<Payment>().UpdateDetached(payment);
        //    //        await _unitOfWork.CommitAsync();

        //    //        // Return the booking with updated payment status
        //    //        var bookingResponse = _mapper.Map<BookingResponse>(booking);
        //    //        bookingResponse.IsFullyPaid = false;
        //    //        return new BaseResponseViewModel<BookingResponse>
        //    //        {
        //    //            Status = new StatusViewModel
        //    //            {
        //    //                Code = HttpStatusCode.OK,
        //    //                Message = "Payment status updated",
        //    //                IsSuccess = true
        //    //            },
        //    //            Data = bookingResponse
        //    //        };
        //    //    }
        //    //    else
        //    //    {
        //    //        return new BaseResponseViewModel<BookingResponse>
        //    //        {
        //    //            Status = new StatusViewModel
        //    //            {
        //    //                Code = HttpStatusCode.BadRequest,
        //    //                Message = "Invalid payment status",
        //    //                IsSuccess = false
        //    //            },
        //    //            Data = null!
        //    //        };
        //    //    }
        //    //}
        //    //else if (payment.Type == PaymentTypeEnum.Full)
        //    //{
        //    // Check if all payments for this booking have been made
        //    var totalPaidAmount = booking.Payments.Sum(p => p.PaymentAmount);
        //    if (totalPaidAmount == booking.TotalPrice)
        //    {
        //        // Update the payment status and return the booking with updated payment status
        //        payment.Status = status;
        //        await _unitOfWork.Repository<Payment>().UpdateDetached(payment);
        //        await _unitOfWork.CommitAsync();

        //        var bookingResponse = _mapper.Map<BookingResponse>(booking);
        //        //bookingResponse.IsFullyPaid = true;
        //        return new BaseResponseViewModel<BookingResponse>
        //        {
        //            Status = new StatusViewModel
        //            {
        //                Code = HttpStatusCode.OK,
        //                Message = "Payment status updated",
        //                IsSuccess = true
        //            },
        //            Data = bookingResponse
        //        };
        //    }
        //    //else
        //    //{
        //    //    return new BaseResponseViewModel<BookingResponse>
        //    //    {
        //    //        Status = new StatusViewModel
        //    //        {
        //    //            Code = HttpStatusCode.BadRequest,
        //    //            Message = "Booking has not been fully paid",
        //    //            IsSuccess = false
        //    //        },
        //    //        Data = null!
        //    //    };
        //    //}
        //    //}
        //    else
        //    {
        //        return new BaseResponseViewModel<BookingResponse>
        //        {
        //            Status = new StatusViewModel
        //            {
        //                Code = HttpStatusCode.BadRequest,
        //                Message = "Invalid payment type",
        //                IsSuccess = false
        //            },
        //            Data = null!
        //        };
        //    }
        //}


        public async Task<BaseResponseViewModel<BookingResponse>> Create(BookingCreateRequest request)
        {
            try
            {
                var booking = _mapper.Map<Booking>(request);
                try
                {
                    await _unitOfWork.Repository<Booking>().InsertAsync(booking);
                    await _unitOfWork.CommitAsync();

                    PaymentCreateRequest paymentRequest = new PaymentCreateRequest();
                    paymentRequest.BookingId = booking.Id;
                    paymentRequest.PaymentDate = booking.BookingDate;
                    paymentRequest.Status = (int?)PaymentStatusEnum.Pending;
                    paymentRequest.PaymentCode = "";
                    paymentRequest.PaymentAmount = booking.TotalPrice;
                    paymentRequest.PaymentMethod = request.PaymentMethod;

                    var payment = _mapper.Map<Payment>(paymentRequest);

                    await _unitOfWork.Repository<Payment>().InsertAsync(payment);
                    await _unitOfWork.CommitAsync();

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                return new BaseResponseViewModel<BookingResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.Created,
                        Message = "Created",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<BookingResponse>(booking)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<BookingResponse>
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

        public async Task<BaseResponseViewModel<BookingResponse>> Delete(int id)
        {
            var booking = GetById(id).Result;
            if (booking == null)
            {
                return new BaseResponseViewModel<BookingResponse>
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
                var tours = booking.Tour;
                if (tours != null)
                {
                    _unitOfWork.Repository<Tour>().Delete(tours);
                }
                var accounts = booking.Customer;
                if (accounts != null)
                {
                    _unitOfWork.Repository<Account>().Delete(accounts);
                }
                var payments = booking.Payments.ToList();
                foreach (var payment in payments)
                {
                    _unitOfWork.Repository<Payment>().Delete(payment);
                }

                _unitOfWork.Repository<Booking>().Delete(booking);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<BookingResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.OK,
                        Message = "OK",
                        IsSuccess = true
                    },
                    Data = null!
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<BookingResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.BadRequest,
                        Message = "Bad Request\n " + ex.Message,
                        IsSuccess = false
                    },
                    Data = null!
                };
            }
        }
    }
}

