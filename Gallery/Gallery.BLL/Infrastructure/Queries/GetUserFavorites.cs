using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Infrastructure.Queries
{
    public class GetUserFavorites
    {
        public Guid UserId { get; set; }

        public int PageCount { get; set; }

        public int PageSize { get; set; }
    }
}
