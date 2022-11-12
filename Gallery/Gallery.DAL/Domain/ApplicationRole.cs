using Microsoft.AspNetCore.Identity;

namespace Gallery.DAL.Domain
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string Description { get; set; } = string.Empty;
    }
}
