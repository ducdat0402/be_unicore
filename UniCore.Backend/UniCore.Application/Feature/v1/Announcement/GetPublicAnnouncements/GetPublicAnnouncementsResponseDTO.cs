using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Feature.v1.Announcement.GetPublicAnnouncements
{
    public class GetPublicAnnouncementsResponseDTO
    {
        public List<AnnouncementDTO> Items { get; set; } = new();
    }
}
