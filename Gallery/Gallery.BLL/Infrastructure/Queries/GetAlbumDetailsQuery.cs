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
    public class GetAlbumDetailsQuery : IRequest<AlbumDetailsViewModel>
    {
        public Guid AlbumId { get; set; }

        public ClaimsPrincipal? User { get; set; }

        public GetAlbumDetailsQuery(Guid albumId, ClaimsPrincipal? user = null)
        {
            AlbumId = albumId;
            User = user;
        }
    }
}
