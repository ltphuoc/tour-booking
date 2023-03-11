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


        }

    }
}
