﻿using AutoMapper;
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
    public interface ITourSevices
    {
        BaseResponsePagingViewModel<TourResponse> GetAll(PagingRequest request, int destinationId);
        BaseResponseViewModel<TourResponse> Get(int id);
        Task<BaseResponseViewModel<TourResponse>> Update(int id, TourUpdateRequest request);
        Task<BaseResponseViewModel<TourResponse>> Create(TourCreateRequest request);
        Task<BaseResponseViewModel<TourResponse>> Delete(int id);

    }
    public class TourServices : ITourSevices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TourServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public BaseResponsePagingViewModel<TourResponse> GetAll(PagingRequest request, int destinationId)
        {

            var query = destinationId == 0 ? _unitOfWork.Repository<Tour>()
                                .GetAll()
                                .Include(d => d.TourDetails)
                                .Include(d => d.TourPrices)
                                .Include(d => d.TourGuides) : _unitOfWork.Repository<Tour>()
                                .GetAll()
                                .Include(d => d.TourDetails)
                                .Include(d => d.TourPrices)
                                .Include(d => d.TourGuides)
                                .Where(d => d.TourDetails.FirstOrDefault()!.DestinationId == destinationId);

            var tours = query
                                .ProjectTo<TourResponse>(_mapper.ConfigurationProvider)
                                .PagingQueryable(request.Page, request.PageSize, Common.Constants.LimitPaging, Common.Constants.DefaultPaging);

            return new BaseResponsePagingViewModel<TourResponse>
            {
                Metadata = new PagingsMetadata()
                {
                    Page = request.Page,
                    Size = request.PageSize,
                    Total = tours.Item1
                },
                Data = tours.Item2.ToList(),
            };
        }

        /*public async Task<BaseResponseViewModel<TourResponse>> Create(TourCreateRequest request)
        {
            try
            {
                var tour = _mapper.Map<Tour>(request);

                try
                {
                    await _unitOfWork.Repository<Tour>().InsertAsync(tour);
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                return new BaseResponseViewModel<TourResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.Created,
                        Message = "Created",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<TourResponse>(tour)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<TourResponse>
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

        }*/

        public BaseResponseViewModel<TourResponse> Get(int id)
        {
            var tour = GetById(id);
            if (tour == null)
            {
                return new BaseResponseViewModel<TourResponse>
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
            return new BaseResponseViewModel<TourResponse>
            {
                Status = new StatusViewModel
                {
                    Code = HttpStatusCode.OK,
                    Message = "OK",
                    IsSuccess = true
                },
                Data = _mapper.Map<TourResponse>(tour)
            };
        }

        private Tour GetById(int id)
        {
            var tour = _unitOfWork.Repository<Tour>().GetById(id).Result;
            var result = _mapper.Map<Tour>(tour);
            return result;
        }

        public async Task<BaseResponseViewModel<TourResponse>> Update(int id, TourUpdateRequest request)
        {
            var tour = GetById(id);
            //var tourResponse = _mapper.Map<TourResponse>(tour);
            if (tour == null)
            {
                return new BaseResponseViewModel<TourResponse>
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
                var updateTour = _mapper.Map<TourUpdateRequest, Tour>(request, tour);
                await _unitOfWork.Repository<Tour>().UpdateDetached(updateTour);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<TourResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Updated",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<TourResponse>(updateTour)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<TourResponse>
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

        public async Task<BaseResponseViewModel<TourResponse>> Create(TourCreateRequest request)
        {
            try
            {
                var tour = _mapper.Map<Tour>(request);

                try
                {
                    await _unitOfWork.Repository<Tour>().InsertAsync(tour);
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                return new BaseResponseViewModel<TourResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.Created,
                        Message = "Created",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<TourResponse>(tour)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<TourResponse>
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

        public async Task<BaseResponseViewModel<TourResponse>> Delete(int id)
        {
            var tour = GetById(id);
            if (tour == null)
            {
                return new BaseResponseViewModel<TourResponse>
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
                var bookings = tour.Bookings.ToList();
                foreach (var booking in bookings)
                {
                    _unitOfWork.Repository<Booking>().Delete(booking);
                }
                var tourDetails = tour.TourDetails.ToList();
                foreach (var tourDetail in tourDetails)
                {
                    _unitOfWork.Repository<TourDetail>().Delete(tourDetail);
                }
                var tourPrices = tour.TourPrices.ToList();
                foreach (var tourPrice in tourPrices)
                {
                    _unitOfWork.Repository<TourPrice>().Delete(tourPrice);
                }
                var tourGuides = tour.TourGuides.ToList();
                foreach (var tourGuide in tourGuides)
                {
                    _unitOfWork.Repository<TourGuide>().Delete(tourGuide);
                }

                _unitOfWork.Repository<Tour>().Delete(tour);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<TourResponse>
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
                return new BaseResponseViewModel<TourResponse>
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
