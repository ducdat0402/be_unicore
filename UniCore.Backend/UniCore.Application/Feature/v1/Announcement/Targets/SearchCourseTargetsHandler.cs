using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Announcement.Targets
{
    public class SearchCourseTargetsRequest : IRequest<AnnouncementTargetSearchResponseDto<CourseTargetItemDto>>
    {
        public string? Search { get; set; }
        public int? Limit { get; set; }
    }

    public class SearchCourseTargetsHandler
        : IRequestHandler<SearchCourseTargetsRequest, AnnouncementTargetSearchResponseDto<CourseTargetItemDto>>
    {
        private readonly ICourseRepository _courseRepository;

        public SearchCourseTargetsHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<AnnouncementTargetSearchResponseDto<CourseTargetItemDto>> HandleAsync(
            SearchCourseTargetsRequest request,
            CancellationToken cancellationToken)
        {
            var limit = AnnouncementTargetLimit.Normalize(request.Limit);
            var search = AnnouncementTargetSearchHelpers.NormalizeSearch(request.Search);
            var (items, total) = await _courseRepository.SearchActiveAsync(search, limit, cancellationToken);

            return new AnnouncementTargetSearchResponseDto<CourseTargetItemDto>
            {
                Data = items.Select(c => new CourseTargetItemDto
                {
                    CourseId = c.Id,
                    CourseName = c.Name,
                    CourseCode = c.Code
                }).ToList(),
                Meta = AnnouncementTargetSearchHelpers.BuildMeta(
                    total, limit, search, AnnouncementTargetConstants.Domain.Course)
            };
        }
    }
}
