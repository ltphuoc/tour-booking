using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObject.Models;
using BusinessObject.UnitOfWork;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using DataAccess.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Net;
using static DataAccess.Common.Constants;

namespace DataAccess.Services
{
    public interface IDestinationImageServices
    {
        BaseResponsePagingViewModel<DestinationImageResponse> GetAll(PagingRequest request, int destinationId);
        BaseResponseViewModel<DestinationImageResponse> Get(int id);
        Task<BaseResponseViewModel<DestinationImageResponse>> Update(int id, DestinationImageUpdateRequest request);
        Task<BaseResponseViewModel<DestinationImageResponse>> Create(DestinationImageCreateRequest request);
        Task<BaseResponseViewModel<DestinationImageResponse>> Delete(int id);
    }
    public class DestinationImageServices : IDestinationImageServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DestinationImageServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponseViewModel<DestinationImageResponse>> Create(DestinationImageCreateRequest request)
        {
            try
            {
                foreach (var item in request.Image)
                {
                    var destination = new DestinationImage();
                    destination.DestinationId = request.DestinationId;
                    destination.Image = item;
                    await _unitOfWork.Repository<DestinationImage>().InsertAsync(destination);
                    await _unitOfWork.CommitAsync();
                }


                return new BaseResponseViewModel<DestinationImageResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.Created,
                        Message = "Created",
                        IsSuccess = true
                    },
                    Data = null!
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<DestinationImageResponse>
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

        public async Task<BaseResponseViewModel<DestinationImageResponse>> Delete(int id)
        {
            var destination = GetById(id);
            if (destination == null)
            {
                return new BaseResponseViewModel<DestinationImageResponse>
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
                _unitOfWork.Repository<DestinationImage>().Delete(destination);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<DestinationImageResponse>
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
                return new BaseResponseViewModel<DestinationImageResponse>
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

        public BaseResponseViewModel<DestinationImageResponse> Get(int id)
        {
            var destination = GetById(id);
            if (destination == null)
            {
                return new BaseResponseViewModel<DestinationImageResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.NotFound,
                        Message = "Not Found",
                        IsSuccess = false
                    },
                };
            }
            return new BaseResponseViewModel<DestinationImageResponse>
            {
                Status = new StatusViewModel
                {
                    Code = HttpStatusCode.OK,
                    Message = "OK",
                    IsSuccess = true
                },
                Data = _mapper.Map<DestinationImageResponse>(destination)
            };
        }

        public BaseResponsePagingViewModel<DestinationImageResponse> GetAll(PagingRequest request, int destinationId)
        {
            var query = destinationId == 0 ? _unitOfWork.Repository<DestinationImage>()
                                .GetAll().Include(d => d.Destination).Where(d => d.Destination.Status == Status.INT_ACTIVE_STATUS)
                                : _unitOfWork.Repository<DestinationImage>()
                                .GetAll().Include(d => d.Destination).Where(d => d.Destination.Status == Status.INT_ACTIVE_STATUS)
                                .Where(d => d.DestinationId == destinationId);

            var dynamicQuery = DynamicQueryHelper.ApplySearchSortAndPaging(query, request);

            var destinationImages = dynamicQuery.ProjectTo<DestinationImageResponse>(_mapper.ConfigurationProvider);

            return new BaseResponsePagingViewModel<DestinationImageResponse>
            {
                Metadata = new PagingsMetadata()
                {
                    Page = request.Page,
                    Size = request.PageSize,
                    Total = destinationImages.Count()
                },
                Data = destinationImages.ToList(),
            };
        }

        public async Task<BaseResponseViewModel<DestinationImageResponse>> Update(int id, DestinationImageUpdateRequest request)
        {
            var destination = GetById(id);

            if (destination == null)
            {
                return new BaseResponseViewModel<DestinationImageResponse>
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
                var updateDestination = _mapper.Map<DestinationImageUpdateRequest, DestinationImage>(request, destination);
                await _unitOfWork.Repository<DestinationImage>().UpdateDetached(updateDestination);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<DestinationImageResponse>
                {
                    Status = new StatusViewModel
                    {
                        Code = HttpStatusCode.OK,
                        Message = "Updated",
                        IsSuccess = true
                    },
                    Data = _mapper.Map<DestinationImageResponse>(updateDestination)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseViewModel<DestinationImageResponse>
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

        private DestinationImage? GetById(int id)
        {
            var destination = _unitOfWork.Repository<DestinationImage>().GetById(id).Result;
            return _mapper.Map<DestinationImage>(destination);
        }
    }
}
