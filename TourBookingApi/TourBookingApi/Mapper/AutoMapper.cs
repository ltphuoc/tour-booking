using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO.Request;
using DataAccess.DTO.Response;

namespace TourBookingApi.Mapper
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            CreateMap<Account, AccountRequest>().ReverseMap();
            CreateMap<Account, AccountResponse>().ReverseMap();
            CreateMap<Account, AccountCreateRequest>().ReverseMap();
            CreateMap<Account, AccountUpdateResquest>().ReverseMap();

            CreateMap<Destination, DestinationRequest>().ReverseMap();
            CreateMap<Destination, DestinationResponse>().ReverseMap();
            CreateMap<Destination, DestinationUpdateRequest>().ReverseMap();
            CreateMap<Destination, DestinationCreateRequest>().ReverseMap();

            CreateMap<Tour, TourRequest>().ReverseMap();
            CreateMap<Tour, TourUpdateRequest>().ReverseMap();
            CreateMap<Tour, TourCreateRequest>().ReverseMap();

            CreateMap<Booking, BookingRequest>().ReverseMap();
            CreateMap<Booking, BookingUpdateRequest>().ReverseMap();
            CreateMap<Booking, BookingCreateRequest>().ReverseMap();

            CreateMap<TourDetail, TourDetailRequest>().ReverseMap();
            CreateMap<TourDetail, TourDetailUpdateRequest>().ReverseMap();
            CreateMap<TourDetail, TourDetailCreateRequest>().ReverseMap();

            CreateMap<TourGuide, TourGuideRequest>().ReverseMap();
            CreateMap<TourGuide, TourGuideUpdateRequest>().ReverseMap();
            CreateMap<TourGuide, TourGuideCreateRequest>().ReverseMap();

            CreateMap<TourPrice, TourPriceRequest>().ReverseMap();
            CreateMap<TourPrice, TourPriceUpdateRequest>().ReverseMap();
            CreateMap<TourPrice, TourPriceCreateRequest>().ReverseMap();

            CreateMap<DestinationImage, DestinationImageResponse>().ReverseMap();
            CreateMap<DestinationImage, DestinationImageRequest>().ReverseMap();



            CreateMap<TourDetail, TourDetailResponse>().ReverseMap();


        }

    }
}
