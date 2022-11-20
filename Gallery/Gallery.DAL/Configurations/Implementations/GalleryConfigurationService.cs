using Gallery.DAL.Configurations.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.DAL.Configurations.Implementations
{
    public class GalleryConfigurationService : IGalleryConfigurationService
    {
        private readonly GalleryConfiguration _config;
        private readonly string _defaultContentPath;
        private const string _staticRequestPath = "files";
        private const string _imagesRelativePath = "images";

        public GalleryConfigurationService(IOptions<GalleryConfiguration> options, IHostingEnvironment environment)
        {
            _defaultContentPath = environment.WebRootPath;
            _config = options.Value;
        }

        public int GetMaxUploadCount()
        {
            return _config.MaxUploadCount;
        }

        public int GetMaxUploadSize()
        {
            return _config.MaxUploadCount;
        }

        public string GetStaticFilePhysicalPath()
        {
            var webroot = string.IsNullOrWhiteSpace(_config.StaticFilePath) ? _defaultContentPath : _config.StaticFilePath;
            return webroot;
        }

        public string GetImagesSubdirectory()
        {
            return _imagesRelativePath;
        }

        public string GetStaticFileRequestPath()
        {
            return _staticRequestPath;
        }
    }
}
