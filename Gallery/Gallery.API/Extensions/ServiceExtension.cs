using Gallery.BLL.Stores.Implementations;
using Gallery.BLL.Stores.Interfaces;
using Gallery.BLL.Infrastructure.Commands;
using Gallery.BLL.Infrastructure;
using Gallery.BLL.Infrastructure.Queries;
using Gallery.BLL.Infrastructure.ViewModels;
using MediatR;
using Gallery.DAL.Repository.Implementations;
using Gallery.DAL.Repository.Interfaces;
using Gallery.DAL.UnitOfWork.Interfaces;
using Gallery.DAL.UnitOfWork.Implementations;

namespace Gallery.API.Extensions
{
    /// <summary>
    /// Helper class for adding services
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// Add services for dependency injections
        /// </summary>
        /// <param name="services"></param>
        public static void AddServiceExtensions(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<IUserStore, UserStore>();
            services.AddTransient<IRequestHandler<CreateUserCommand, bool>, UserCommandHandler>();
            services.AddTransient<IRequestHandler<EditUserCommand, bool>, UserCommandHandler>();
            services.AddTransient<IRequestHandler<EditUserRoleCommand, Unit>, UserCommandHandler>();

            services.AddTransient<IRequestHandler<GetActualUserIdQuery, string?>, UserQueryHandler>();
            services.AddTransient<IRequestHandler<GetUserQuery, ProfileWithNameViewModel>, UserQueryHandler>();
            services.AddTransient<IRequestHandler<GetProfileQuery, ProfileViewModel>, UserQueryHandler>();
            services.AddTransient<IRequestHandler<GetFullProfileQuery, ProfileWithNameViewModel>, UserQueryHandler>();
            services.AddTransient<IRequestHandler<GetUsersByRoleQuery, IEnumerable<UserNameViewModel>>, UserQueryHandler>();

            services.AddTransient<IRequestHandler<GetAlbumDetailsQuery, AlbumDetailsViewModel>, AlbumQueryHandler>();
            services.AddTransient<IRequestHandler<GetAlbumsQuery, IEnumerable<AlbumListViewModel>>, AlbumQueryHandler>();

            services.AddTransient<IRequestHandler<EditFavoritesCommand, bool>, AlbumCommandHandler>();
            services.AddTransient<IRequestHandler<AddPicturesToAlbumCommand, bool>, AlbumCommandHandler>();
            services.AddTransient<IRequestHandler<CreateAlbumCommand, bool>, AlbumCommandHandler>();
            services.AddTransient<IRequestHandler<DeleteAlbumCommand, bool>, AlbumCommandHandler>();
            services.AddTransient<IRequestHandler<RemovePicturesFromAlbumCommand, bool>, AlbumCommandHandler>();
            services.AddTransient<IRequestHandler<LikeAlbumCommand, bool>, AlbumCommandHandler>();

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IFileRepository, FileRepository>();
        }
    }
}
