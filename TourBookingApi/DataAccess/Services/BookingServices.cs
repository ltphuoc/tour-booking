using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObject.Models;
using BusinessObject.UnitOfWork;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Microsoft.EntityFrameworkCore;
using NTQ.Sdk.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public interface IBookingSevices
    {
        BaseResponsePagingViewModel<BookingResponse> GetAll(PagingRequest request);
        BaseResponseViewModel<BookingResponse> Get(int id);
        Task<BaseResponseViewModel<BookingResponse>> Update(int id, BookingUpdateRequest request);
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
        public BaseResponsePagingViewModel<BookingResponse> GetAll(PagingRequest request)
        {
            var bookings = _unitOfWork.Repository<Booking>()
                                .GetAll()
                                .Include(d => d.Customer)
                                .Include(d => d.Tour)
                                .ProjectTo<BookingResponse>(_mapper.ConfigurationProvider)
                                .PagingQueryable(request.Page, request.PageSize, Common.Constants.LimitPaging, Common.Constants.DefaultPaging);

            //var bookingDTO = _mapper.Map<List<BookingResponse>>(bookings.Item2.ToList());

            return new BaseResponsePagingViewModel<BookingResponse>
            {
                Metadata = new PagingsMetadata()
                {
                    Page = request.Page,
                    Size = request.PageSize,
                    Total = bookings.Item1
                },
                Data = bookings.Item2.ToList(),
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
            var booking = GetById(id);
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

        private Booking GetById(int id)
        {
            var booking = _unitOfWork.Repository<Booking>().GetById(id).Result;
            var result = _mapper.Map<Booking>(booking);
            return result;
        }

        public async Task<BaseResponseViewModel<BookingResponse>> Update(int id, BookingUpdateRequest request)
        {
            var booking = GetById(id);
            //var bookingResponse = _mapper.Map<Booking>(booking);
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

        public async Task<BaseResponseViewModel<BookingResponse>> Create(BookingCreateRequest request)
        {
            try
            {
                var booking = _mapper.Map<Booking>(request);
                try
                {
                    await _unitOfWork.Repository<Booking>().InsertAsync(booking);
                    await _unitOfWork.CommitAsync();

                    //var book = 

                    PaymentCreateRequest paymentRequest = new PaymentCreateRequest();
                    paymentRequest.BookingId = booking.Id;
                    paymentRequest.PaymentDate = booking.BookingDate;
                    paymentRequest.Status = 3;
                    paymentRequest.PaymentCode = booking.Tour.TourName + booking.Tour.TourDuration.ToString();
                    paymentRequest.PaymentAmount = booking.TotalPrice;
                    paymentRequest.PaymentMethod = "1";

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
            var booking = GetById(id);
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

