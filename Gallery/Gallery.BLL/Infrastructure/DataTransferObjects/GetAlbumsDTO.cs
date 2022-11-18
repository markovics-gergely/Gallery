using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Infrastructure.DataTransferObjects
{
    public class GetAlbumsDTO
    {
        public int PageSize { get; set; }

        public int PageCount { get; set; }
    }
}
