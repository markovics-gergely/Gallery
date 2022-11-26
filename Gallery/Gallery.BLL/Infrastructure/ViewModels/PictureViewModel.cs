using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Infrastructure.ViewModels
{
    public class PictureViewModel
    {
        public Guid Id { get; set; }

        public string DisplayUrl { get; set; } = string.Empty;
    }
}
