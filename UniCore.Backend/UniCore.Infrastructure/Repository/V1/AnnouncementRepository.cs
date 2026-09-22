using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Helper.Constant;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class AnnouncementRepository : RepositoryEFCoreBase<Announcement>, IAnnouncementRepository
    {
        public AnnouncementRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<Announcement?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<Announcement?> GetByIdWithDetailsAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(a => a.AnnouncementStudents)
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<List<Announcement>> GetPublishedPublicAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            return await _dbSet
                .AsNoTracking()
                .Where(a =>
                    a.Status == AnnouncementConstants.Status.Published &&
                    a.ScopeType == AnnouncementConstants.Scope.Public &&
                    (a.PublishDate == null || a.PublishDate <= now) &&
                    (a.ExpiredDate == null || a.ExpiredDate > now))
                .OrderByDescending(a => a.PublishDate ?? a.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<Announcement?> GetPublishedPublicByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(a =>
                    a.Id == id &&
                    a.Status == AnnouncementConstants.Status.Published &&
                    a.ScopeType == AnnouncementConstants.Scope.Public &&
                    (a.PublishDate == null || a.PublishDate <= now) &&
                    (a.ExpiredDate == null || a.ExpiredDate > now),
                    cancellationToken);
        }
    }
}
