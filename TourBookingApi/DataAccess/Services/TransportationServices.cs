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
    public interface ITransportationSevices
    {
        BaseResponsePagingViewModel<TransportationResponse> GetAll(PagingRequest request);
        BaseResponseViewModel<TransportationResponse> Get(int id);
        Task<BaseResponseViewModel<TransportationResponse>> Update(int id, TransportationUpdateRequest request);
        Task<BaseResponseViewModel<TransportationResponse>> Create(TransportationCreateRequest request);
        Task<BaseResponseViewModel<TransportationResponse>> Delete(int id);

    }
    public class TransportationServices : ITransportationSevices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransportationServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public BaseResponsePagingViewModel<TransportationResponse> GetAll(PagingRequest request)
        {
            var query = _unitOfWork.Repository<Transportation>()
                                .GetAll()
                                .Include(d => d.TourDetails);

            var dynamicQuery = DynamicQueryHelper.ApplySearchSortAndPaging(query, request);

            var transportations = dynamicQuery.ProjectTo<TransportationResponse>(_mapper.ConfigurationProvider);

            return new BaseResponsePagingViewModel<TransportationResponse>
            {
                Metadata = new PagingsMetadata()
                {
                    Page = request.Page,
                    Size = request.PageSize,
                    Total = transportations.Count()
                },
                Data = transportations.ToList(),
            };
        }

        public async Task<BaseResponseViewModel<TransportationResponse>> Create(TransportationRequest request)
        {
            try
            {
                var transportation = _mapper.Map<Transportation>(request);

                try
                {
                    await _unitOfWork.Repository<Transportation>().InsertAsync(transportation);
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                return new BaseResponseViewModel<TransportationResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.Created,
                        Message = "Created",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<TransportationResponse>(transportation)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<TransportationResponse>
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

        public BaseResponseViewModel<TransportationResponse> Get(int id)
        {
            var transportation = GetById(id);
            if (transportation == null)
            {
                return new BaseResponseViewModel<TransportationResponse>
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
            return new BaseResponseViewModel<TransportationResponse>
            {
                Status = new StatusViewModel
                {
                    Code = HttpStatusCode.OK,
                    Message = "OK",
                    IsSuccess = true
                },
                Data = _mapper.Map<TransportationResponse>(transportation)
            };
        }

        private Transportation GetById(int id)
        {
            var transportation = _unitOfWork.Repository<Transportation>().GetById(id).Result;
            var result = _mapper.Map<Transportation>(transportation);
            return result;
        }

        public async Task<BaseResponseViewModel<TransportationResponse>> Update(int id, TransportationUpdateRequest request)
        {
            var transportation = GetById(id);
            //var transportationResponse = _mapper.Map<Transportation>(transportation);
            if (transportation == null)
            {
                return new BaseResponseViewModel<TransportationResponse>
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
                var updateTransportation = _mapper.Map<TransportationUpdateRequest, Transportation>(request, transportation);
                await _unitOfWork.Repository<Transportation>().UpdateDetached(updateTransportation);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<TransportationResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.NoContent,
                        Message = "Updated",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<TransportationResponse>(updateTransportation)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<TransportationResponse>
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

        public async Task<BaseResponseViewModel<TransportationResponse>> Create(TransportationCreateRequest request)
        {
            try
            {
                var transportation = _mapper.Map<Transportation>(request);

                try
                {
                    await _unitOfWork.Repository<Transportation>().InsertAsync(transportation);
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                return new BaseResponseViewModel<TransportationResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.Created,
                        Message = "Created",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<TransportationResponse>(transportation)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<TransportationResponse>
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

        public async Task<BaseResponseViewModel<TransportationResponse>> Delete(int id)
        {
            var transportation = GetById(id);
            if (transportation == null)
            {
                return new BaseResponseViewModel<TransportationResponse>
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
                /*var tourDetails = transportation.Booking;
                if (bookings != null)
                {
                    _unitOfWork.Repository<Booking>().Delete(bookings);
                }*/

                _unitOfWork.Repository<Transportation>().Delete(transportation);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<TransportationResponse>
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
                return new BaseResponseViewModel<TransportationResponse>
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

