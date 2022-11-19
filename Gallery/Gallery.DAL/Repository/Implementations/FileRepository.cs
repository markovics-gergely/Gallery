using Gallery.DAL.Configurations;
using Gallery.DAL.Domain;
using Gallery.DAL.Repository.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.DAL.Repository.Implementations
{
    public class FileRepository : IFileRepository
    {
        private readonly GalleryApplication _galleryConfiguration;

        public FileRepository(IOptions<GalleryApplication> galleryConfiguration)
        {
            _galleryConfiguration = galleryConfiguration.Value;
        }

        public void DeleteFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException("File doesn't exist");
            }
            File.Delete(filePath);
        }

        public string SanitizeFilename(string fileName)
        {
            var file = Path.GetFileName(fileName);
            var htmlEncoded = WebUtility.HtmlEncode(file);
            return htmlEncoded;
        }

        public Picture SaveFile(Guid userId, string tempFilePath, string extension)
        {
            if (!File.Exists(tempFilePath))
            {
                throw new ArgumentException("File doesn't exist");
            }
            var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var userDir = $"{_galleryConfiguration.StaticFilePath}\\{userId}";
            var savePath = $"{userDir}\\{fileName}{extension}";
            if (!Directory.Exists(userDir))
            {
                Directory.CreateDirectory(userDir);
            }
            File.Copy(tempFilePath, savePath);
            File.Delete(tempFilePath);
            var attributes = new FileInfo(savePath);
            return new Picture
            {
                DisplayPath = $"/{userId}/{fileName}{extension}",
                PhysicalPath = savePath,
                Size = attributes.Length / Math.Pow(1024, 2)
            };
        }
    }
}
