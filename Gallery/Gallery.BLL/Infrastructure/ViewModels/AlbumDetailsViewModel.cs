using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Infrastructure.ViewModels
{
    public class AlbumDetailsViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsPrivate { get; set; }

        public bool IsFavorite { get; set; }

        public int LikeCount { get; set; }

        public IEnumerable<PictureViewModel> Pictures { get; set; } = Enumerable.Empty<PictureViewModel>();

        public UserNameViewModel Creator { get; set; } = new UserNameViewModel();
    }
}
