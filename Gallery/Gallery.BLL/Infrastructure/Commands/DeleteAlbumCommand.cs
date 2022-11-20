using Gallery.BLL.Infrastructure.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Infrastructure.Commands
{
    public class DeleteAlbumCommand : IRequest<Unit>
    {
        public Guid AlbumId { get; set; }

        public ClaimsPrincipal User { get; set; }

        public DeleteAlbumCommand(Guid albumId, ClaimsPrincipal user)
        {
            AlbumId = albumId;
            User = user;
        }
    }
}
