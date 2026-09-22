using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Feature.v1.Announcement.GetAnnouncementById
{
    public class GetAnnouncementByIdHandler : IRequestHandler<GetAnnouncementByIdRequestDTO, GetAnnouncementByIdResponseDTO>
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetAnnouncementByIdRequestDTO> _validator;

        public GetAnnouncementByIdHandler(
            IAnnouncementRepository announcementRepository,
            IMapper mapper,
            IValidator<GetAnnouncementByIdRequestDTO> validator)
        {
            _announcementRepository = announcementRepository;
            _mapper = mapper;
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

            return new GetAnnouncementByIdResponseDTO
            {
                Announcement = entity == null ? null : _mapper.Map<AnnouncementDTO>(entity)
            };
        }
    }
}
