using UniCore.Helper.Constant;

namespace UniCore.Application.Feature.v1.Announcement
{
    internal static class AnnouncementScopeHelper
    {
        public static string? NormalizeScopeValue(string scopeType, string? scopeValue)
        {
            var normalized = scopeType.Trim().ToUpperInvariant();

            if (normalized is AnnouncementConstants.Scope.Public or AnnouncementConstants.Scope.Student)
            {
                return null;
            }

            return string.IsNullOrWhiteSpace(scopeValue) ? null : scopeValue.Trim();
        }

        public static List<string> NormalizeStudentIds(IEnumerable<string>? studentIds)
        {
            if (studentIds == null)
            {
                return new List<string>();
            }

            return studentIds
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Select(id => id.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }
}
