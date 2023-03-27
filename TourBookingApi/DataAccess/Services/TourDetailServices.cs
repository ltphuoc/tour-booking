using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObject.Models;
using BusinessObject.UnitOfWork;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using DataAccess.Helpers;
using Microsoft.EntityFrameworkCore;
using NTQ.Sdk.Core.Utilities;
using System.Net;

namespace DataAccess.Services
{
    public interface ITourDetailSevices
    {
        BaseResponsePagingViewModel<TourDetailResponse> GetAll(PagingRequest request);
        BaseResponseViewModel<TourDetailResponse> Get(int id);
        Task<BaseResponseViewModel<TourDetailResponse>> Update(int id, TourDetailUpdateRequest request);
        Task<BaseResponseViewModel<TourDetailResponse>> Create(TourDetailCreateRequest request);
        Task<BaseResponseViewModel<TourDetailResponse>> Delete(int id);

    }

    public class TourDetailServices : ITourDetailSevices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TourDetailServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public BaseResponsePagingViewModel<TourDetailResponse> GetAll(PagingRequest request)
        {
            var query = _unitOfWork.Repository<TourDetail>()
                                .GetAll()
                                .Include(d => d.Destination);

            var dynamicQuery = DynamicQueryHelper.ApplySearchSortAndPaging(query, request);

            var tourdetails = dynamicQuery.ProjectTo<TourDetailResponse>(_mapper.ConfigurationProvider);

            return new BaseResponsePagingViewModel<TourDetailResponse>
            {
                Metadata = new PagingsMetadata()
                {
                    Page = request.Page,
                    Size = request.PageSize,
                    Total = tourdetails.Count()
                },
                Data = tourdetails.ToList(),
            };
        }

        public async Task<BaseResponseViewModel<TourDetailResponse>> Create(TourDetailRequest request)
        {
            try
            {
                var tourdetail = _mapper.Map<TourDetail>(request);

                try
                {
                    await _unitOfWork.Repository<TourDetail>().InsertAsync(tourdetail);
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                return new BaseResponseViewModel<TourDetailResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.Created,
                        Message = "Created",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<TourDetailResponse>(tourdetail)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<TourDetailResponse>
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

        public BaseResponseViewModel<TourDetailResponse> Get(int id)
        {
            var tourdetail = GetById(id);
            if (tourdetail == null)
            {
                return new BaseResponseViewModel<TourDetailResponse>
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
            return new BaseResponseViewModel<TourDetailResponse>
            {
                Status = new StatusViewModel
                {
                    Code = HttpStatusCode.OK,
                    Message = "OK",
                    IsSuccess = true
                },
                Data = _mapper.Map<TourDetailResponse>(tourdetail)
            };
        }

        private TourDetail GetById(int id)
        {
            var tourdetail = _unitOfWork.Repository<TourDetail>().GetById(id).Result;
            var result = _mapper.Map<TourDetail>(tourdetail);
            return result;
        }

        public async Task<BaseResponseViewModel<TourDetailResponse>> Update(int id, TourDetailUpdateRequest request)
        {
            var tourdetail = GetById(id);
            //var tourDetailResponse = _mapper.Map<TourDetailResponse>(tourdetail);
            if (tourdetail == null)
            {
                return new BaseResponseViewModel<TourDetailResponse>
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
                var updateTourDetail = _mapper.Map<TourDetailUpdateRequest, TourDetail>(request, tourdetail);
                await _unitOfWork.Repository<TourDetail>().UpdateDetached(tourdetail);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<TourDetailResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Updated",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<TourDetailResponse>(updateTourDetail)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<TourDetailResponse>
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

        public async Task<BaseResponseViewModel<TourDetailResponse>> Create(TourDetailCreateRequest request)
        {
            try
            {
                var tourdetail = _mapper.Map<TourDetail>(request);

                try
                {
                    await _unitOfWork.Repository<TourDetail>().InsertAsync(tourdetail);
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                return new BaseResponseViewModel<TourDetailResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.Created,
                        Message = "Created",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<TourDetailResponse>(tourdetail)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<TourDetailResponse>
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

        public async Task<BaseResponseViewModel<TourDetailResponse>> Delete(int id)
        {
            var tourdetail = GetById(id);
            if (tourdetail == null)
            {
                return new BaseResponseViewModel<TourDetailResponse>
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
                var destinations = tourdetail.Destination;
                if (destinations != null)
                {
                    _unitOfWork.Repository<Destination>().Delete(destinations);
                }
                var tours = tourdetail.Tour;
                if (tours != null)
                {
                    _unitOfWork.Repository<Tour>().Delete(tours);
                }
                var transports = tourdetail.Transportation;
                if (transports != null)
                {
                    _unitOfWork.Repository<Transportation>().Delete(transports);
                }

                _unitOfWork.Repository<TourDetail>().Delete(tourdetail);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<TourDetailResponse>
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
                return new BaseResponseViewModel<TourDetailResponse>
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
