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
    public class AddPicturesToAlbumCommand : IRequest<Unit>
    {
        public Guid AlbumId { get; set; }

        public AddAlbumPicturesDTO Dto { get; set; }

        public ClaimsPrincipal User { get; set; }

        public AddPicturesToAlbumCommand(Guid albumId, AddAlbumPicturesDTO dto, ClaimsPrincipal user)
        {
            AlbumId = albumId;
            Dto = dto;
            User = user;
        }
    }
}
