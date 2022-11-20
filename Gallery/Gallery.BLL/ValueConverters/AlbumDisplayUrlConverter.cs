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
    public class AlbumDisplayUrlConverter : IValueConverter<Album, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGalleryConfigurationService _galleryConfiguration;

        public AlbumDisplayUrlConverter(IHttpContextAccessor httpContextAccessor, IGalleryConfigurationService galleryConfiguration)
        {
            _httpContextAccessor = httpContextAccessor;
            _galleryConfiguration = galleryConfiguration;
        }

        public string Convert(Album sourceMember, ResolutionContext context)
        {
            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://" +
                $"{_httpContextAccessor.HttpContext.Request.Host}/{_galleryConfiguration.GetStaticFileRequestPath()}/{_galleryConfiguration.GetImagesSubdirectory()}";
            return sourceMember.Pictures.Count > 0 ? baseUrl + sourceMember.Pictures.First().DisplayPath : string.Empty;
        }
    }
}
