using AutoMapper;
using Gallery.BLL.Exceptions;
using Gallery.BLL.Extensions;
using Gallery.BLL.Infrastructure.Commands;
using Gallery.BLL.Validators.Implementations;
using Gallery.BLL.Validators.Interfaces;
using Gallery.DAL.Configurations;
using Gallery.DAL.Configurations.Interfaces;
using Gallery.DAL.Domain;
using Gallery.DAL.Repository.Interfaces;
using Gallery.DAL.UnitOfWork.Interfaces;
using MediatR;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Gallery.BLL.Infrastructure
{
    public class AlbumCommandHandler :
        IRequestHandler<AddFavoriteCommand, Unit>,
        IRequestHandler<RemoveFavoriteCommand, Unit>,
        IRequestHandler<AddPicturesToAlbumCommand, Unit>,
        IRequestHandler<CreateAlbumCommand, Guid>,
        IRequestHandler<DeleteAlbumCommand, Unit>,
        IRequestHandler<RemovePicturesFromAlbumCommand, Unit>,
        IRequestHandler<LikeAlbumCommand, Unit>,
        IRequestHandler<EditAlbumDataCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileRepository _fileRepository;
        private readonly IGalleryConfigurationService _galleryConfiguration;
        private readonly IMapper _mapper;
        private IValidator? _validator;

        public AlbumCommandHandler(
            IUnitOfWork unitOfWork, 
            IFileRepository fileRepository,
            IGalleryConfigurationService galleryConfiguration,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _fileRepository = fileRepository;
            _galleryConfiguration = galleryConfiguration;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(AddFavoriteCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(request.User.GetUserIdFromJwt());
            var user = _unitOfWork.UserRepository.Get(x => x.Id == userId, includeProperties: nameof(ApplicationUser.FavoritedAlbums)).First();
            var favorite = _unitOfWork.AlbumRepository.GetByID(request.AlbumId);
            user.FavoritedAlbums.Add(favorite);
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.Save();
            return Unit.Value;
        }

        public async Task<Unit> Handle(RemoveFavoriteCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(request.User.GetUserIdFromJwt());
            var user = _unitOfWork.UserRepository.Get(x => x.Id == userId, includeProperties: nameof(ApplicationUser.FavoritedAlbums)).First();
            var favorite = _unitOfWork.AlbumRepository.GetByID(request.AlbumId);
            user.FavoritedAlbums.Remove(favorite);
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.Save();
            return Unit.Value;
        }

        public async Task<Unit> Handle(AddPicturesToAlbumCommand request, CancellationToken cancellationToken)
        {
            _validator = new EnumerableMaxCountValidator<IFormFile>(request.Dto.Pictures, _galleryConfiguration.GetMaxUploadCount());
            if (!_validator.Validate())
            {
                throw new ValidationErrorException("Too many files provided");
            }
            var albumEntity = _unitOfWork.AlbumRepository.Get(
                filter: x => x.Id == request.AlbumId, 
                includeProperties: string.Join(',', nameof(Album.Pictures), nameof(Album.Creator)))
                .FirstOrDefault();
            if (albumEntity == null)
            {
                throw new EntityNotFoundException("Album not found");
            }
            _validator = new AlbumOwnerValidator(albumEntity, request.User);
            var validatorResult = _validator.Validate();
            if (!validatorResult)
            {
                throw new ValidationErrorException("Not authorized to add pictures");
            }
            foreach (var uploadedFile in request.Dto.Pictures)
            {
                _validator = new AndCondition(
                    new FileSizeValidator(uploadedFile, _galleryConfiguration.GetMaxUploadSize()),
                    new FileHeaderValidator(uploadedFile)
                    );
                if (!_validator.Validate())
                {
                    throw new ValidationErrorException("There was a problem with the provided files");
                }
            }
            IList<Picture> pictures = new List<Picture>();
            var userId = Guid.Parse(request.User.GetUserIdFromJwt());
            foreach (var uploadedFile in request.Dto.Pictures)
            {
                var filePath = Path.GetTempFileName();
                var sanitizedFilename = _fileRepository.SanitizeFilename(uploadedFile.FileName);
                var extension = Path.GetExtension(sanitizedFilename);
                using (var stream = File.Create(filePath))
                {
                    await uploadedFile.CopyToAsync(stream);
                }
                var savedPicture = _fileRepository.SaveFile(userId, filePath, extension);
                pictures.Add(savedPicture);
            }
            albumEntity.Pictures = albumEntity.Pictures.Concat(pictures).ToList();
            _unitOfWork.AlbumRepository.Update(albumEntity);
            await _unitOfWork.Save();
            return Unit.Value;
        }

        public async Task<Guid> Handle(CreateAlbumCommand request, CancellationToken cancellationToken)
        {
            _validator = new EnumerableMaxCountValidator<IFormFile>(request.Dto.Pictures, _galleryConfiguration.GetMaxUploadCount());
            if (!_validator.Validate())
            {
                throw new ValidationErrorException("Too many files provided");
            }
            var albumEntity = _mapper.Map<Album>(request.Dto);
            var userId = Guid.Parse(request.User.GetUserIdFromJwt());
            var user = _unitOfWork.UserRepository.GetByID(userId);
            foreach(var uploadedFile in request.Dto.Pictures)
            {
                _validator = new AndCondition( 
                    new FileSizeValidator(uploadedFile, _galleryConfiguration.GetMaxUploadSize()),
                    new FileHeaderValidator(uploadedFile)
                    );
                if (!_validator.Validate())
                {
                    throw new ValidationErrorException("There was a problem with the provided files");
                }
            }

            IList<Picture> pictures = new List<Picture>();
            foreach (var uploadedFile in request.Dto.Pictures)
            {
                var filePath = Path.GetTempFileName();
                var sanitizedFilename = _fileRepository.SanitizeFilename(uploadedFile.FileName);
                var extension = Path.GetExtension(sanitizedFilename);
                using (var stream = File.Create(filePath))
                {
                    await uploadedFile.CopyToAsync(stream);
                }
                var savedPicture = _fileRepository.SaveFile(userId, filePath, extension);
                pictures.Add(savedPicture);
            }
            albumEntity.Creator = user;
            albumEntity.Pictures = pictures;
            _unitOfWork.AlbumRepository.Insert(albumEntity);
            await _unitOfWork.Save();
            return albumEntity.Id;
        }

        public async Task<Unit> Handle(DeleteAlbumCommand request, CancellationToken cancellationToken)
        {
            var albumEntity = _unitOfWork.AlbumRepository.Get(
                filter: x => x.Id == request.AlbumId, 
                includeProperties: string.Join(',', nameof(Album.Creator), nameof(Album.Pictures))
                ).First();
            _validator = new OrCondition(
                new AlbumOwnerValidator(albumEntity, request.User),
                new AndCondition(new AdminRoleValidator(request.User), new PublicAlbumValidator(albumEntity))
                );

            var validatorResult = _validator.Validate();
            if (!validatorResult)
            {
                throw new ValidationErrorException("Cannot delete selected album");
            }
            _unitOfWork.AlbumRepository.Delete(albumEntity);
            foreach (var picture in albumEntity.Pictures)
            {
                _fileRepository.DeleteFile(picture.PhysicalPath);
            }
            await _unitOfWork.Save();
            return Unit.Value;
        }

        public async Task<Unit> Handle(RemovePicturesFromAlbumCommand request, CancellationToken cancellationToken)
        {
            var pictureEntities = _unitOfWork.PictureRepository.Get(
                filter: x => request.Dto.PictureIds.Contains(x.Id), 
                includeProperties: $"{nameof(Picture.Album)}.{nameof(Album.Creator)}"
                );
            foreach (var picture in pictureEntities)
            {
                _validator = new AlbumOwnerValidator(picture.Album, request.User);
                var validatorResult = _validator.Validate();
                if (!validatorResult)
                {
                    throw new ValidationErrorException("Cannot remove pictures from this album");
                }
            }
            foreach (var picture in pictureEntities)
            {
                _unitOfWork.PictureRepository.Delete(picture);
                _fileRepository.DeleteFile(picture.PhysicalPath);
            }
            await _unitOfWork.Save();
            return Unit.Value;
        }

        public async Task<Unit> Handle(LikeAlbumCommand request, CancellationToken cancellationToken)
        {
            var albumEntity = _unitOfWork.AlbumRepository.GetByID(request.AlbumId);
            albumEntity.LikeCount++;
            _unitOfWork.AlbumRepository.Update(albumEntity);
            await _unitOfWork.Save();
            return Unit.Value;
        }

        public async Task<Unit> Handle(EditAlbumDataCommand request, CancellationToken cancellationToken)
        {
            var albumEntity = _unitOfWork.AlbumRepository.Get(filter: x => x.Id == request.AlbumId, includeProperties: nameof(Album.Creator)).FirstOrDefault();
            if (albumEntity == null)
            {
                throw new EntityNotFoundException("Album not found");
            }
            _validator = new AlbumOwnerValidator(albumEntity, request.User);
            if (!_validator.Validate())
            {
                throw new ValidationErrorException("Cannot modify this album");
            }
            if (request.Dto.Name != null)
            {
                albumEntity.Name = request.Dto.Name;
            }
            if (request.Dto.IsPrivate != null)
            {
                albumEntity.IsPrivate = request.Dto.IsPrivate.Value;
            }

            _unitOfWork.AlbumRepository.Update(albumEntity);
            await _unitOfWork.Save();
            return Unit.Value;
        }
    }
}
