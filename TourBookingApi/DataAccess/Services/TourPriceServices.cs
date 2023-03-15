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
    public interface ITourPriceSevices
    {
        BaseResponsePagingViewModel<TourPrice> GetAll(PagingRequest request);
        BaseResponseViewModel<TourPrice> Get(int id);
        Task<BaseResponseViewModel<TourPrice>> Update(int id, TourPriceUpdateRequest request);
        Task<BaseResponseViewModel<TourPrice>> Create(TourPriceCreateRequest request);
        Task<BaseResponseViewModel<TourPrice>> Delete(int id);

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
        public BaseResponsePagingViewModel<TourPrice> GetAll(PagingRequest request)
        {
            var tourPrices = _unitOfWork.Repository<TourPrice>()
                                .GetAll()
                                /*.Include(d => d.DestinationImages)*/
                                /*.ProjectTo<DestinationResponse>(_mapper.ConfigurationProvider)*/
                                .PagingQueryable(request.Page, request.PageSize, Common.Constants.LimitPaging, Common.Constants.DefaultPaging);

            return new BaseResponsePagingViewModel<TourPrice>
            {
                Metadata = new PagingsMetadata()
                {
                    Page = request.Page,
                    Size = request.PageSize,
                    Total = tourPrices.Item1
                },
                Data = tourPrices.Item2.ToList(),
            };
        }

        public BaseResponseViewModel<TourPrice> Get(int id)
        {
            var tourPrice = GetById(id);
            if (tourPrice == null)
            {
                return new BaseResponseViewModel<TourPrice>
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
            return new BaseResponseViewModel<TourPrice>
            {
                Status = new StatusViewModel
                {
                    Code = HttpStatusCode.OK,
                    Message = "OK",
                    IsSuccess = true
                },
                Data = _mapper.Map<TourPrice>(tourPrice)
            };
        }

        private TourPrice GetById(int id)
        {
            var tourPrice = _unitOfWork.Repository<TourPrice>().GetById(id).Result;
            var result = _mapper.Map<TourPrice>(tourPrice);
            return result;
        }

        public async Task<BaseResponseViewModel<TourPrice>> Update(int id, TourPriceUpdateRequest request)
        {
            var tourPrice = GetById(id);
            var tourPriceResponse = _mapper.Map<TourPrice>(tourPrice);
            if (tourPrice == null)
            {
                return new BaseResponseViewModel<TourPrice>
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
                var updateTourPrice = _mapper.Map<TourPriceUpdateRequest, TourPrice>(request, tourPriceResponse);
                await _unitOfWork.Repository<TourPrice>().UpdateDetached(updateTourPrice);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<TourPrice>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Updated",
                        IsSuccess = true
                    },
                    Data = updateTourPrice
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<TourPrice>
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

        public async Task<BaseResponseViewModel<TourPrice>> Create(TourPriceCreateRequest request)
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

                return new BaseResponseViewModel<TourPrice>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.Created,
                        Message = "Created",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<TourPrice>(tourPrice)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<TourPrice>
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

        public async Task<BaseResponseViewModel<TourPrice>> Delete(int id)
        {
            var tourPrice = GetById(id);
            if (tourPrice == null)
            {
                return new BaseResponseViewModel<TourPrice>
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

                return new BaseResponseViewModel<TourPrice>
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
                return new BaseResponseViewModel<TourPrice>
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
