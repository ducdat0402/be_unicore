using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Feature.v1.Announcement.GetPublicAnnouncements
{
    public class GetPublicAnnouncementsHandler : IRequestHandler<GetPublicAnnouncementsRequestDTO, GetPublicAnnouncementsResponseDTO>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IMapper _mapper;

        public GetPublicAnnouncementsHandler(IAnnouncementRepository announcementRepository, IMapper mapper)
        {
            _announcementRepository = announcementRepository;
            _mapper = mapper;
        }

        public async Task<GetPublicAnnouncementsResponseDTO> HandleAsync(
            GetPublicAnnouncementsRequestDTO request,
            CancellationToken cancellationToken)
        {
            var items = await _announcementRepository.GetPublishedPublicAsync(cancellationToken);

            return new GetPublicAnnouncementsResponseDTO
            {
                Items = _mapper.Map<List<AnnouncementDTO>>(items)
            };
        }
    }
}
