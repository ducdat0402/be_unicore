using UniCore.Application.Contract.RequestHandlerHub;

namespace UniCore.Application.Feature.v1.Announcement.PreviewRecipients
{
    public class PreviewRecipientsRequestDTO : IRequest<PreviewRecipientsResponseDTO>
    {
        public string ScopeType { get; set; } = string.Empty;
        public string? ScopeValue { get; set; }
        public List<string> TargetStudentIds { get; set; } = new();
    }
}
