using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniCore.API.Controllers;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.DTO;
using UniCore.Application.Feature.v1.Announcement.GetPublicAnnouncementById;
using UniCore.Application.Feature.v1.Announcement.GetPublicAnnouncements;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.API.Controllers.v1
{
    /// <summary>
    /// Guest-readable PUBLIC announcements (no authentication).
    /// </summary>
    [ApiVersion("1.0")]
    [AllowAnonymous]
    [Route("api/v{version:apiVersion}/public/announcements")]
    public class PublicAnnouncementController : BaseController
    {
        private readonly IAnnouncementService _announcementService;

        public PublicAnnouncementController(IAnnouncementService announcementService, IJsonStringLocalizer localizer)
            : base(localizer)
        {
            _announcementService = announcementService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseAPIResponse<GetPublicAnnouncementsResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseAPIResponse<GetPublicAnnouncementsResponseDTO>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _announcementService.GetPublicListAsync(cancellationToken);
            return OkResponse(result, _localizer.GetString(MessageConstants.Announcement.GetPublicListSuccess));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetPublicAnnouncementByIdResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<GetPublicAnnouncementByIdResponseDTO>>> GetById(
            string id,
            CancellationToken cancellationToken)
        {
            var result = await _announcementService.GetPublicByIdAsync(id, cancellationToken);
            if (result.Announcement == null)
            {
                return NotFoundResponse<GetPublicAnnouncementByIdResponseDTO>(
                    _localizer.GetString(MessageConstants.Announcement.NotFound));
            }

            return OkResponse(result, _localizer.GetString(MessageConstants.Announcement.GetPublicByIdSuccess));
        }
    }
}
