using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Announcement.Targets
{
    public class SearchDepartmentTargetsRequest : IRequest<AnnouncementTargetSearchResponseDto<DepartmentTargetItemDto>>
    {
        public string? Search { get; set; }
        public int? Limit { get; set; }
    }

    public class SearchDepartmentTargetsHandler
        : IRequestHandler<SearchDepartmentTargetsRequest, AnnouncementTargetSearchResponseDto<DepartmentTargetItemDto>>
    {
        private readonly IDepartmentRepository _departmentRepository;

        public SearchDepartmentTargetsHandler(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<AnnouncementTargetSearchResponseDto<DepartmentTargetItemDto>> HandleAsync(
            SearchDepartmentTargetsRequest request,
            CancellationToken cancellationToken)
        {
            var limit = AnnouncementTargetLimit.Normalize(request.Limit);
            var search = AnnouncementTargetSearchHelpers.NormalizeSearch(request.Search);
            var (items, total) = await _departmentRepository.SearchActiveAsync(search, limit, cancellationToken);

            return new AnnouncementTargetSearchResponseDto<DepartmentTargetItemDto>
            {
                Data = items.Select(d => new DepartmentTargetItemDto
                {
                    DepartmentId = d.Id,
                    DepartmentName = d.Name,
                    DepartmentCode = d.Code
                }).ToList(),
                Meta = AnnouncementTargetSearchHelpers.BuildMeta(
                    total, limit, search, AnnouncementTargetConstants.Domain.Department)
            };
        }
    }
}
