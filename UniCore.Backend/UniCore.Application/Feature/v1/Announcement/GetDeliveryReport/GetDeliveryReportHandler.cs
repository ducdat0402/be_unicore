using FluentValidation;
using FluentValidation.Results;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Announcement.GetDeliveryReport
{
    public class GetDeliveryReportHandler : IRequestHandler<GetDeliveryReportRequestDTO, GetDeliveryReportResponseDTO>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IAnnouncementStudentRepository _announcementStudentRepository;
        private readonly IValidator<GetDeliveryReportRequestDTO> _validator;

        public GetDeliveryReportHandler(
            IAnnouncementRepository announcementRepository,
            IAnnouncementStudentRepository announcementStudentRepository,
            IValidator<GetDeliveryReportRequestDTO> validator)
        {
            _announcementRepository = announcementRepository;
            _announcementStudentRepository = announcementStudentRepository;
            _validator = validator;
        }

        public async Task<GetDeliveryReportResponseDTO> HandleAsync(GetDeliveryReportRequestDTO request, CancellationToken cancellationToken)
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

            var (total, viewed, acknowledged) = await _announcementStudentRepository.GetDeliveryStatsAsync(
                entity.Id,
                cancellationToken);

            var isPublishedOrCancelled =
                string.Equals(entity.Status, AnnouncementConstants.Status.Published, StringComparison.OrdinalIgnoreCase)
                || string.Equals(entity.Status, AnnouncementConstants.Status.Cancelled, StringComparison.OrdinalIgnoreCase);

            if (!isPublishedOrCancelled && total == 0)
            {
                throw new InvalidOperationException(
                    "Delivery report is only available for PUBLISHED/CANCELLED announcements or announcements that already have recipients.");
            }

            var recipients = await _announcementStudentRepository.GetByAnnouncementIdAsync(entity.Id, cancellationToken);

            return new GetDeliveryReportResponseDTO
            {
                AnnouncementId = entity.Id,
                Title = entity.Title,
                Status = entity.Status,
                RecipientCount = total,
                ViewedCount = viewed,
                AcknowledgedCount = acknowledged,
                Recipients = recipients.Select(r => new DeliveryRecipientDTO
                {
                    StudentId = r.StudentId,
                    Username = r.Student?.Username,
                    Email = r.Student?.Email,
                    ViewedAt = r.ViewedAt,
                    AcknowledgedAt = r.AcknowledgedAt
                }).ToList()
            };
        }
    }
}
