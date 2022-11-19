using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Infrastructure.Commands
{
    public class LikeAlbumCommand : IRequest<bool>
    {
        public Guid AlbumId { get; set; }

        public LikeAlbumCommand(Guid albumId)
        {
            AlbumId = albumId;
        }
    }
}
