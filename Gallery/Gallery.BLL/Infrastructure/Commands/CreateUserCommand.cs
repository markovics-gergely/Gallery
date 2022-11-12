using Gallery.BLL.Infrastructure.DataTransferObjects;
using MediatR;

namespace Gallery.BLL.Infrastructure.Commands
{
    public class CreateUserCommand : IRequest<bool>
    {
        public RegisterUserDTO DTO { get; set; }

        public CreateUserCommand(RegisterUserDTO dto)
        {
            DTO = dto;
        }
    }
}
