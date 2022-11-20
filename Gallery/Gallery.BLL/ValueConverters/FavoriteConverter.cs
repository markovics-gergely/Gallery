using AutoMapper;
using Gallery.BLL.Extensions;
using Gallery.DAL.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.ValueResolvers
{
    public class FavoriteConverter : IValueConverter<Album, bool>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FavoriteConverter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool Convert(Album sourceMember, ResolutionContext context)
        {
            if (!_httpContextAccessor.HttpContext.User.Identity?.IsAuthenticated ?? true)
            {
                return false;
            }
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.GetUserIdFromJwt());
            return sourceMember.FavoritedBy.Select(x => x.Id).Contains(userId);
        }
    }
}
