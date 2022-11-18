using AutoMapper;
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
        IRequestHandler<GetAlbumsQuery, IEnumerable<AlbumListViewModel>>
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
                includeProperties: string.Join(nameof(Album.Pictures), nameof(Album.Creator)))
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

        public Task<IEnumerable<AlbumListViewModel>> Handle(GetAlbumsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Album, bool>> filter = x => !x.IsPrivate;
            if (request.User != null)
            {
                var userId = Guid.Parse(request.User.Claims.First(x => x.Type == "sub").Value);
                filter = x => !x.IsPrivate || x.Creator.Id == userId;
            }
            var albumEntities = _unitOfWork.AlbumRepository.Get(
                filter: filter,
                orderBy: x => x.Skip((request.PageCount - 1) * request.PageSize).Take(request.PageSize).OrderByDescending(x => x.Name), 
                includeProperties: string.Join(',', nameof(Album.Pictures), nameof(Album.Creator))
                );
            var albumsViewModel = _mapper.Map<IEnumerable<AlbumListViewModel>>(albumEntities);
            return Task.FromResult(albumsViewModel);
        }
    }
}
