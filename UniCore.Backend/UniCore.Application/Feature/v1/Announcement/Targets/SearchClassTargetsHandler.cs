using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Announcement.Targets
{
    public class SearchClassTargetsRequest : IRequest<AnnouncementTargetSearchResponseDto<ClassTargetItemDto>>
    {
        public string? Search { get; set; }
        public int? Limit { get; set; }
    }

    public class SearchClassTargetsHandler
        : IRequestHandler<SearchClassTargetsRequest, AnnouncementTargetSearchResponseDto<ClassTargetItemDto>>
    {
        private readonly ISchoolClassRepository _schoolClassRepository;

        public SearchClassTargetsHandler(ISchoolClassRepository schoolClassRepository)
        {
            _schoolClassRepository = schoolClassRepository;
        }

        public async Task<AnnouncementTargetSearchResponseDto<ClassTargetItemDto>> HandleAsync(
            SearchClassTargetsRequest request,
            CancellationToken cancellationToken)
        {
            var limit = AnnouncementTargetLimit.Normalize(request.Limit);
            var search = AnnouncementTargetSearchHelpers.NormalizeSearch(request.Search);
            var (items, total) = await _schoolClassRepository.SearchActiveAsync(search, limit, cancellationToken);

            return new AnnouncementTargetSearchResponseDto<ClassTargetItemDto>
            {
                Data = items.Select(c => new ClassTargetItemDto
                {
                    ClassId = c.Id,
                    ClassName = c.Name,
                    ClassCode = c.Code
                }).ToList(),
                Meta = AnnouncementTargetSearchHelpers.BuildMeta(
                    total, limit, search, AnnouncementTargetConstants.Domain.Class)
            };
        }
    }
}
