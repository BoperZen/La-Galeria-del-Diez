using AutoMapper;
using La_Galeria_del_Diez.Application.DTOs;
using La_Galeria_del_Diez.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace La_Galeria_del_Diez.Application.Profiles
{
    public class BiddingProfile : Profile
    {
        public BiddingProfile()
        {
            CreateMap<Bidding, BiddingDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.IdUserNavigation.Username))
                .ReverseMap()
                .ForMember(dest => dest.IdUserNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.IdAuctionNavigation, opt => opt.Ignore());
        }
    }
}
