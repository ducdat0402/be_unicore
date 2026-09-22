using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.Contract.UnitOfWork;
using UniCore.Application.DTO.Entity;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Announcement.PublishAnnouncement
{
    public class PublishAnnouncementHandler : IRequestHandler<PublishAnnouncementRequestDTO, PublishAnnouncementResponseDTO>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IAnnouncementStudentRepository _announcementStudentRepository;
        private readonly IAnnouncementAudienceService _audienceService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<PublishAnnouncementRequestDTO> _validator;

        public PublishAnnouncementHandler(
            IAnnouncementRepository announcementRepository,
            IAnnouncementStudentRepository announcementStudentRepository,
            IAnnouncementAudienceService audienceService,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<PublishAnnouncementRequestDTO> validator)
        {
            _announcementRepository = announcementRepository;
            _announcementStudentRepository = announcementStudentRepository;
            _audienceService = audienceService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<PublishAnnouncementResponseDTO> HandleAsync(PublishAnnouncementRequestDTO request, CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var entity = await _announcementRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Announcement with ID {request.Id} not found.");
            }

            if (!string.Equals(entity.Status, AnnouncementConstants.Status.Draft, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Only DRAFT announcements can be published.");
            }

            var scopeType = entity.ScopeType.Trim().ToUpperInvariant();
            int? recipientCount = null;

            await using var tx = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                if (scopeType == AnnouncementConstants.Scope.Public)
                {
                    // Guest-readable: no account snapshot.
                    await _announcementStudentRepository.ReplaceRecipientsAsync(
                        entity.Id,
                        Array.Empty<string>(),
                        cancellationToken);
                    recipientCount = null;
                }
                else if (scopeType == AnnouncementConstants.Scope.Student)
                {
                    // Targets already stored in announcement_students at draft time.
                    var count = await _announcementStudentRepository.CountByAnnouncementIdAsync(entity.Id, cancellationToken);
                    if (count == 0)
                    {
                        throw new InvalidOperationException(
                            "Cannot publish STUDENT announcement without target students in announcement_students.");
                    }
                    recipientCount = count;
                }
                else
                {
                    var studentIds = await _audienceService.ResolveStudentIdsAsync(
                        entity.ScopeType,
                        entity.ScopeValue,
                        null,
                        cancellationToken);

                    if (studentIds.Count == 0)
                    {
                        throw new InvalidOperationException("Cannot publish announcement with zero recipients.");
                    }

                    await _announcementStudentRepository.ReplaceRecipientsAsync(
                        entity.Id,
                        studentIds,
                        cancellationToken);
                    recipientCount = studentIds.Count;
                }

                entity.Status = AnnouncementConstants.Status.Published;
                entity.RecipientCount = recipientCount;
                entity.PublishDate ??= DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;
                entity.UpdatedBy = request.ActorUserId;

                await _announcementRepository.UpdateAsync(entity, cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }

            var published = await _announcementRepository.GetByIdWithDetailsAsync(entity.Id, cancellationToken);

            return new PublishAnnouncementResponseDTO
            {
                Announcement = _mapper.Map<AnnouncementDTO>(published)
            };
        }
    }
}
