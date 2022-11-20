using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Infrastructure.DataTransferObjects
{
    public class EditAlbumDTO
    {
        public string? Name { get; set; }

        public bool? IsPrivate { get; set; }
    }
}
