using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObject.Models;
using BusinessObject.UnitOfWork;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;
using Microsoft.EntityFrameworkCore;
using NTQ.Sdk.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
            var destinations = _unitOfWork.Repository<Destination>()
                                .GetAll()
                                .Include(d => d.DestinationImages)
                                .ProjectTo<DestinationResponse>(_mapper.ConfigurationProvider)
                                .PagingQueryable(request.Page, request.PageSize, Common.Constants.LimitPaging, Common.Constants.DefaultPaging);

            return new BaseResponsePagingViewModel<DestinationResponse>
            {
                Metadata = new PagingsMetadata()
                {
                    Page = request.Page,
                    Size = request.PageSize,
                    Total = destinations.Item1
                },
                Data = destinations.Item2.ToList(),
            };
        }

        public async Task<BaseResponseViewModel<DestinationResponse>> Create(DestinationRequest request)
        {
            try
            {
                var destination = _mapper.Map<Destination>(request);

                try
                {
                    await _unitOfWork.Repository<Destination>().InsertAsync(destination);
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

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
                    Data = null
                };
            }

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
                    Data = null
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
            var destinationResponse = _mapper.Map<DestinationResponse>(destination);
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
                    Data = null
                };
            }
            try
            {
                var updateDestination = _mapper.Map<DestinationUpdateRequest, DestinationResponse>(request, destinationResponse);
                await _unitOfWork.Repository<DestinationResponse>().UpdateDetached(updateDestination);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<DestinationResponse>
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
                return new BaseResponseViewModel<DestinationResponse>
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

        public async Task<BaseResponseViewModel<DestinationResponse>> Create(DestinationCreateRequest request)
        {
            try
            {
                var destination = _mapper.Map<Destination>(request);

                try
                {
                    await _unitOfWork.Repository<Destination>().InsertAsync(destination);
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

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
                    Data = null
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
                    Data = null
                };
            }
            try
            {
                var destinationImages = destination.DestinationImages.ToList();
                foreach (var destinationImage in destinationImages)
                {
                    _unitOfWork.Repository<DestinationImage>().Delete(destinationImage);
                }
                var tourDetails = destination.TourDetails.ToList();
                foreach (var tourDetail in tourDetails)
                {
                    _unitOfWork.Repository<TourDetail>().Delete(tourDetail);
                }

                _unitOfWork.Repository<Destination>().Delete(destination);
                await _unitOfWork.CommitAsync();

                return new BaseResponseViewModel<DestinationResponse>
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
                return new BaseResponseViewModel<DestinationResponse>
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


        private Destination GetById(int id)
        {
            var destination = _unitOfWork.Repository<Destination>().GetById(id).Result;
            var result = _mapper.Map<Destination>(destination);
            return result;
        }

    }
}
