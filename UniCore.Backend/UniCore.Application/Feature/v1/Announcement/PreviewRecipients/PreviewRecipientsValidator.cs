using FluentValidation;
using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Announcement.PreviewRecipients
{
    public class PreviewRecipientsValidator : AbstractValidator<PreviewRecipientsRequestDTO>
    {
        public PreviewRecipientsValidator()
        {
            RuleFor(x => x.ScopeType)
                .NotEmpty()
                .WithMessage("ScopeType is required.")
                .Must(s => AnnouncementConstants.Scope.Supported.Contains(s))
                .WithMessage("ScopeType must be PUBLIC, DEPARTMENT, CLASS, or STUDENT.");
        }
    }
}
