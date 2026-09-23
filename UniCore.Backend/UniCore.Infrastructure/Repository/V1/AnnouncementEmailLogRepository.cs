using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class AnnouncementEmailLogRepository : RepositoryEFCoreBase<AnnouncementEmailLog>, IAnnouncementEmailLogRepository
    {
        public AnnouncementEmailLogRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task AddRangeLogsAsync(IEnumerable<AnnouncementEmailLog> logs, CancellationToken cancellationToken = default)
        {
            var list = logs.ToList();
            if (list.Count == 0)
            {
                return;
            }

            await _dbSet.AddRangeAsync(list, cancellationToken);
            await _UniCoreDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
