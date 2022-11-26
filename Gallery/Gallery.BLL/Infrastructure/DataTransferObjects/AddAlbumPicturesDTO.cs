﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Infrastructure.DataTransferObjects
{
    public class AddAlbumPicturesDTO
    {
        public IEnumerable<IFormFile> Pictures { get; set; } = Enumerable.Empty<IFormFile>();
    }
}
