using AutoMapper;
using Gallery.BLL.Infrastructure.Commands;
using Gallery.BLL.Validators.Implementations;
using Gallery.BLL.Validators.Interfaces;
using Gallery.DAL.Domain;
using Gallery.DAL.Repository.Interfaces;
using Gallery.DAL.UnitOfWork.Interfaces;
using MediatR;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Internal;
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
        IRequestHandler<EditFavoritesCommand, bool>,
        IRequestHandler<AddPicturesToAlbumCommand, bool>,
        IRequestHandler<CreateAlbumCommand, bool>,
        IRequestHandler<DeleteAlbumCommand, bool>,
        IRequestHandler<RemovePicturesFromAlbumCommand, bool>,
        IRequestHandler<LikeAlbumCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileRepository _fileRepository;
        private readonly IMapper _mapper;
        private IValidator? _validator;

        public AlbumCommandHandler(
            IUnitOfWork unitOfWork, 
            IFileRepository fileRepository,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _fileRepository = fileRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(EditFavoritesCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(request.User.Claims.First(x => x.Type == "sub").Value);
            var user = _unitOfWork.UserRepository.Get(x => x.Id == userId, includeProperties: nameof(ApplicationUser.FavoritedAlbums)).First();
            var favorites = _unitOfWork.AlbumRepository.Get(filter: x => request.Dto.AlbumIds.Contains(x.Id)).ToList();
            user.FavoritedAlbums = favorites;
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.Save();
            return true;
        }

        public async Task<bool> Handle(AddPicturesToAlbumCommand request, CancellationToken cancellationToken)
        {
            var albumEntity = _unitOfWork.AlbumRepository.Get(filter: x => x.Id == request.AlbumId, includeProperties: nameof(Album.Pictures)).FirstOrDefault();
            if (albumEntity == null)
            {
                return false;
            }
            _validator = new AlbumOwnerValidator(albumEntity, request.User);
            var validatorResult = _validator.Validate();
            if (!validatorResult)
            {
                return false;
            }
            albumEntity.Pictures.Add(new Picture());
            _unitOfWork.AlbumRepository.Update(albumEntity);
            await _unitOfWork.Save();
            return true;
        }

        public async Task<bool> Handle(CreateAlbumCommand request, CancellationToken cancellationToken)
        {
            var albumEntity = _mapper.Map<Album>(request.Dto);
            var userId = Guid.Parse(request.User.Claims.First(c => c.Type == "sub").Value);
            var user = _unitOfWork.UserRepository.GetByID(userId);
            IList<Picture> pictures = new List<Picture>();
            foreach(var uploadedFile in request.Dto.Pictures)
            {
                if (uploadedFile.Length > 0)
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
                
            }
            albumEntity.Creator = user;
            albumEntity.Pictures = pictures;
            _unitOfWork.AlbumRepository.Insert(albumEntity);
            await _unitOfWork.Save();
            return true;
        }

        public async Task<bool> Handle(DeleteAlbumCommand request, CancellationToken cancellationToken)
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
                return false;
            }
            _unitOfWork.AlbumRepository.Delete(albumEntity);
            foreach (var picture in albumEntity.Pictures)
            {
                _fileRepository.DeleteFile(picture.PhysicalPath);
            }
            await _unitOfWork.Save();
            return true;
        }

        public async Task<bool> Handle(RemovePicturesFromAlbumCommand request, CancellationToken cancellationToken)
        {
            var pictureEntities = _unitOfWork.PictureRepository.Get(filter: x => request.Dto.PictureIds.Contains(x.Id), includeProperties: nameof(Picture.Album));
            foreach (var picture in pictureEntities)
            {
                _validator = new AlbumOwnerValidator(picture.Album, request.User);
                var validatorResult = _validator.Validate();
                if (!validatorResult)
                {
                    return false;
                }
            }
            foreach (var picture in pictureEntities)
            {
                _unitOfWork.PictureRepository.Delete(picture);
                _fileRepository.DeleteFile(picture.PhysicalPath);
            }
            await _unitOfWork.Save();
            return true;
        }

        public async Task<bool> Handle(LikeAlbumCommand request, CancellationToken cancellationToken)
        {
            var albumEntity = _unitOfWork.AlbumRepository.GetByID(request.AlbumId);
            albumEntity.LikeCount++;
            _unitOfWork.AlbumRepository.Update(albumEntity);
            await _unitOfWork.Save();
            return true;
        }
    }
}
