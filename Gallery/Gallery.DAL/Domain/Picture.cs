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

        public string PhysicalPath { get; set; } = string.Empty;

        public string DisplayPath { get; set; } = string.Empty;

        public double Size { get; set; }

        public string FileExtension { get; set; } = string.Empty;
    }
}
