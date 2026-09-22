using FluentValidation;

namespace UniCore.Application.Feature.v1.Announcement.PublishAnnouncement
{
    public class PublishAnnouncementValidator : AbstractValidator<PublishAnnouncementRequestDTO>
    {
        public PublishAnnouncementValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Announcement ID is required.");
        }
    }
}
