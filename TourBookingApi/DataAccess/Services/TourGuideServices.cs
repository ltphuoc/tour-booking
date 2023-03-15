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
    public interface ITourGuideSevices
    {
        BaseResponsePagingViewModel<TourGuide> GetAll(PagingRequest request);
        BaseResponseViewModel<TourGuide> Get(int id);
        Task<BaseResponseViewModel<TourGuide>> Update(int id, TourGuideUpdateRequest request);
        Task<BaseResponseViewModel<TourGuide>> Create(TourGuideCreateRequest request);
        Task<BaseResponseViewModel<TourGuide>> Delete(int id);

    }
    public class TourGuideServices : ITourGuideSevices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TourGuideServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public BaseResponsePagingViewModel<TourGuide> GetAll(PagingRequest request)
        {
            var tourguides = _unitOfWork.Repository<TourGuide>()
                                .GetAll()
                                /*.Include(d => d.DestinationImages)*/
                                /*.ProjectTo<DestinationResponse>(_mapper.ConfigurationProvider)*/
                                .PagingQueryable(request.Page, request.PageSize, Common.Constants.LimitPaging, Common.Constants.DefaultPaging);

            return new BaseResponsePagingViewModel<TourGuide>
            {
                Metadata = new PagingsMetadata()
                {
                    Page = request.Page,
                    Size = request.PageSize,
                    Total = tourguides.Item1
                },
                Data = tourguides.Item2.ToList(),
            };
        }

        public BaseResponseViewModel<TourGuide> Get(int id)
        {
            var tourguide = GetById(id);
            if (tourguide == null)
            {
                return new BaseResponseViewModel<TourGuide>
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
            return new BaseResponseViewModel<TourGuide>
            {
                Status = new StatusViewModel
                {
                    Code = HttpStatusCode.OK,
                    Message = "OK",
                    IsSuccess = true
                },
                Data = _mapper.Map<TourGuide>(tourguide)
            };
        }

        private TourGuide GetById(int id)
        {
            var tourGuide = _unitOfWork.Repository<TourGuide>().GetById(id).Result;
            var result = _mapper.Map<TourGuide>(tourGuide);
            return result;
        }

        public async Task<BaseResponseViewModel<TourGuide>> Update(int id, TourGuideUpdateRequest request)
        {
            var tourGuide = GetById(id);
            var tourGuideResponse = _mapper.Map<TourGuide>(tourGuide);
            if (tourGuide == null)
            {
                return new BaseResponseViewModel<TourGuide>
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
                var updateTourGuide = _mapper.Map<TourGuideUpdateRequest, TourGuide>(request, tourGuideResponse);
                await _unitOfWork.Repository<TourGuide>().UpdateDetached(updateTourGuide);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<TourGuide>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Updated",
                        IsSuccess = true
                    },
                    Data = updateTourGuide
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<TourGuide>
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

        public async Task<BaseResponseViewModel<TourGuide>> Create(TourGuideCreateRequest request)
        {
            try
            {
                var tourGuide = _mapper.Map<TourGuide>(request);

                try
                {
                    await _unitOfWork.Repository<TourGuide>().InsertAsync(tourGuide);
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                return new BaseResponseViewModel<TourGuide>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.Created,
                        Message = "Created",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<TourGuide>(tourGuide)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<TourGuide>
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

        public async Task<BaseResponseViewModel<TourGuide>> Delete(int id)
        {
            var tourGuide = GetById(id);
            if (tourGuide == null)
            {
                return new BaseResponseViewModel<TourGuide>
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
                var tours = tourGuide.Tour;
                if (tours != null)
                {
                    _unitOfWork.Repository<Tour>().Delete(tours);
                }

                _unitOfWork.Repository<TourGuide>().Delete(tourGuide);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<TourGuide>
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
                return new BaseResponseViewModel<TourGuide>
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
