using AutoMapper;
using La_Galeria_del_Diez.Application.DTOs;
using La_Galeria_del_Diez.Infraestructure.Models;

namespace La_Galeria_del_Diez.Application.Profiles
{
    public class AuctionableObjectProfile : Profile
    {
        public AuctionableObjectProfile()
        {
            CreateMap<AuctionableObject, Auctionable_ObjectDTO>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.IdCategory))
                .ForMember(dest => dest.Auctions, opt => opt.MapFrom(src => src.Auction))
                .ReverseMap();
        }
    }
}