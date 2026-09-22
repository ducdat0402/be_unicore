using FluentValidation;

namespace UniCore.Application.Feature.v1.Announcement.CancelAnnouncement
{
    public class CancelAnnouncementValidator : AbstractValidator<CancelAnnouncementRequestDTO>
    {
        public CancelAnnouncementValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Announcement ID is required.");
        }
    }
}
