using Gallery.DAL.Configurations;
using Gallery.DAL.Configurations.Implementations;
using Gallery.DAL.Configurations.Interfaces;

namespace Gallery.API.Extensions
{
    public static class ConfigurationExtension
    {
        public static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GalleryConfiguration>(configuration.GetSection("GalleryApplication"));
            services.AddTransient<IGalleryConfigurationService, GalleryConfigurationService>();
        }
    }
}
