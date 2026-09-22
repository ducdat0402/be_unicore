using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class UserRepository : RepositoryEFCoreBase<User>, IUserRepository
    {
        public UserRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(e => e.UserRoles)
                .ThenInclude(e => e.Role)
                .Include(e => e.UserPermissions)
                .ThenInclude(e => e.Permission)
                .FirstOrDefaultAsync(u => u.Email.Equals(email), cancellationToken);
        }

        public async Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(e => e.UserRoles)
                .ThenInclude(e => e.Role)
                .Include(e => e.UserPermissions)
                .ThenInclude(e => e.Permission)
                .FirstOrDefaultAsync(u => u.Id.Equals(id), cancellationToken);
        }

        public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(e => e.UserRoles)
                .ThenInclude(e => e.Role)
                .Include(e => e.UserPermissions)
                .ThenInclude(e => e.Permission)
                .FirstOrDefaultAsync(u => u.Username.Equals(username), cancellationToken);
        }

        public async Task<List<string>> GetActiveStudentIdsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(u =>
                    u.IsActive &&
                    u.UserRoles.Any(ur => ur.Role != null && ur.Role.Name == "Student" && ur.Role.IsActive))
                .Select(u => u.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<string>> GetActiveStudentIdsByIdsAsync(IEnumerable<string> studentIds, CancellationToken cancellationToken = default)
        {
            var ids = studentIds
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (ids.Count == 0)
            {
                return new List<string>();
            }

            return await _dbSet
                .AsNoTracking()
                .Where(u =>
                    ids.Contains(u.Id) &&
                    u.IsActive &&
                    u.UserRoles.Any(ur => ur.Role != null && ur.Role.Name == "Student" && ur.Role.IsActive))
                .Select(u => u.Id)
                .ToListAsync(cancellationToken);
        }
    }
}
