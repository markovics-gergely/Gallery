using Gallery.DAL.Domain;

namespace Gallery.BLL.Stores.Interfaces
{
    public interface IUserStore
    {
        Task<bool> CreateUserAsync(ApplicationUser user, string password, CancellationToken cancellationToken);
        Task DeleteUserAsync(ApplicationUser user, CancellationToken cancellationToken);
        Task<ApplicationUser> GetUser(string id, CancellationToken cancellationToken);
        Task<IEnumerable<ApplicationUser>> GetUsersByRole(string role, CancellationToken cancellationToken);
        Task<ApplicationRole> GetRole(string name, CancellationToken cancellationToken);
        Task<IEnumerable<ApplicationRole>> GetOwnRoles(CancellationToken cancellationToken);
        Task<bool> AddRoleToUser(ApplicationUser user, string role, CancellationToken cancellationToken);
        Task<bool> DeleteRolesOfUser(ApplicationUser user, CancellationToken cancellationToken);
        Task<bool> IsUserTaken(string name, string email, CancellationToken cancellationToken);
        Task<ApplicationUser?> GetActualUser(CancellationToken cancellationToken);
        Task UpdateActualUserAsync(ApplicationUser user, Guid userId, CancellationToken cancellationToken);
        Task<bool> IsValidAsync(ApplicationUser user, CancellationToken cancellationToken);
    }
}
