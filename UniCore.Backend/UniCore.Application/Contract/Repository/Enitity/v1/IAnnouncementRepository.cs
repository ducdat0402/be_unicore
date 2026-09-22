using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface IAnnouncementRepository : IRepository<Announcement>
    {
        Task<Announcement?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<Announcement?> GetByIdWithDetailsAsync(string id, CancellationToken cancellationToken = default);
        Task<List<Announcement>> GetPublishedPublicAsync(CancellationToken cancellationToken = default);
        Task<Announcement?> GetPublishedPublicByIdAsync(string id, CancellationToken cancellationToken = default);
    }
}
