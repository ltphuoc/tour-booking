﻿using AutoMapper;
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
    public interface IBookingSevices
    {
        BaseResponsePagingViewModel<Booking> GetAll(PagingRequest request);
        BaseResponseViewModel<Booking> Get(int id);
        Task<BaseResponseViewModel<Booking>> Update(int id, BookingUpdateRequest request);
        Task<BaseResponseViewModel<Booking>> Create(BookingCreateRequest request);
        Task<BaseResponseViewModel<Booking>> Delete(int id);

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
        public BaseResponsePagingViewModel<Booking> GetAll(PagingRequest request)
        {
            var bookings = _unitOfWork.Repository<Booking>()
                                .GetAll()
                                /*.Include(d => d.DestinationImages)*/
                                /*.ProjectTo<DestinationResponse>(_mapper.ConfigurationProvider)*/
                                .PagingQueryable(request.Page, request.PageSize, Common.Constants.LimitPaging, Common.Constants.DefaultPaging);

            return new BaseResponsePagingViewModel<Booking>
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
                    Data = null
                };
            }

        }

        public BaseResponseViewModel<Booking> Get(int id)
        {
            var booking = GetById(id);
            if (booking == null)
            {
                return new BaseResponseViewModel<Booking>
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
            return new BaseResponseViewModel<Booking>
            {
                Status = new StatusViewModel
                {
                    Code = HttpStatusCode.OK,
                    Message = "OK",
                    IsSuccess = true
                },
                Data = _mapper.Map<Booking>(booking)
            };
        }

        private Booking GetById(int id)
        {
            var booking = _unitOfWork.Repository<Booking>().GetById(id).Result;
            var result = _mapper.Map<Booking>(booking);
            return result;
        }

        public async Task<BaseResponseViewModel<Booking>> Update(int id, BookingUpdateRequest request)
        {
            var booking = GetById(id);
            var bookingResponse = _mapper.Map<Booking>(booking);
            if (booking == null)
            {
                return new BaseResponseViewModel<Booking>
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
                var updateBooking = _mapper.Map<BookingUpdateRequest, Booking>(request, bookingResponse);
                await _unitOfWork.Repository<Booking>().UpdateDetached(updateBooking);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<Booking>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Updated",
                        IsSuccess = true
                    },
                    Data = updateBooking
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
                    Data = null
                };
            }
        }

        public async Task<BaseResponseViewModel<Booking>> Create(BookingCreateRequest request)
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
                    Data = null
                };
            }
        }

        public async Task<BaseResponseViewModel<Booking>> Delete(int id)
        {
            var booking = GetById(id);
            if (booking == null)
            {
                return new BaseResponseViewModel<Booking>
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

                return new BaseResponseViewModel<Booking>
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
                return new BaseResponseViewModel<Booking>
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
