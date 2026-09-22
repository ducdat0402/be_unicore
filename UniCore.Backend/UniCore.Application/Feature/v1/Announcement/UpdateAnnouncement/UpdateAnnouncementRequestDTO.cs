using System.Text.Json.Serialization;
using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Announcement.UpdateAnnouncement
{
    public class UpdateAnnouncementRequestDTO : IRequest<UpdateAnnouncementResponseDTO>
    {
        [JsonIgnore]
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string ScopeType { get; set; } = string.Empty;
        public string? ScopeValue { get; set; }
        public List<string> TargetStudentIds { get; set; } = new();
        public bool RequireAcknowledgement { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime? ExpiredDate { get; set; }

        [JsonIgnore]
        public string? ActorUserId { get; set; }
    }
}
