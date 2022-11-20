using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Infrastructure.ViewModels
{
    public class AlbumListViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int CountOfPictures { get; set; }

        public string CoverUrl { get; set; } = string.Empty;

        public UserNameViewModel Creator { get; set; } = new UserNameViewModel();
    }
}
