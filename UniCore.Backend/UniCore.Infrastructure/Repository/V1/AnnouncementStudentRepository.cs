using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Entity;
using UniCore.Infrastructure.Database;
using UniCore.Infrastructure.Repository.Base;

namespace UniCore.Infrastructure.Repository.V1
{
    public class AnnouncementStudentRepository : RepositoryEFCoreBase<AnnouncementStudent>, IAnnouncementStudentRepository
    {
        public AnnouncementStudentRepository(UniCoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<List<AnnouncementStudent>> GetByAnnouncementIdAsync(string announcementId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(s => s.Student)
                .Where(s => s.AnnouncementId == announcementId)
                .OrderBy(s => s.Student.Username)
                .ToListAsync(cancellationToken);
        }

        public async Task ReplaceRecipientsAsync(string announcementId, IEnumerable<string> studentIds, CancellationToken cancellationToken = default)
        {
            await _dbSet.Where(s => s.AnnouncementId == announcementId).ExecuteDeleteAsync(cancellationToken);

            var distinctIds = studentIds
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (distinctIds.Count == 0)
            {
                return;
            }

            var now = DateTime.UtcNow;
            var entities = distinctIds.Select(studentId => new AnnouncementStudent
            {
                Id = Guid.NewGuid().ToString(),
                AnnouncementId = announcementId,
                StudentId = studentId,
                CreatedAt = now
            });

            await _dbSet.AddRangeAsync(entities, cancellationToken);
            await _UniCoreDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> CountByAnnouncementIdAsync(string announcementId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().CountAsync(s => s.AnnouncementId == announcementId, cancellationToken);
        }

        public async Task<(int Total, int Viewed, int Acknowledged)> GetDeliveryStatsAsync(string announcementId, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsNoTracking().Where(s => s.AnnouncementId == announcementId);
            var total = await query.CountAsync(cancellationToken);
            var viewed = await query.CountAsync(s => s.ViewedAt != null, cancellationToken);
            var acknowledged = await query.CountAsync(s => s.AcknowledgedAt != null, cancellationToken);
            return (total, viewed, acknowledged);
        }
    }
}
