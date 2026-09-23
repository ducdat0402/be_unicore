using FluentValidation;
using FluentValidation.Results;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.Feature.v1.Announcement;

namespace UniCore.Application.Feature.v1.Announcement.GetAnnouncementById
{
    public class GetAnnouncementByIdHandler : IRequestHandler<GetAnnouncementByIdRequestDTO, GetAnnouncementByIdResponseDTO>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IValidator<GetAnnouncementByIdRequestDTO> _validator;

        public GetAnnouncementByIdHandler(
            IAnnouncementRepository announcementRepository,
            IValidator<GetAnnouncementByIdRequestDTO> validator)
        {
            _announcementRepository = announcementRepository;
            _validator = validator;
        }

        public async Task<GetAnnouncementByIdResponseDTO> HandleAsync(GetAnnouncementByIdRequestDTO request, CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var entity = await _announcementRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                return new GetAnnouncementByIdResponseDTO { Announcement = null };
            }

            var utcNow = DateTime.UtcNow;
            var item = AdminAnnouncementMapping.ToAdminItem(entity, utcNow, includeContent: true);

            return new GetAnnouncementByIdResponseDTO { Announcement = item };
        }
    }
}
