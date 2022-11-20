using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.DAL.Configurations.Interfaces
{
    public interface IGalleryConfigurationService
    {
        int GetMaxUploadCount();

        int GetMaxUploadSize();

        string GetStaticFilePhysicalPath();

        string GetStaticFileRequestPath();

        string GetImagesSubdirectory();
    }
}
