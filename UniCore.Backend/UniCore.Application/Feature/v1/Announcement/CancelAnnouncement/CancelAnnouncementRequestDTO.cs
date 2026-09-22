using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Announcement.CancelAnnouncement
{
    public class CancelAnnouncementRequestDTO : IRequest<CancelAnnouncementResponseDTO>
    {
        public string Id { get; set; } = string.Empty;
        public string? ActorUserId { get; set; }
    }
}
