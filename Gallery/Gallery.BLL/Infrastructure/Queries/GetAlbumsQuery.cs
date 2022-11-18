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
    public class GetAlbumsQuery : IRequest<IEnumerable<AlbumListViewModel>>
    { 
        public int PageCount { get; set; }

        public int PageSize { get; set; }

        public ClaimsPrincipal? User { get; set; }

        public Guid? UserId { get; set; }

        public GetAlbumsQuery(int pageCount, int pageSize, ClaimsPrincipal? user = null, Guid? userId = null)
        {
            UserId = userId;
            PageCount = pageCount;
            PageSize = pageSize;
            User = user;
        }
    }
}
