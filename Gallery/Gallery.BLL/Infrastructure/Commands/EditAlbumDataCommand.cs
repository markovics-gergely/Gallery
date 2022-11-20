using Gallery.BLL.Infrastructure.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Infrastructure.Commands
{
    public class EditAlbumDataCommand : IRequest<Unit>
    {
        public Guid AlbumId { get; set; }

        public EditAlbumDTO Dto { get; set; }

        public ClaimsPrincipal User { get; set; }

        public EditAlbumDataCommand(Guid albumId, EditAlbumDTO dto, ClaimsPrincipal user)
        {
            AlbumId = albumId;
            Dto = dto;
            User = user;
        }
    }
}
