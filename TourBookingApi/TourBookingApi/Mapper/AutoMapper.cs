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
            CreateMap<Account, RegisterRequest>().ReverseMap();
            CreateMap<Account, LoginRequest>().ReverseMap();
            CreateMap<Account, ChangePasswordRequest>().ReverseMap();

            CreateMap<Destination, DestinationRequest>().ReverseMap();
            CreateMap<Destination, DestinationResponse>().ReverseMap();
            CreateMap<Destination, DestinationUpdateRequest>().ReverseMap();
            CreateMap<Destination, DestinationCreateRequest>().ReverseMap();

            CreateMap<DestinationImage, DestinationImageResponse>().ReverseMap();
            CreateMap<DestinationImage, DestinationImageRequest>().ReverseMap();
            CreateMap<DestinationImage, DestinationImageCreateRequest>().ReverseMap();
            CreateMap<DestinationImage, DestinationImageUpdateRequest>().ReverseMap();


            CreateMap<Tour, TourRequest>().ReverseMap();
            CreateMap<Tour, TourResponse>().ReverseMap();
            CreateMap<Tour, TourUpdateRequest>().ReverseMap();
            CreateMap<Tour, TourCreateRequest>().ReverseMap();

            CreateMap<Booking, BookingRequest>().ReverseMap();
            CreateMap<Booking, BookingUpdateRequest>().ReverseMap();
            CreateMap<Booking, BookingCreateRequest>().ReverseMap();
            CreateMap<Booking, BookingResponse>().ReverseMap();

            CreateMap<TourDetail, TourDetailRequest>().ReverseMap();
            CreateMap<TourDetail, TourDetailResponse>().ReverseMap();
            CreateMap<TourDetail, TourDetailUpdateRequest>().ReverseMap();
            CreateMap<TourDetail, TourDetailCreateRequest>().ReverseMap();

            CreateMap<TourGuide, TourGuideRequest>().ReverseMap();
            CreateMap<TourGuide, TourGuideUpdateRequest>().ReverseMap();
            CreateMap<TourGuide, TourGuideCreateRequest>().ReverseMap();
            CreateMap<TourGuide, TourGuideReponse>().ReverseMap();

            CreateMap<TourPrice, TourPriceRequest>().ReverseMap();
            CreateMap<TourPrice, TourPriceUpdateRequest>().ReverseMap();
            CreateMap<TourPrice, TourPriceCreateRequest>().ReverseMap();
            CreateMap<TourPrice, TourPriceResponse>().ReverseMap();

            CreateMap<Payment, PaymentRequest>().ReverseMap();
            CreateMap<Payment, PaymentCreateRequest>().ReverseMap();
            CreateMap<Payment, PaymentResponse>().ReverseMap();

            CreateMap<Transportation, TransportationRequest>().ReverseMap();
            CreateMap<Transportation, TransportationResponse>().ReverseMap();
            CreateMap<Transportation, TransportationUpdateRequest>().ReverseMap();
            CreateMap<Transportation, TransportationCreateRequest>().ReverseMap();
        }
    }
}
