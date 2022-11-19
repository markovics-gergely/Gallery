using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.DAL.Domain
{
    public class Album
    {
        public Guid Id { get; set; }

        public ApplicationUser Creator { get; set; } = new ApplicationUser();

        public string Name { get; set; } = string.Empty;

        public bool IsPrivate { get; set; }

        public int LikeCount { get; set; }

        public IList<Picture> Pictures { get; set; } = new List<Picture>();

        public IList<ApplicationUser> FavoritedBy { get; set; } = new List<ApplicationUser>();
    }
}
