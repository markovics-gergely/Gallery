using Gallery.BLL.Infrastructure.ViewModels;
using MediatR;

namespace Gallery.BLL.Infrastructure.Queries
{
    public class GetUserQuery : IRequest<ProfileWithNameViewModel>
    {
        public string? Id { get; set; }

        public GetUserQuery(string? id)
        {
            Id = id;
        }
    }
}
