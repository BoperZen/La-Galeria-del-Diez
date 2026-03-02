using AutoMapper;
using La_Galeria_del_Diez.Application.DTOs;
using La_Galeria_del_Diez.Infraestructure.Models;

namespace La_Galeria_del_Diez.Application.Profiles
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<Image, ImageDTO>()
                .ReverseMap()
                .ForMember(dest => dest.IdObjectNavigation, opt => opt.Ignore());
        }
    }
}
