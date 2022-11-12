using AutoMapper;
using Gallery.BLL.Infrastructure.Queries;
using Gallery.BLL.Infrastructure.ViewModels;
using Gallery.BLL.Stores.Interfaces;
using MediatR;

namespace Gallery.BLL.Infrastructure
{
    public class UserQueryHandler :
        IRequestHandler<GetUserQuery, ProfileWithNameViewModel>,
        IRequestHandler<GetProfileQuery, ProfileViewModel>,
        IRequestHandler<GetFullProfileQuery, ProfileWithNameViewModel>,
        IRequestHandler<GetActualUserIdQuery, string?>,
        IRequestHandler<GetUsersByRoleQuery, IEnumerable<UserNameViewModel>>
    {
        private readonly IUserStore _userStore;
        private readonly IMapper _mapper;

        public UserQueryHandler(IMapper mapper, IUserStore userStore)
        {
            _userStore = userStore;
            _mapper = mapper;
        }

        public async Task<ProfileWithNameViewModel> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var domain = request.Id != null ?
                await _userStore.GetUser(request.Id, cancellationToken).ConfigureAwait(false) :
                await _userStore.GetActualUser(cancellationToken);

            return _mapper.Map<ProfileWithNameViewModel>(domain);
        }

        public async Task<string?> Handle(GetActualUserIdQuery request, CancellationToken cancellationToken)
        {
            return (await _userStore.GetActualUser(cancellationToken))?.Id.ToString();
        }

        public async Task<ProfileViewModel> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ProfileViewModel>(await _userStore.GetActualUser(cancellationToken));
        }

        public async Task<ProfileWithNameViewModel> Handle(GetFullProfileQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ProfileWithNameViewModel>(await _userStore.GetActualUser(cancellationToken));
        }

        public async Task<IEnumerable<UserNameViewModel>> Handle(GetUsersByRoleQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<UserNameViewModel>>(await _userStore.GetUsersByRole(request.Role, cancellationToken));
        }
    }
}
