using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface IAnnouncementStudentRepository : IRepository<AnnouncementStudent>
    {
        Task<List<AnnouncementStudent>> GetByAnnouncementIdAsync(string announcementId, CancellationToken cancellationToken = default);
        Task ReplaceRecipientsAsync(string announcementId, IEnumerable<string> studentIds, CancellationToken cancellationToken = default);
        Task<int> CountByAnnouncementIdAsync(string announcementId, CancellationToken cancellationToken = default);
        Task<(int Total, int Viewed, int Acknowledged)> GetDeliveryStatsAsync(string announcementId, CancellationToken cancellationToken = default);
    }
}
