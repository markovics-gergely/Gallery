using Gallery.DAL.Configurations;
using Gallery.DAL.Configurations.Interfaces;
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
        private readonly IGalleryConfigurationService _galleryConfiguration;

        public FileRepository(IGalleryConfigurationService galleryConfiguration)
        {
            _galleryConfiguration = galleryConfiguration;
        }

        public void DeleteFile(string filePath)
        {
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
            var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var userDir = $"{_galleryConfiguration.GetStaticFilePhysicalPath()}\\{_galleryConfiguration.GetImagesSubdirectory()}\\{userId}";
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
                Size = attributes.Length / Math.Pow(1024, 2),
                FileExtension = extension
            };
        }
    }
}
