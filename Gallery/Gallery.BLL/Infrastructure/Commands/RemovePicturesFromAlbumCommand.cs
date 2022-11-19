﻿using Gallery.BLL.Infrastructure.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Infrastructure.Commands
{
    public class RemovePicturesFromAlbumCommand : IRequest<bool>
    {
        public Guid AlbumId { get; set; }

        public RemoveAlbumPicturesDTO Dto { get; set; }

        public ClaimsPrincipal User { get; set; }

        public RemovePicturesFromAlbumCommand(Guid albumId, RemoveAlbumPicturesDTO dto, ClaimsPrincipal user)
        {
            AlbumId = albumId;
            Dto = dto;
            User = user;
        }
    }
}
