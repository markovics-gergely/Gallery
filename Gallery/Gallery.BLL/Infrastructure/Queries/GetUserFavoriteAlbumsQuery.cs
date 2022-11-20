using Gallery.BLL.Infrastructure.DataTransferObjects;
using Gallery.BLL.Infrastructure.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Infrastructure.Queries
{
    public class GetUserFavoriteAlbumsQuery : IRequest<EnumerableWithCountViewModel<AlbumListViewModel>>
    {
        public GetAlbumsDTO Dto { get; set; }

        public ClaimsPrincipal User { get; set; }

        public GetUserFavoriteAlbumsQuery(GetAlbumsDTO dto, ClaimsPrincipal user)
        {
            Dto = dto;
            User = user;
        }
    }
}
