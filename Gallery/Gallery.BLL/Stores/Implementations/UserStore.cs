using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Gallery.BLL.Stores.Interfaces;
using Gallery.DAL;
using Gallery.DAL.Domain;
using Gallery.BLL.Exceptions;

namespace Gallery.BLL.Stores.Implementations
{
    public class UserStore : IUserStore
    {
        private readonly GalleryDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserStore(GalleryDbContext dbContext, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApplicationUser> GetUser(string id, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == Guid.Parse(id))
                ?? throw new EntityNotFoundException("User not found");
            return user;
        }

        public async Task<bool> IsUserTaken(string name, string email, CancellationToken cancellationToken)
        {
            return await _dbContext.Users.AsNoTracking().AnyAsync(u => u.UserName == name || u.Email == email, cancellationToken);
        }

        public async Task<bool> CreateUserAsync(ApplicationUser user, string password, CancellationToken cancellationToken)
        {
            if (await IsUserTaken(user.UserName, user.Email, cancellationToken))
            {
                throw new InvalidParameterException("Username or email is already used!");
            }

            var result = await _userManager.CreateAsync(user, password);

            if (result.Errors.Any())
            {
                throw new InvalidParameterException(string.Join('\n', result.Errors.Select(e => e.Description).ToList()));
            }

            return result.Succeeded;
        }

        public async Task<ApplicationUser?> GetActualUser(CancellationToken cancellationToken)
        {
            var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            if (userId == null) return null;
            return await GetUser(userId, cancellationToken);
        }

        public async Task DeleteUserAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var dUser = await _userManager.FindByNameAsync(user.UserName) ?? throw new InvalidParameterException("User not found!");
            await _userManager.DeleteAsync(dUser);
        }

        public async Task<ApplicationRole> GetRole(string name, CancellationToken cancellationToken)
        {
            return await _dbContext.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Name == name, cancellationToken)
                ?? throw new EntityNotFoundException("Role not found");
        }

        public async Task<bool> AddRoleToUser(ApplicationUser user, string role, CancellationToken cancellationToken)
        {
            var roleDomain = await GetRole(role, cancellationToken);
            var userRole = new IdentityUserRole<Guid>
            {
                UserId = user.Id,
                RoleId = roleDomain.Id
            };
            await _dbContext.UserRoles.AddAsync(userRole, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task UpdateActualUserAsync(ApplicationUser user, Guid userId, CancellationToken cancellationToken)
        {
            var domain = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            if (domain != null)
            {
                domain.FirstName = user.FirstName;
                domain.LastName = user.LastName;
                domain.UserName = user.UserName;
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> IsValidAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var domain = await GetActualUser(cancellationToken);
            return await _dbContext.Users
                .AsNoTracking()
                .AnyAsync(u => u.Id != domain.Id &&
                (
                    u.Email == user.Email ||
                    u.UserName == user.UserName
                ),
                cancellationToken);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersByRole(string role, CancellationToken cancellationToken)
        {
            var roleId = (await GetRole(role, cancellationToken)).Id;
            return await _dbContext.Users.AsNoTracking()
                .Include(u => u.UserRoles)
                .Where(u => u.UserRoles.Any(r => r.RoleId == roleId))
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> DeleteRolesOfUser(ApplicationUser user, CancellationToken cancellationToken)
        {
            var userRoles = await _dbContext.UserRoles
                .AsNoTracking()
                .Where(ur => ur.UserId == user.Id)
                .ToListAsync(cancellationToken);

            _dbContext.UserRoles.RemoveRange(userRoles);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<IEnumerable<ApplicationRole>> GetOwnRoles(CancellationToken cancellationToken)
        {
            var user = await GetActualUser(cancellationToken);
            if (user == null) throw new EntityNotFoundException("User not found");
            var roleIds = (await _dbContext.UserRoles
                .AsNoTracking()
                .Where(ur => ur.UserId == user.Id)
                .ToListAsync(cancellationToken))
                .Select(ur => ur.RoleId);
            return await _dbContext.Roles
                .AsNoTracking()
                .Where(r => roleIds.Contains(r.Id))
                .ToListAsync(cancellationToken);
        }
    }
}
