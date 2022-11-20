using AutoMapper;
using Gallery.BLL.Infrastructure.ViewModels;
using Gallery.DAL.Configurations.Interfaces;
using Gallery.DAL.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.ValueResolvers
{
    public class PictureDisplayUrlConverter : IValueConverter<Picture, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGalleryConfigurationService _galleryConfiguration;

        public PictureDisplayUrlConverter(IHttpContextAccessor httpContextAccessor, IGalleryConfigurationService galleryConfiguration)
        {
            _httpContextAccessor = httpContextAccessor;
            _galleryConfiguration = galleryConfiguration;
        }

        public string Convert(Picture sourceMember, ResolutionContext context)
        {
            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://" +
                $"{_httpContextAccessor.HttpContext.Request.Host}/{_galleryConfiguration.GetStaticFileRequestPath()}/{_galleryConfiguration.GetImagesSubdirectory()}";
            return baseUrl + sourceMember.DisplayPath;
        }
    }
}
