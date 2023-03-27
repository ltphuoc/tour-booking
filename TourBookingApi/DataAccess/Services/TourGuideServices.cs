using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObject.Models;
using BusinessObject.UnitOfWork;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using DataAccess.Helpers;
using System.Net;

namespace DataAccess.Services
{
    public interface ITourGuideSevices
    {
        BaseResponsePagingViewModel<TourGuideReponse> GetAll(PagingRequest request);
        BaseResponseViewModel<TourGuideReponse> Get(int id);
        Task<BaseResponseViewModel<TourGuideReponse>> Update(int id, TourGuideUpdateRequest request);
        Task<BaseResponseViewModel<TourGuideReponse>> Create(TourGuideCreateRequest request);
        Task<BaseResponseViewModel<TourGuideReponse>> Delete(int id);

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
        public BaseResponsePagingViewModel<TourGuideReponse> GetAll(PagingRequest request)
        {
            var query = _unitOfWork.Repository<TourGuide>()
                                .GetAll();

            var dynamicQuery = DynamicQueryHelper.ApplySearchSortAndPaging(query, request);

            var tourguides = dynamicQuery.ProjectTo<TourGuideReponse>(_mapper.ConfigurationProvider);

            return new BaseResponsePagingViewModel<TourGuideReponse>
            {
                Metadata = new PagingsMetadata()
                {
                    Page = request.Page,
                    Size = request.PageSize,
                    Total = tourguides.Count()
                },
                Data = tourguides.ToList(),
            };
        }

        public BaseResponseViewModel<TourGuideReponse> Get(int id)
        {
            var tourguide = GetById(id);
            if (tourguide == null)
            {
                return new BaseResponseViewModel<TourGuideReponse>
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
            return new BaseResponseViewModel<TourGuideReponse>
            {
                Status = new StatusViewModel
                {
                    Code = HttpStatusCode.OK,
                    Message = "OK",
                    IsSuccess = true
                },
                Data = _mapper.Map<TourGuideReponse>(tourguide)
            };
        }

        private TourGuide GetById(int id)
        {
            var tourGuide = _unitOfWork.Repository<TourGuide>().GetById(id).Result;
            var result = _mapper.Map<TourGuide>(tourGuide);
            return result;
        }

        public async Task<BaseResponseViewModel<TourGuideReponse>> Update(int id, TourGuideUpdateRequest request)
        {
            var tourGuide = GetById(id);
            //var tourGuideResponse = _mapper.Map<TourGuide>(tourGuide);
            if (tourGuide == null)
            {
                return new BaseResponseViewModel<TourGuideReponse>
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
                var updateTourGuide = _mapper.Map<TourGuideUpdateRequest, TourGuide>(request, tourGuide);
                await _unitOfWork.Repository<TourGuide>().UpdateDetached(updateTourGuide);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<TourGuideReponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.NoContent,
                        Message = "Updated",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<TourGuideReponse>(updateTourGuide)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<TourGuideReponse>
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

        public async Task<BaseResponseViewModel<TourGuideReponse>> Create(TourGuideCreateRequest request)
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

                return new BaseResponseViewModel<TourGuideReponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.Created,
                        Message = "Created",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<TourGuideReponse>(tourGuide)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<TourGuideReponse>
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

        public async Task<BaseResponseViewModel<TourGuideReponse>> Delete(int id)
        {
            var tourGuide = GetById(id);
            if (tourGuide == null)
            {
                return new BaseResponseViewModel<TourGuideReponse>
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
                //var tours = tourGuide.Tour;
                //if (tours != null)
                //{
                //    _unitOfWork.Repository<Tour>().Delete(tours);
                //}

                _unitOfWork.Repository<TourGuide>().Delete(tourGuide);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<TourGuideReponse>
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
                return new BaseResponseViewModel<TourGuideReponse>
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
