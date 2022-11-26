using Gallery.BLL.Validators.Interfaces;
using Gallery.DAL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Validators.Implementations
{
    public class PublicAlbumValidator : IValidator
    {
        private readonly Album album;

        public PublicAlbumValidator(Album album)
        {
            this.album = album;
        }

        public bool Validate()
        {
            return !album.IsPrivate;
        }
    }
}
