using Gallery.BLL.Infrastructure.DataTransferObjects;
using MediatR;

namespace Gallery.BLL.Infrastructure.Commands
{
    public class EditUserRoleCommand : IRequest
    {
        public EditUserRoleDTO DTO { get; set; }

        public EditUserRoleCommand(EditUserRoleDTO dto)
        {
            DTO = dto;
        }
    }
}
