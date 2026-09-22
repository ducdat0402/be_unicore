using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UniCore.API.Controllers;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.DTO;
using UniCore.Application.Feature.v1.Announcement.CancelAnnouncement;
using UniCore.Application.Feature.v1.Announcement.CreateAnnouncement;
using UniCore.Application.Feature.v1.Announcement.DeleteAnnouncement;
using UniCore.Application.Feature.v1.Announcement.GetAllAnnouncement;
using UniCore.Application.Feature.v1.Announcement.GetAnnouncementById;
using UniCore.Application.Feature.v1.Announcement.GetDeliveryReport;
using UniCore.Application.Feature.v1.Announcement.PreviewRecipients;
using UniCore.Application.Feature.v1.Announcement.PublishAnnouncement;
using UniCore.Application.Feature.v1.Announcement.UpdateAnnouncement;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace UniCore.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    [Route("api/v{version:apiVersion}/announcements")]
    public class AnnouncementController : BaseController
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementController(IAnnouncementService announcementService, IJsonStringLocalizer localizer) : base(localizer)
        {
            _announcementService = announcementService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseAPIResponse<GetAllAnnouncementResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseAPIResponse<GetAllAnnouncementResponseDTO>>> GetAll(
            [FromQuery] GetAllAnnouncementRequestDTO request,
            CancellationToken cancellationToken)
        {
            var result = await _announcementService.GetAllAsync(request, cancellationToken);
            var message = _localizer.GetString(MessageConstants.Announcement.GetAllSuccess);
            return OkResponse(result, message);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetAnnouncementByIdResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<GetAnnouncementByIdResponseDTO>>> GetById(string id, CancellationToken cancellationToken)
        {
            var result = await _announcementService.GetByIdAsync(id, cancellationToken);
            if (result.Announcement == null)
            {
                return NotFoundResponse<GetAnnouncementByIdResponseDTO>(
                    _localizer.GetString(MessageConstants.Announcement.NotFound));
            }

            return OkResponse(result, _localizer.GetString(MessageConstants.Announcement.GetByIdSuccess));
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseAPIResponse<CreateAnnouncementResponseDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<CreateAnnouncementResponseDTO>>> Create(
            [FromBody] CreateAnnouncementRequestDTO request,
            CancellationToken cancellationToken)
        {
            request.ActorUserId = GetActorUserId();
            var result = await _announcementService.CreateAsync(request, cancellationToken);
            return CreatedResponse(result, _localizer.GetString(MessageConstants.Announcement.CreateSuccess));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseAPIResponse<UpdateAnnouncementResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<UpdateAnnouncementResponseDTO>>> Update(
            string id,
            [FromBody] UpdateAnnouncementRequestDTO request,
            CancellationToken cancellationToken)
        {
            request.Id = id;
            request.ActorUserId = GetActorUserId();
            var result = await _announcementService.UpdateAsync(request, cancellationToken);
            return OkResponse(result, _localizer.GetString(MessageConstants.Announcement.UpdateSuccess));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseAPIResponse<DeleteAnnouncementResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<DeleteAnnouncementResponseDTO>>> Delete(string id, CancellationToken cancellationToken)
        {
            var result = await _announcementService.DeleteAsync(id, cancellationToken);
            return OkResponse(result, _localizer.GetString(MessageConstants.Announcement.DeleteSuccess));
        }

        [HttpPost("preview-recipients")]
        [ProducesResponseType(typeof(BaseAPIResponse<PreviewRecipientsResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<PreviewRecipientsResponseDTO>>> PreviewRecipients(
            [FromBody] PreviewRecipientsRequestDTO request,
            CancellationToken cancellationToken)
        {
            var result = await _announcementService.PreviewRecipientsAsync(request, cancellationToken);
            return OkResponse(result, _localizer.GetString(MessageConstants.Announcement.PreviewSuccess));
        }

        [HttpPost("{id}/publish")]
        [ProducesResponseType(typeof(BaseAPIResponse<PublishAnnouncementResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<PublishAnnouncementResponseDTO>>> Publish(
            string id,
            CancellationToken cancellationToken)
        {
            var result = await _announcementService.PublishAsync(
                new PublishAnnouncementRequestDTO { Id = id, ActorUserId = GetActorUserId() },
                cancellationToken);
            return OkResponse(result, _localizer.GetString(MessageConstants.Announcement.PublishSuccess));
        }

        [HttpPost("{id}/cancel")]
        [ProducesResponseType(typeof(BaseAPIResponse<CancelAnnouncementResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<CancelAnnouncementResponseDTO>>> Cancel(
            string id,
            CancellationToken cancellationToken)
        {
            var result = await _announcementService.CancelAsync(
                new CancelAnnouncementRequestDTO { Id = id, ActorUserId = GetActorUserId() },
                cancellationToken);
            return OkResponse(result, _localizer.GetString(MessageConstants.Announcement.CancelSuccess));
        }

        [HttpGet("{id}/delivery-report")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetDeliveryReportResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<GetDeliveryReportResponseDTO>>> GetDeliveryReport(
            string id,
            CancellationToken cancellationToken)
        {
            var result = await _announcementService.GetDeliveryReportAsync(id, cancellationToken);
            return OkResponse(result, _localizer.GetString(MessageConstants.Announcement.DeliveryReportSuccess));
        }

        private string? GetActorUserId()
        {
            return User.FindFirst(AuthConstants.Claims.UserId)?.Value
                   ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? User.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}
