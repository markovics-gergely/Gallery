using AutoMapper;
using Gallery.BLL.Exceptions;
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
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Infrastructure
{
    public class AlbumQueryHandler :
        IRequestHandler<GetOwnAlbumsQuery, EnumerableWithTotalViewModel<UserProfileViewModel>>,
        IRequestHandler<GetAlbumDetailsQuery, AlbumViewModel>,
        IRequestHandler<GetAlbumsQuery, EnumerableWithTotalViewModel<AlbumViewModel>>,
        IRequestHandler<GetUserFavoriteAlbumsQuery, EnumerableWithTotalViewModel<AlbumViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IValidator? _validator;
        private const int _albumPictureCount = 9;

        public AlbumQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<AlbumViewModel> Handle(GetAlbumDetailsQuery request, CancellationToken cancellationToken)
        {
            var albumEntity = _unitOfWork.AlbumRepository.Get(
                filter: x => x.Id == request.AlbumId, 
                includeProperties: string.Join(',', nameof(Album.Pictures), nameof(Album.Creator), nameof(Album.FavoritedBy)))
                .FirstOrDefault();
            if (albumEntity == null)
            {
                throw new EntityNotFoundException("Requested album not found");
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
                throw new ValidationErrorException("Cannot access this album");
            }
            var albumViewModel = _mapper.Map<AlbumViewModel>(albumEntity);
            return Task.FromResult(albumViewModel);
        }

        public Task<EnumerableWithTotalViewModel<AlbumViewModel>> Handle(GetAlbumsQuery request, CancellationToken cancellationToken)
        {
            var signedIn = request.User?.Identity?.IsAuthenticated ?? false;
            Expression<Func<Album, bool>> filter = x => !x.IsPrivate;
            if (request.UserId != null)
            {
                if (signedIn && Guid.Parse(request.User!.GetUserIdFromJwt()) == request.UserId)
                {
                    filter = x => x.Creator.Id == request.UserId;
                }
                else
                {
                    filter = x => !x.IsPrivate && x.Creator.Id == request.UserId;
                }
            }
            var albumEntities = _unitOfWork.AlbumRepository.Get(
                filter: filter,
                transform: x => x.AsNoTracking(),
                includeProperties: string.Join(',', nameof(Album.Pictures), nameof(Album.Creator), nameof(Album.FavoritedBy))
                ).ToList();

            var albumsViewModelWithCount = _mapper.Map<EnumerableWithTotalViewModel<AlbumViewModel>>(albumEntities);
            albumsViewModelWithCount.Values = albumsViewModelWithCount.Values.Skip((request.Dto.PageCount - 1) * request.Dto.PageSize).Take(request.Dto.PageSize);
            foreach (var album in albumsViewModelWithCount.Values)
            {
                album.Pictures = album.Pictures.Take(Math.Min(album.Pictures.Count(), _albumPictureCount));
            }
            return Task.FromResult(albumsViewModelWithCount);
        }

        public Task<EnumerableWithTotalViewModel<AlbumViewModel>> Handle(GetUserFavoriteAlbumsQuery request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(request.User.GetUserIdFromJwt());
            var albumEntities = _unitOfWork.UserRepository.Get(
                filter: x => x.Id == userId,
                transform: x => x.AsNoTracking(),
                includeProperties: string.Join(',', $"{nameof(ApplicationUser.FavoritedAlbums)}.{nameof(Album.Pictures)}", $"{nameof(ApplicationUser.FavoritedAlbums)}.{nameof(Album.Creator)}")
                ).First();

            var albumsViewModelWithCount = _mapper.Map<EnumerableWithTotalViewModel<AlbumViewModel>>(albumEntities.FavoritedAlbums);
            albumsViewModelWithCount.Values = albumsViewModelWithCount.Values.Skip((request.Dto.PageCount - 1) * request.Dto.PageSize).Take(request.Dto.PageSize);
            foreach (var album in albumsViewModelWithCount.Values)
            {
                album.Pictures = album.Pictures.Take(Math.Min(album.Pictures.Count(), _albumPictureCount));
            }
            return Task.FromResult(albumsViewModelWithCount);
        }

        public Task<EnumerableWithTotalViewModel<UserProfileViewModel>> Handle(GetOwnAlbumsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Album, bool>> filter = x => x.Creator.Id == Guid.Parse(request.User.GetUserIdFromJwt());
            var albumEntities = _unitOfWork.AlbumRepository.Get(
                filter: filter,
                transform: x => x.AsNoTracking(),
                includeProperties: string.Join(',', nameof(Album.Pictures))
                ).ToList();

            var albumsViewModelWithCount = _mapper.Map<EnumerableWithTotalViewModel<UserProfileViewModel>>(albumEntities);
            albumsViewModelWithCount.Values = albumsViewModelWithCount.Values.Skip((request.Dto.PageCount - 1) * request.Dto.PageSize).Take(request.Dto.PageSize);
            return Task.FromResult(albumsViewModelWithCount);
        }
    }
}
