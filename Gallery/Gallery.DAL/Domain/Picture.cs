using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.DAL.Domain
{
    public class Picture
    {
        public Guid Id { get; set; }

        public Album Album { get; set; } = new Album();

        public string Path { get; set; } = string.Empty;

        public int Size { get; set; }

        public PictureFormat FileExtension { get; set; }
    }
}
