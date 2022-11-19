using AutoMapper;
using Gallery.BLL.Infrastructure.ViewModels;
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
    public class DisplayUrlConverter : IValueConverter<Album, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DisplayUrlConverter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Convert(Album sourceMember, ResolutionContext context)
        {
            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/images";
            return sourceMember.Pictures.Count > 0 ? baseUrl + sourceMember.Pictures.First().DisplayPath : string.Empty;
        }
    }
}
