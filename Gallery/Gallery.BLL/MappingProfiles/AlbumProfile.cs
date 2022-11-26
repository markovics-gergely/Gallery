using AutoMapper;
using Gallery.BLL.Infrastructure.DataTransferObjects;
using Gallery.BLL.Infrastructure.ViewModels;
using Gallery.BLL.ValueResolvers;
using Gallery.DAL.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.MappingProfiles
{
    public class AlbumProfile : Profile
    {
        public AlbumProfile() 
        {
            CreateMap<CreateAlbumDTO, Album>()
                .ForMember(dest => dest.IsPrivate, opt => opt.MapFrom(src => src.IsPrivate))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Pictures, opt => opt.Ignore());

            CreateMap<Album, AlbumViewModel>()
                .ForMember(dest => dest.IsPrivate, opt => opt.MapFrom(src => src.IsPrivate))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.LikeCount))
                .ForMember(dest => dest.Pictures, opt => opt.MapFrom(src => src.Pictures))
                .ForMember(dest => dest.IsFavorite, opt => opt.ConvertUsing<FavoriteConverter, Album>(src => src));

            CreateMap<Picture, PictureViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DisplayUrl, opt => opt.ConvertUsing<PictureDisplayUrlConverter, Picture>(src => src));

            CreateMap<Album, UserProfileViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CountOfPictures, opt => opt.MapFrom(src => src.Pictures.Count))
                .ForMember(dest => dest.CoverUrl, opt => opt.ConvertUsing<AlbumDisplayUrlConverter, Album>(src => src));
        }
    }
}
