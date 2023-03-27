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
    public interface ITourPriceSevices
    {
        BaseResponsePagingViewModel<TourPriceResponse> GetAll(PagingRequest request);
        BaseResponseViewModel<TourPriceResponse> Get(int id);
        Task<BaseResponseViewModel<TourPriceResponse>> Update(int id, TourPriceUpdateRequest request);
        Task<BaseResponseViewModel<TourPriceResponse>> Create(TourPriceCreateRequest request);
        Task<BaseResponseViewModel<TourPriceResponse>> Delete(int id);

    }
    public class TourPriceServices : ITourPriceSevices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TourPriceServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public BaseResponsePagingViewModel<TourPriceResponse> GetAll(PagingRequest request)
        {
            var query = _unitOfWork.Repository<TourPrice>()
                                .GetAll()
                                .Include(d => d.Tour);

            var dynamicQuery = DynamicQueryHelper.ApplySearchSortAndPaging(query, request);

            var tourPrices = dynamicQuery.ProjectTo<TourPriceResponse>(_mapper.ConfigurationProvider);

            return new BaseResponsePagingViewModel<TourPriceResponse>
            {
                Metadata = new PagingsMetadata()
                {
                    Page = request.Page,
                    Size = request.PageSize,
                    Total = tourPrices.Count()
                },
                Data = tourPrices.ToList(),
            };
        }

        public BaseResponseViewModel<TourPriceResponse> Get(int id)
        {
            var tourPrice = GetById(id);
            if (tourPrice == null)
            {
                return new BaseResponseViewModel<TourPriceResponse>
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
            return new BaseResponseViewModel<TourPriceResponse>
            {
                Status = new StatusViewModel
                {
                    Code = HttpStatusCode.OK,
                    Message = "OK",
                    IsSuccess = true
                },
                Data = _mapper.Map<TourPriceResponse>(tourPrice)
            };
        }

        private TourPrice GetById(int id)
        {
            var tourPrice = _unitOfWork.Repository<TourPrice>().GetById(id).Result;
            var result = _mapper.Map<TourPrice>(tourPrice);
            return result;
        }

        public async Task<BaseResponseViewModel<TourPriceResponse>> Update(int id, TourPriceUpdateRequest request)
        {
            var tourPrice = GetById(id);
            //var tourPriceResponse = _mapper.Map<TourPrice>(tourPrice);
            if (tourPrice == null)
            {
                return new BaseResponseViewModel<TourPriceResponse>
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
                var updateTourPrice = _mapper.Map<TourPriceUpdateRequest, TourPrice>(request, tourPrice);
                await _unitOfWork.Repository<TourPrice>().UpdateDetached(tourPrice);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<TourPriceResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Updated",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<TourPriceResponse>(updateTourPrice)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<TourPriceResponse>
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

        public async Task<BaseResponseViewModel<TourPriceResponse>> Create(TourPriceCreateRequest request)
        {
            try
            {
                var tourPrice = _mapper.Map<TourPrice>(request);

                try
                {
                    await _unitOfWork.Repository<TourPrice>().InsertAsync(tourPrice);
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                return new BaseResponseViewModel<TourPriceResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.Created,
                        Message = "Created",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<TourPriceResponse>(tourPrice)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<TourPriceResponse>
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

        public async Task<BaseResponseViewModel<TourPriceResponse>> Delete(int id)
        {
            var tourPrice = GetById(id);
            if (tourPrice == null)
            {
                return new BaseResponseViewModel<TourPriceResponse>
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
                var tours = tourPrice.Tour;
                if (tours != null)
                {
                    _unitOfWork.Repository<Tour>().Delete(tours);
                }

                _unitOfWork.Repository<TourPrice>().Delete(tourPrice);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<TourPriceResponse>
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
                return new BaseResponseViewModel<TourPriceResponse>
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
