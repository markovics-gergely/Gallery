using Gallery.BLL.Configurations;

namespace Gallery.API.Extensions
{
    public static class ConfigurationExtension
    {
        public static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GalleryApplication>(configuration.GetSection("GalleryApplication"));
        }
    }
}
