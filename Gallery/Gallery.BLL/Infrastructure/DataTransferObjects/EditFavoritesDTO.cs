using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Infrastructure.DataTransferObjects
{
    public class EditFavoritesDTO
    {
        public IEnumerable<Guid> AlbumIds { get; set; } = Enumerable.Empty<Guid>();
    }
}
