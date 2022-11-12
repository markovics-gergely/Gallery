using Gallery.BLL.Infrastructure.ViewModels;
using MediatR;

namespace Gallery.BLL.Infrastructure.Queries
{
    public class GetProfileQuery : IRequest<ProfileViewModel>
    {
    }
}
