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
    public class DisplayUrlListConverter : IValueConverter<Album, IEnumerable<string>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DisplayUrlListConverter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<string> Convert(Album sourceMember, ResolutionContext context)
        {
            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/images";
            return sourceMember.Pictures.Count > 0 ? 
                sourceMember.Pictures.Select(x => baseUrl + x.DisplayPath) : 
                Enumerable.Empty<string>();
        }
    }
}
