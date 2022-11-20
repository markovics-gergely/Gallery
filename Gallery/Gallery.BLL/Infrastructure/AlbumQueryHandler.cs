using AutoMapper;
using Gallery.BLL.Extensions;
using Gallery.BLL.Infrastructure.Commands;
using Gallery.BLL.Infrastructure.Queries;
using Gallery.BLL.Infrastructure.ViewModels;
using Gallery.BLL.Validators.Implementations;
using Gallery.BLL.Validators.Interfaces;
using Gallery.DAL.Domain;
using Gallery.DAL.UnitOfWork.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Infrastructure
{
    public class AlbumQueryHandler :
        IRequestHandler<GetAlbumDetailsQuery, AlbumDetailsViewModel>,
        IRequestHandler<GetAlbumsQuery, EnumerableWithCountViewModel<AlbumListViewModel>>,
        IRequestHandler<GetUserFavoriteAlbumsQuery, EnumerableWithCountViewModel<AlbumListViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IValidator? _validator;

        public AlbumQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<AlbumDetailsViewModel> Handle(GetAlbumDetailsQuery request, CancellationToken cancellationToken)
        {
            var albumEntity = _unitOfWork.AlbumRepository.Get(
                filter: x => x.Id == request.AlbumId, 
                includeProperties: string.Join(',', nameof(Album.Pictures), nameof(Album.Creator), nameof(Album.FavoritedBy)))
                .FirstOrDefault();
            if (albumEntity == null)
            {
                throw new ArgumentException("Requested album not found", nameof(request));
            }

            if (request.User == null)
            {
                _validator = new PublicAlbumValidator(albumEntity);
            }
            else
            {
                _validator = new OrCondition(
                    new PublicAlbumValidator(albumEntity),
                    new AlbumOwnerValidator(albumEntity, request.User)
                    );
            }
            var isValid = _validator.Validate();
            if (!isValid)
            {
                throw new ArgumentException("Unauthorized access", nameof(request));
            }
            var albumViewModel = _mapper.Map<AlbumDetailsViewModel>(albumEntity);
            return Task.FromResult(albumViewModel);
        }

        public Task<EnumerableWithCountViewModel<AlbumListViewModel>> Handle(GetAlbumsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Album, bool>> filter = x => !x.IsPrivate;
            if (request.UserId != null)
            {
                var userId = request.UserId;
                filter = x => !x.IsPrivate && x.Creator.Id == userId;
            }
            var albumEntities = _unitOfWork.AlbumRepository.Get(
                filter: filter,
                orderBy: x => x.Skip((request.Dto.PageCount - 1) * request.Dto.PageSize).Take(request.Dto.PageSize).OrderBy(x => x.Name), 
                includeProperties: string.Join(',', nameof(Album.Pictures), nameof(Album.Creator))
                ).ToList();
            var albumsViewModelWithCount = _mapper.Map<EnumerableWithCountViewModel<AlbumListViewModel>>(albumEntities);
            return Task.FromResult(albumsViewModelWithCount);
        }

        public Task<EnumerableWithCountViewModel<AlbumListViewModel>> Handle(GetUserFavoriteAlbumsQuery request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(request.User.GetUserIdFromJwt());
            var albumEntities = _unitOfWork.UserRepository.Get(
                filter: x => x.Id == userId,
                includeProperties: string.Join(',', nameof(ApplicationUser.FavoritedAlbums), "FavoritedAlbums.Pictures")
                ).First();

            var albumsViewModelWithCount = _mapper.Map<EnumerableWithCountViewModel<AlbumListViewModel>>(
                albumEntities.FavoritedAlbums.Skip((request.Dto.PageCount - 1) * request.Dto.PageSize).Take(request.Dto.PageSize)
                );
            return Task.FromResult(albumsViewModelWithCount);
        }
    }
}
