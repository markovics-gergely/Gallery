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
    public class GetAlbumsQuery : IRequest<EnumerableWithTotalViewModel<AlbumViewModel>>
    {
        public GetAlbumsDTO Dto { get; set; }

        public ClaimsPrincipal? User { get; set; }

        public Guid? UserId { get; set; }

        public GetAlbumsQuery(GetAlbumsDTO dto, ClaimsPrincipal? user = null, Guid? userId = null)
        {
            UserId = userId;
            Dto = dto;
            User = user;
        }
    }
}
