using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Feature.v1.Announcement.GetPublicAnnouncementById
{
    public class GetPublicAnnouncementByIdHandler
        : IRequestHandler<GetPublicAnnouncementByIdRequestDTO, GetPublicAnnouncementByIdResponseDTO>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetPublicAnnouncementByIdRequestDTO> _validator;

        public GetPublicAnnouncementByIdHandler(
            IAnnouncementRepository announcementRepository,
            IMapper mapper,
            IValidator<GetPublicAnnouncementByIdRequestDTO> validator)
        {
            _announcementRepository = announcementRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<GetPublicAnnouncementByIdResponseDTO> HandleAsync(
            GetPublicAnnouncementByIdRequestDTO request,
            CancellationToken cancellationToken)
        {
            ValidationResult results = await _validator.ValidateAsync(request, cancellationToken);
            if (!results.IsValid)
            {
                throw new ValidationException(results.Errors);
            }

            var entity = await _announcementRepository.GetPublishedPublicByIdAsync(request.Id, cancellationToken);

            return new GetPublicAnnouncementByIdResponseDTO
            {
                Announcement = entity == null ? null : _mapper.Map<AnnouncementDTO>(entity)
            };
        }
    }

    public class GetPublicAnnouncementByIdValidator : AbstractValidator<GetPublicAnnouncementByIdRequestDTO>
    {
        public GetPublicAnnouncementByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Announcement ID is required.");
        }
    }
}
