using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Gallery.BLL.Infrastructure.Commands;
using Gallery.DAL.Domain;
using Gallery.BLL.Stores.Interfaces;
using Gallery.BLL.Exceptions;
using Gallery.BLL.Utils;

namespace Gallery.BLL.Infrastructure
{
    public class UserCommandHandler :
        IRequestHandler<CreateUserCommand, bool>,
        IRequestHandler<EditUserCommand, bool>,
        IRequestHandler<EditUserRoleCommand, Unit>
    {
        private readonly IUserStore _userStore;
        private readonly IMapper _mapper;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserCommandHandler(IMapper mapper, IUserStore userStore, RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _userStore = userStore;
            _mapper = mapper;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (request.DTO.Password != request.DTO.ConfirmedPassword)
            {
                throw new InvalidParameterException("A megadott jelszó nem egyezik!");
            }
            if (!RegexUtilities.IsValidEmail(request.DTO.Email))
            {
                throw new InvalidParameterException("Nem megfelelő e-mail formátum!");
            }

            var user = _mapper.Map<ApplicationUser>(request.DTO);
            if (await _userStore.IsUserTaken(user.UserName, user.Email, cancellationToken))
            {
                throw new InvalidParameterException("Nem érvényes adatok!");
            }

            bool succeded = await _userStore.CreateUserAsync(user, request.DTO.Password, cancellationToken);
            if (succeded)
            {
                succeded = await _userStore.AddRoleToUser(user, "Regular", cancellationToken);
            }
            return succeded;
        }

        public async Task<bool> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<ApplicationUser>(request.DTO);
            if (await _userStore.IsValidAsync(user, cancellationToken))
            {
                throw new InvalidParameterException("Nem érvényes adatok!");
            }
            var userId = (await _userStore.GetActualUser(cancellationToken))?.Id
                ?? throw new EntityNotFoundException("User not found!");
            await _userStore.UpdateActualUserAsync(user, userId, cancellationToken);
            return true;
        }

        public async Task<Unit> Handle(EditUserRoleCommand request, CancellationToken cancellationToken)
        {
            var domain = await _userStore.GetUser(request.DTO.Id.ToString(), cancellationToken);
            await _userStore.DeleteRolesOfUser(domain, cancellationToken);
            await _userStore.AddRoleToUser(domain, request.DTO.Role, cancellationToken);
            
            return Unit.Value;
        }
    }
}
