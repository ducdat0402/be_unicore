using System.Linq.Expressions;
using UniCore.Application.Contract.Repository.Enitity.v1;
using UniCore.Application.Contract.RequestHandlerHub;
using UniCore.Application.DTO.Entity;

namespace UniCore.Application.Feature.v1.Announcement.GetAllAnnouncement
{
    public class GetAllAnnouncementHandler : IRequestHandler<GetAllAnnouncementRequestDTO, GetAllAnnouncementResponseDTO>
    {
        private readonly IAnnouncementRepository _announcementRepository;

        public GetAllAnnouncementHandler(IAnnouncementRepository announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        public async Task<GetAllAnnouncementResponseDTO> HandleAsync(GetAllAnnouncementRequestDTO request, CancellationToken cancellationToken)
        {
            Expression<Func<UniCore.Application.Entity.Announcement, bool>>? filter = BuildFilter(request);

            var pagedResult = await _announcementRepository.GetPageNumberPaginationAsync<AnnouncementDTO>(
                request,
                filter,
                cancellationToken);

            return new GetAllAnnouncementResponseDTO
            {
                Items = pagedResult.Items,
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize,
                TotalRecords = pagedResult.TotalRecords,
                TotalPages = pagedResult.TotalPages,
                HasNextPage = pagedResult.HasNextPage,
                HasPreviousPage = pagedResult.HasPreviousPage
            };
        }

        private static Expression<Func<UniCore.Application.Entity.Announcement, bool>>? BuildFilter(GetAllAnnouncementRequestDTO request)
        {
            var hasSearch = !string.IsNullOrWhiteSpace(request.SearchTerm);
            var hasStatus = !string.IsNullOrWhiteSpace(request.Status);

            if (!hasSearch && !hasStatus)
            {
                return null;
            }

            if (hasSearch && hasStatus)
            {
                var term = request.SearchTerm!;
                var status = request.Status!;
                return a =>
                    ((a.Title != null && a.Title.Contains(term)) || (a.Code != null && a.Code.Contains(term)))
                    && a.Status == status;
            }

            if (hasSearch)
            {
                var term = request.SearchTerm!;
                return a => (a.Title != null && a.Title.Contains(term)) || (a.Code != null && a.Code.Contains(term));
            }

            var statusOnly = request.Status!;
            return a => a.Status == statusOnly;
        }
    }
}
