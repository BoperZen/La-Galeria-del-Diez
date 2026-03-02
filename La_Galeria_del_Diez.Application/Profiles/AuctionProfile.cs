using AutoMapper;
using La_Galeria_del_Diez.Application.DTOs;
using La_Galeria_del_Diez.Infraestructure.Models;

namespace La_Galeria_del_Diez.Application.Profiles
{
    public class AuctionProfile : Profile
    {
        public AuctionProfile()
        {
            CreateMap<Auction, AuctionDTO>()
                .ForMember(dest => dest.IdUserNavegation, opt => opt.MapFrom(src => src.IdUserNavigation))
                .ForMember(dest => dest.Winner, opt => opt.MapFrom(src => src.Winner))
                .ForMember(dest => dest.Biddings, opt => opt.MapFrom(src => src.Bidding))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.IdStateNavigation))
                .ReverseMap();
        }
    }
}
