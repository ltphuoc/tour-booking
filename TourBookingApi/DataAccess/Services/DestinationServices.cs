using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObject.Models;
using BusinessObject.UnitOfWork;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using DataAccess.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Net;
using static DataAccess.Common.Constants;

namespace DataAccess.Services
{
    public interface IDestinationServices
    {
        BaseResponsePagingViewModel<DestinationResponse> GetAll(PagingRequest request);
        BaseResponseViewModel<DestinationResponse> Get(int id);
        Task<BaseResponseViewModel<DestinationResponse>> Update(int id, DestinationUpdateRequest request);
        Task<BaseResponseViewModel<DestinationResponse>> Create(DestinationCreateRequest request);
        Task<BaseResponseViewModel<DestinationResponse>> Delete(int id);

    }
    public class DestinationServices : IDestinationServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DestinationServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public BaseResponsePagingViewModel<DestinationResponse> GetAll(PagingRequest request)
        {
            var query = _unitOfWork.Repository<Destination>().GetAll().Include(d => d.DestinationImages).Where(d => d.Status == Status.INT_ACTIVE_STATUS);

            var dynamicQuery = DynamicQueryHelper.ApplySearchSortAndPaging(query, request);

            //var destinations = dynamicQuery.Select(x => _mapper.Map<DestinationResponse>(x));
            var destinations = dynamicQuery.ProjectTo<DestinationResponse>(_mapper.ConfigurationProvider);

            return new BaseResponsePagingViewModel<DestinationResponse>
            {
                Metadata = new PagingsMetadata()
                {
                    Page = request.Page,
                    Size = request.PageSize,
                    Total = dynamicQuery.Count()
                },
                Data = destinations.ToList()
            };
        }

        public BaseResponseViewModel<DestinationResponse> Get(int id)
        {
            var destination = GetById(id);
            if (destination == null)
            {
                return new BaseResponseViewModel<DestinationResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.NotFound,
                        Message = "Not Found",
                        IsSuccess = false
                    },
                };
            }
            return new BaseResponseViewModel<DestinationResponse>
            {
                Status = new StatusViewModel
                {
                    Code = HttpStatusCode.OK,
                    Message = "OK",
                    IsSuccess = true
                },
                Data = _mapper.Map<DestinationResponse>(destination)
            };
        }

        public async Task<BaseResponseViewModel<DestinationResponse>> Update(int id, DestinationUpdateRequest request)
        {
            var destination = GetById(id);

            if (destination == null)
            {
                return new BaseResponseViewModel<DestinationResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.NotFound,
                        Message = "Not Found",
                        IsSuccess = false
                    },
                };
            }
            try
            {
                var updateDestination = _mapper.Map<DestinationUpdateRequest, Destination>(request, destination);
                await _unitOfWork.Repository<Destination>().UpdateDetached(updateDestination);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<DestinationResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Updated",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<DestinationResponse>(updateDestination)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<DestinationResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.BadRequest,
                        Message = "Bad Request",
                        IsSuccess = false
                    },
                };
            }
        }

        public async Task<BaseResponseViewModel<DestinationResponse>> Create(DestinationCreateRequest request)
        {
            try
            {
                var destination = _mapper.Map<Destination>(request);
                destination.Status = Status.INT_ACTIVE_STATUS;

                await _unitOfWork.Repository<Destination>().InsertAsync(destination);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<DestinationResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.Created,
                        Message = "Created",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<DestinationResponse>(destination)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<DestinationResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.BadRequest,
                        Message = "Bad Request",
                        IsSuccess = false
                    },
                };
            }
        }

        public async Task<BaseResponseViewModel<DestinationResponse>> Delete(int id)
        {
            var destination = GetById(id);
            if (destination == null)
            {
                return new BaseResponseViewModel<DestinationResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.NotFound,
                        Message = "Not Found",
                        IsSuccess = false
                    },
                };
            }
            try
            {
                //var destinationImages = destination.DestinationImages.ToList();
                //foreach (var destinationImage in destinationImages)
                //{
                //    _unitOfWork.Repository<DestinationImage>().Delete(destinationImage);
                //}
                //var tourDetails = destination.TourDetails.ToList();
                //foreach (var tourDetail in tourDetails)
                //{
                //    _unitOfWork.Repository<TourDetail>().Delete(tourDetail);
                //}
                destination.Status = Status.INT_DELETED_STATUS;

                await _unitOfWork.Repository<Destination>().UpdateDetached(destination);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<DestinationResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.OK,
                        Message = "OK",
                        IsSuccess = true
                    },
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<DestinationResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.BadRequest,
                        Message = "Bad Request\n " + ex.Message,
                        IsSuccess = false
                    },
                };
            }
        }

        private Destination GetById(int id)
        {
            var destination = _unitOfWork.Repository<Destination>().GetById(id).Result;
            var result = _mapper.Map<Destination>(destination);
            return result;
        }

    }
}
