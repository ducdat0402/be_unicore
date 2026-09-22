using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.DTO.Entity;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Announcement.UpdateAnnouncement
{
    public class UpdateAnnouncementHandler : IRequestHandler<UpdateAnnouncementRequestDTO, UpdateAnnouncementResponseDTO>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IAnnouncementStudentRepository _announcementStudentRepository;
        private readonly IAnnouncementAudienceService _audienceService;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateAnnouncementRequestDTO> _validator;

        public UpdateAnnouncementHandler(
            IAnnouncementRepository announcementRepository,
            IAnnouncementStudentRepository announcementStudentRepository,
            IAnnouncementAudienceService audienceService,
            IMapper mapper,
            IValidator<UpdateAnnouncementRequestDTO> validator)
        {
            _announcementRepository = announcementRepository;
            _announcementStudentRepository = announcementStudentRepository;
            _audienceService = audienceService;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<UpdateAnnouncementResponseDTO> HandleAsync(UpdateAnnouncementRequestDTO request, CancellationToken cancellationToken)
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

            if (!string.Equals(entity.Status, AnnouncementConstants.Status.Draft, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Only DRAFT announcements can be updated.");
            }

            await _audienceService.ValidateScopeAsync(
                request.ScopeType,
                request.ScopeValue,
                request.TargetStudentIds,
                cancellationToken);

            var scopeType = request.ScopeType.Trim().ToUpperInvariant();
            var scopeValue = AnnouncementScopeHelper.NormalizeScopeValue(scopeType, request.ScopeValue);

            entity.Title = request.Title.Trim();
            entity.Description = request.Description;
            entity.Content = request.Content;
            entity.Type = request.Type.Trim().ToUpperInvariant();
            entity.ScopeType = scopeType;
            entity.ScopeValue = scopeValue;
            entity.RequireAcknowledgement = request.RequireAcknowledgement;
            entity.PublishDate = request.PublishDate;
            entity.ExpiredDate = request.ExpiredDate;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = request.ActorUserId;

            await _announcementRepository.UpdateAsync(entity, cancellationToken);

            if (scopeType == AnnouncementConstants.Scope.Student)
            {
                var studentIds = AnnouncementScopeHelper.NormalizeStudentIds(request.TargetStudentIds);
                await _announcementStudentRepository.ReplaceRecipientsAsync(entity.Id, studentIds, cancellationToken);
            }
            else
            {
                // Clear any draft STUDENT rows if scope changed away from STUDENT.
                await _announcementStudentRepository.ReplaceRecipientsAsync(entity.Id, Array.Empty<string>(), cancellationToken);
            }

            var updated = await _announcementRepository.GetByIdWithDetailsAsync(entity.Id, cancellationToken);

            return new UpdateAnnouncementResponseDTO
            {
                Announcement = _mapper.Map<AnnouncementDTO>(updated)
            };
        }
    }
}
