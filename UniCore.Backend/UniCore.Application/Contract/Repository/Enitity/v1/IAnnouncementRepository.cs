using UniCore.Application.Entity;

namespace UniCore.Application.Contract.Repository.Enitity.v1
{
    public interface IAnnouncementRepository : IRepository<Announcement>
    {
        Task<Announcement?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<Announcement?> GetByIdWithDetailsAsync(string id, CancellationToken cancellationToken = default);
        Task<List<Announcement>> GetPublishedPublicAsync(CancellationToken cancellationToken = default);
        Task<Announcement?> GetPublishedPublicByIdAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get active announcements targeted to a specific student (via AnnouncementStudent or PUBLIC scope).
        /// </summary>
        /// <param name="typeFilter">Filter by type (NORMAL/IMPORTANT/URGENT).</param>
        Task<(List<Announcement> Items, int TotalCount)> GetAnnouncementsForStudentAsync(
            string studentId,
            string? typeFilter,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default);
    }
}
