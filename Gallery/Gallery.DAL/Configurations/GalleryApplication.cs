using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.DAL.Configurations
{
    public class GalleryApplication
    {
        public int MaxUploadSize { get; set; }

        public int MaxUploadCount { get; set; }

        public string StaticFilePath { get; set; } = string.Empty;

        public IEnumerable<string> AcceptedExtensions { get; set; } = Enumerable.Empty<string>();
    }
}
