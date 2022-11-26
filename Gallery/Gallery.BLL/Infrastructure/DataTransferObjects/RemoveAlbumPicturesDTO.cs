using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Infrastructure.DataTransferObjects
{
    public class RemoveAlbumPicturesDTO
    {
        public IEnumerable<Guid> PictureIds { get; set; } = Enumerable.Empty<Guid>();
    }
}
