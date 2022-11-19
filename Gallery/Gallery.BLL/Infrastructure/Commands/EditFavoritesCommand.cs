using Gallery.BLL.Infrastructure.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Infrastructure.Commands
{
    public class EditFavoritesCommand : IRequest<bool>
    {
        public EditFavoritesDTO Dto { get; set; }

        public ClaimsPrincipal User { get; set; }

        public EditFavoritesCommand(EditFavoritesDTO dto, ClaimsPrincipal user)
        {
            Dto = dto;
            User = user;
        }
    }
}
