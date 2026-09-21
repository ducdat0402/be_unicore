using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class UserProfileRepository : RepositoryEFCoreBase<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<UserProfile?> GetByUserIDAsync(string userId, CancellationToken ct = default)
        {
            var result = await _dbSet.Where(x => x.UserId.Equals(userId))
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(ct);
                                    
            return result;
        }
    }
}
