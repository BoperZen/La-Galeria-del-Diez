using AutoMapper;
using La_Galeria_del_Diez.Application.DTOs;
using La_Galeria_del_Diez.Application.DTOs.Api;

namespace La_Galeria_del_Diez.Application.Profiles.Api
{
    public class AuctionApiProfile : Profile
    {
        public AuctionApiProfile()
        {
            CreateMap<AuctionDTO, AuctionApiDto>().ReverseMap();
        }
    }
}
