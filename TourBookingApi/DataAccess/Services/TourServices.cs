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
    public interface ITourSevices
    {
        BaseResponsePagingViewModel<Tour> GetAll(PagingRequest request);
        BaseResponseViewModel<Tour> Get(int id);
        Task<BaseResponseViewModel<Tour>> Update(int id, TourUpdateRequest request);
        Task<BaseResponseViewModel<Tour>> Create(TourCreateRequest request);
        Task<BaseResponseViewModel<Tour>> Delete(int id);

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
        public BaseResponsePagingViewModel<Tour> GetAll(PagingRequest request)
        {
            var tours = _unitOfWork.Repository<Tour>()
                                .GetAll()
                                /*.Include(d => d.DestinationImages)*/
                                /*.ProjectTo<DestinationResponse>(_mapper.ConfigurationProvider)*/
                                .PagingQueryable(request.Page, request.PageSize, Common.Constants.LimitPaging, Common.Constants.DefaultPaging);

            return new BaseResponsePagingViewModel<Tour>
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

        public async Task<BaseResponseViewModel<Tour>> Create(TourRequest request)
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

                return new BaseResponseViewModel<Tour>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.Created,
                        Message = "Created",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<Tour>(tour)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<Tour>
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

        public BaseResponseViewModel<Tour> Get(int id)
        {
            var tour = GetById(id);
            if (tour == null)
            {
                return new BaseResponseViewModel<Tour>
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
            return new BaseResponseViewModel<Tour>
            {
                Status = new StatusViewModel
                {
                    Code = HttpStatusCode.OK,
                    Message = "OK",
                    IsSuccess = true
                },
                Data = _mapper.Map<Tour>(tour)
            };
        }

        private Tour GetById(int id)
        {
            var tour = _unitOfWork.Repository<Tour>().GetById(id).Result;
            var result = _mapper.Map<Tour>(tour);
            return result;
        }

        public async Task<BaseResponseViewModel<Tour>> Update(int id, TourUpdateRequest request)
        {
            var tour = GetById(id);
            var tourResponse = _mapper.Map<Tour>(tour);
            if (tour == null)
            {
                return new BaseResponseViewModel<Tour>
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
                var updateDestination = _mapper.Map<TourUpdateRequest, Tour>(request, tourResponse);
                await _unitOfWork.Repository<Tour>().UpdateDetached(updateDestination);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<Tour>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Updated",
                        IsSuccess = true
                    },
                    Data = updateDestination
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<Tour>
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

        public async Task<BaseResponseViewModel<Tour>> Create(TourCreateRequest request)
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

                return new BaseResponseViewModel<Tour>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.Created,
                        Message = "Created",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<Tour>(tour)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<Tour>
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

        public async Task<BaseResponseViewModel<Tour>> Delete(int id)
        {
            var tour = GetById(id);
            if (tour == null)
            {
                return new BaseResponseViewModel<Tour>
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

                return new BaseResponseViewModel<Tour>
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
                return new BaseResponseViewModel<Tour>
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
