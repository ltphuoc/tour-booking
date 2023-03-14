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
    public interface ITourDetailSevices
    {
        BaseResponsePagingViewModel<TourDetail> GetAll(PagingRequest request);
        BaseResponseViewModel<TourDetail> Get(int id);
        Task<BaseResponseViewModel<TourDetail>> Update(int id, TourDetailUpdateRequest request);
        Task<BaseResponseViewModel<TourDetail>> Create(TourDetailCreateRequest request);
        Task<BaseResponseViewModel<TourDetail>> Delete(int id);

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
        public BaseResponsePagingViewModel<TourDetail> GetAll(PagingRequest request)
        {
            var tourdetails = _unitOfWork.Repository<TourDetail>()
                                .GetAll()
                                /*.Include(d => d.DestinationImages)*/
                                /*.ProjectTo<DestinationResponse>(_mapper.ConfigurationProvider)*/
                                .PagingQueryable(request.Page, request.PageSize, Common.Constants.LimitPaging, Common.Constants.DefaultPaging);

            return new BaseResponsePagingViewModel<TourDetail>
            {
                Metadata = new PagingsMetadata()
                {
                    Page = request.Page,
                    Size = request.PageSize,
                    Total = tourdetails.Item1
                },
                Data = tourdetails.Item2.ToList(),
            };
        }

        public async Task<BaseResponseViewModel<TourDetail>> Create(TourDetailRequest request)
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

                return new BaseResponseViewModel<TourDetail>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.Created,
                        Message = "Created",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<TourDetail>(tourdetail)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<TourDetail>
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

        public BaseResponseViewModel<TourDetail> Get(int id)
        {
            var tourdetail = GetById(id);
            if (tourdetail == null)
            {
                return new BaseResponseViewModel<TourDetail>
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
            return new BaseResponseViewModel<TourDetail>
            {
                Status = new StatusViewModel
                {
                    Code = HttpStatusCode.OK,
                    Message = "OK",
                    IsSuccess = true
                },
                Data = _mapper.Map<TourDetail>(tourdetail)
            };
        }

        private TourDetail GetById(int id)
        {
            var tourdetail = _unitOfWork.Repository<TourDetail>().GetById(id).Result;
            var result = _mapper.Map<TourDetail>(tourdetail);
            return result;
        }

        public async Task<BaseResponseViewModel<TourDetail>> Update(int id, TourDetailUpdateRequest request)
        {
            var tourdetail = GetById(id);
            var tourDetailResponse = _mapper.Map<TourDetail>(tourdetail);
            if (tourdetail == null)
            {
                return new BaseResponseViewModel<TourDetail>
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
                var updateTourDetail = _mapper.Map<TourDetailUpdateRequest, TourDetail>(request, tourDetailResponse);
                await _unitOfWork.Repository<TourDetail>().UpdateDetached(updateTourDetail);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<TourDetail>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Updated",
                        IsSuccess = true
                    },
                    Data = updateTourDetail
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<TourDetail>
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

        public async Task<BaseResponseViewModel<TourDetail>> Create(TourDetailCreateRequest request)
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

                return new BaseResponseViewModel<TourDetail>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.Created,
                        Message = "Created",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<TourDetail>(tourdetail)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<TourDetail>
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

        public async Task<BaseResponseViewModel<TourDetail>> Delete(int id)
        {
            var tourdetail = GetById(id);
            if (tourdetail == null)
            {
                return new BaseResponseViewModel<TourDetail>
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

                return new BaseResponseViewModel<TourDetail>
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
                return new BaseResponseViewModel<TourDetail>
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
