using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.DTO.Entity;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Announcement.CreateAnnouncement
{
    public class CreateAnnouncementHandler : IRequestHandler<CreateAnnouncementRequestDTO, CreateAnnouncementResponseDTO>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IAnnouncementStudentRepository _announcementStudentRepository;
        private readonly IAnnouncementAudienceService _audienceService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateAnnouncementRequestDTO> _validator;

        public CreateAnnouncementHandler(
            IAnnouncementRepository announcementRepository,
            IAnnouncementStudentRepository announcementStudentRepository,
            IAnnouncementAudienceService audienceService,
            IMapper mapper,
            IValidator<CreateAnnouncementRequestDTO> validator)
        {
            _announcementRepository = announcementRepository;
            _announcementStudentRepository = announcementStudentRepository;
            _audienceService = audienceService;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<CreateAnnouncementResponseDTO> HandleAsync(CreateAnnouncementRequestDTO request, CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            await _audienceService.ValidateScopeAsync(
                request.ScopeType,
                request.ScopeValue,
                request.TargetStudentIds,
                cancellationToken);

            var scopeType = request.ScopeType.Trim().ToUpperInvariant();
            var scopeValue = AnnouncementScopeHelper.NormalizeScopeValue(scopeType, request.ScopeValue);
            var now = DateTime.UtcNow;

            var entity = new UniCore.Application.Entity.Announcement
            {
                Id = Guid.NewGuid().ToString(),
                Title = request.Title.Trim(),
                Description = request.Description,
                Content = request.Content,
                Type = request.Type.Trim().ToUpperInvariant(),
                Status = AnnouncementConstants.Status.Draft,
                ScopeType = scopeType,
                ScopeValue = scopeValue,
                RequireAcknowledgement = request.RequireAcknowledgement,
                PublishDate = request.PublishDate,
                ExpiredDate = request.ExpiredDate,
                CreatedAt = now,
                CreatedBy = request.ActorUserId
            };

            await _announcementRepository.AddAsync(entity, cancellationToken);

            if (scopeType == AnnouncementConstants.Scope.Student)
            {
                var studentIds = AnnouncementScopeHelper.NormalizeStudentIds(request.TargetStudentIds);
                await _announcementStudentRepository.ReplaceRecipientsAsync(entity.Id, studentIds, cancellationToken);
            }

            var created = await _announcementRepository.GetByIdWithDetailsAsync(entity.Id, cancellationToken);

            return new CreateAnnouncementResponseDTO
            {
                Announcement = _mapper.Map<AnnouncementDTO>(created)
            };
        }
    }
}
