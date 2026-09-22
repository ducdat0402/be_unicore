namespace UniCore.Application.Contract.Service.v1
{
    public interface IAnnouncementAudienceService
    {
        Task ValidateScopeAsync(
            string scopeType,
            string? scopeValue,
            IEnumerable<string>? targetStudentIds = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Resolves account-based recipients. Returns empty for PUBLIC (no snapshot).
        /// For STUDENT, returns validated targetStudentIds (does not read DB targets).
        /// </summary>
        Task<IReadOnlyList<string>> ResolveStudentIdsAsync(
            string scopeType,
            string? scopeValue,
            IEnumerable<string>? targetStudentIds = null,
            CancellationToken cancellationToken = default);
    }
}
