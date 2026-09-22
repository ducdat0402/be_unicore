using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Announcement.CancelAnnouncement
{
    public class CancelAnnouncementHandler : IRequestHandler<CancelAnnouncementRequestDTO, CancelAnnouncementResponseDTO>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CancelAnnouncementRequestDTO> _validator;

        public CancelAnnouncementHandler(
            IAnnouncementRepository announcementRepository,
            IMapper mapper,
            IValidator<CancelAnnouncementRequestDTO> validator)
        {
            _announcementRepository = announcementRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<CancelAnnouncementResponseDTO> HandleAsync(CancelAnnouncementRequestDTO request, CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var entity = await _announcementRepository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Announcement with ID {request.Id} not found.");
            }

            if (!string.Equals(entity.Status, AnnouncementConstants.Status.Published, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Only PUBLISHED announcements can be cancelled.");
            }

            entity.Status = AnnouncementConstants.Status.Cancelled;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = request.ActorUserId;

            await _announcementRepository.UpdateAsync(entity, cancellationToken);

            var cancelled = await _announcementRepository.GetByIdAsync(entity.Id, cancellationToken);

            return new CancelAnnouncementResponseDTO
            {
                Announcement = _mapper.Map<AnnouncementDTO>(cancelled)
            };
        }
    }
}
