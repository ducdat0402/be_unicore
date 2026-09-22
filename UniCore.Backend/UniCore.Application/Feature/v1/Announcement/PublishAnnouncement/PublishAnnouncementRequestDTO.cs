using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Announcement.PublishAnnouncement
{
    public class PublishAnnouncementRequestDTO : IRequest<PublishAnnouncementResponseDTO>
    {
        public string Id { get; set; } = string.Empty;
        public string? ActorUserId { get; set; }
    }
}
