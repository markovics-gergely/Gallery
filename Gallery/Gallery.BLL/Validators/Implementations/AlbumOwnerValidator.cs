using Gallery.BLL.Extensions;
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
    public class AlbumOwnerValidator : IValidator
    {
        private readonly Album album;
        private readonly ClaimsPrincipal user;

        public AlbumOwnerValidator(Album album, ClaimsPrincipal user)
        {
            this.album = album;
            this.user = user;
        }

        public bool Validate()
        {
            var userId = Guid.Parse(user.GetUserIdFromJwt());
            return userId == album.Creator.Id;
        }
    }
}
