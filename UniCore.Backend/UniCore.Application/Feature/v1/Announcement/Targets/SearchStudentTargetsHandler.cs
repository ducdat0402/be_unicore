using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Announcement.Targets
{
    public class SearchStudentTargetsRequest : IRequest<AnnouncementTargetSearchResponseDto<StudentTargetItemDto>>
    {
        public string? Search { get; set; }
        public int? Limit { get; set; }
    }

    public class SearchStudentTargetsHandler
        : IRequestHandler<SearchStudentTargetsRequest, AnnouncementTargetSearchResponseDto<StudentTargetItemDto>>
    {
        private readonly IUserRepository _userRepository;

        public SearchStudentTargetsHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<AnnouncementTargetSearchResponseDto<StudentTargetItemDto>> HandleAsync(
            SearchStudentTargetsRequest request,
            CancellationToken cancellationToken)
        {
            var limit = AnnouncementTargetLimit.Normalize(request.Limit);
            var search = AnnouncementTargetSearchHelpers.NormalizeSearch(request.Search);
            var (items, total) = await _userRepository.SearchActiveVerifiedStudentsAsync(search, limit, cancellationToken);

            return new AnnouncementTargetSearchResponseDto<StudentTargetItemDto>
            {
                Data = items.Select(u => new StudentTargetItemDto
                {
                    StudentId = u.Id,
                    StudentName = AnnouncementTargetSearchHelpers.ResolveStudentName(u),
                    StudentCode = u.Code ?? u.Username
                }).ToList(),
                Meta = AnnouncementTargetSearchHelpers.BuildMeta(
                    total, limit, search, AnnouncementTargetConstants.Domain.Students)
            };
        }
    }
}
