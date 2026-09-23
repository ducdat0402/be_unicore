using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniCore.Application.Feature.v1.Announcement.GetStudentAnnouncements;
using UniCore.Application.Feature.v1.Announcement.MarkAnnouncementAcknowledged;
using UniCore.Application.Feature.v1.Announcement.MarkAnnouncementViewed;

namespace UniCore.API.Controllers.v1
{
    /// <summary>
    /// Student announcement endpoints for viewing and interacting with announcements.
    /// </summary>
    [ApiVersion("1.0")]
    [Authorize(Roles = "Student")]
    [Route("api/v{version:apiVersion}/student/announcements")]
    public class StudentAnnouncementController : BaseController
    {
        private readonly GetStudentAnnouncementsHandler _getStudentAnnouncementsHandler;
        private readonly MarkAnnouncementViewedHandler _markViewedHandler;
        private readonly MarkAnnouncementAcknowledgedHandler _markAcknowledgedHandler;

        public StudentAnnouncementController(
            GetStudentAnnouncementsHandler getStudentAnnouncementsHandler,
            MarkAnnouncementViewedHandler markViewedHandler,
            MarkAnnouncementAcknowledgedHandler markAcknowledgedHandler)
        {
            _getStudentAnnouncementsHandler = getStudentAnnouncementsHandler;
            _markViewedHandler = markViewedHandler;
            _markAcknowledgedHandler = markAcknowledgedHandler;
        }

        /// <summary>
        /// Get announcements for current student with read status.
        /// </summary>
        /// <param name="isRead">Filter by read status (optional).</param>
        /// <param name="type">Filter by type: URGENT, IMPORTANT, NORMAL (optional).</param>
        /// <param name="page">Page number (default 1).</param>
        /// <param name="pageSize">Page size (default 20, max 100).</param>
        /// <returns>Paginated list of announcements with read status.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(GetStudentAnnouncementsResponseDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyAnnouncements(
            [FromQuery] bool? isRead = null,
            [FromQuery] string? type = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            var studentId = GetCurrentUserId();

            var request = new GetStudentAnnouncementsRequestDTO
            {
                StudentId = studentId,
                IsRead = isRead,
                Type = type,
                Page = page,
                PageSize = Math.Min(pageSize, 100)
            };

            var result = await _getStudentAnnouncementsHandler.HandleAsync(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Mark an announcement as viewed.
        /// </summary>
        /// <param name="announcementId">Announcement ID.</param>
        /// <returns>Viewed timestamp.</returns>
        [HttpPost("{announcementId}/view")]
        [ProducesResponseType(typeof(MarkAnnouncementViewedResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkAsViewed(
            string announcementId,
            CancellationToken cancellationToken = default)
        {
            var studentId = GetCurrentUserId();

            var request = new MarkAnnouncementViewedRequestDTO
            {
                AnnouncementId = announcementId,
                StudentId = studentId
            };

            try
            {
                var result = await _markViewedHandler.HandleAsync(request, cancellationToken);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Mark an announcement as acknowledged.
        /// </summary>
        /// <param name="announcementId">Announcement ID.</param>
        /// <returns>Acknowledged timestamp.</returns>
        [HttpPost("{announcementId}/acknowledge")]
        [ProducesResponseType(typeof(MarkAnnouncementAcknowledgedResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkAsAcknowledged(
            string announcementId,
            CancellationToken cancellationToken = default)
        {
            var studentId = GetCurrentUserId();

            var request = new MarkAnnouncementAcknowledgedRequestDTO
            {
                AnnouncementId = announcementId,
                StudentId = studentId
            };

            try
            {
                var result = await _markAcknowledgedHandler.HandleAsync(request, cancellationToken);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        private string GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                ?? User.FindFirst("sub")
                ?? User.FindFirst("userId");

            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User ID not found in token");
            }

            return userIdClaim.Value;
        }
    }
}
