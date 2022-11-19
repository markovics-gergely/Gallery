using Gallery.DAL.Domain;
using Gallery.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.DAL.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<ApplicationUser> UserRepository { get; }

        IGenericRepository<Album> AlbumRepository { get; }

        IGenericRepository<Picture> PictureRepository { get; }

        Task Save();
    }
}
