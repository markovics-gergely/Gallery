using Gallery.BLL.Infrastructure.DataTransferObjects;
using MediatR;

namespace Gallery.BLL.Infrastructure.Commands
{
    public class EditUserCommand : IRequest<bool>
    {
        public EditUserDTO DTO { get; set; }

        public EditUserCommand(EditUserDTO dto)
        {
            DTO = dto;
        }
    }
}
