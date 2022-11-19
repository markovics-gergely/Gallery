using Gallery.DAL.Domain;
using Gallery.DAL.Repository.Implementations;
using Gallery.DAL.Repository.Interfaces;
using Gallery.DAL.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.DAL.UnitOfWork.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GalleryDbContext context;
        private readonly IGenericRepository<ApplicationUser> userRepository;
        private readonly IGenericRepository<Album> albumRepository;
        private readonly IGenericRepository<Picture> pictureRepository;

        public IGenericRepository<ApplicationUser> UserRepository => userRepository;

        public IGenericRepository<Album> AlbumRepository => albumRepository;

        public IGenericRepository<Picture> PictureRepository => pictureRepository;

        public UnitOfWork(GalleryDbContext context, 
            IGenericRepository<ApplicationUser> userRepository,
            IGenericRepository<Album> albumRepository,
            IGenericRepository<Picture> pictureRepository)
        {
            this.context = context;
            this.userRepository = userRepository;
            this.albumRepository = albumRepository;
            this.pictureRepository = pictureRepository;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
    }
}
