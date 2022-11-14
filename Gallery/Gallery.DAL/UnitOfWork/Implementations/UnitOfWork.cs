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
        private IGenericRepository<ApplicationUser>? userRepository;
        private IGenericRepository<Album>? albumRepository;
        private IGenericRepository<Picture>? pictureRepository;

        public IGenericRepository<ApplicationUser> UserRepository
        {
            get
            {
                userRepository ??= new GenericRepository<ApplicationUser>(context);
                return userRepository;
            }
        }

        public IGenericRepository<Album> AlbumRepository
        {
            get
            {
                albumRepository ??= new GenericRepository<Album>(context);
                return albumRepository;
            }
        }

        public IGenericRepository<Picture> PictureRepository
        {
            get
            {
                pictureRepository ??= new GenericRepository<Picture>(context);
                return pictureRepository;
            }
        }

        public UnitOfWork(GalleryDbContext context)
        {
            this.context = context;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
    }
}
