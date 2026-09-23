namespace UniCore.Application.Feature.v1.Announcement.Targets
{
    internal static class AnnouncementTargetSearchHelpers
    {
        public static string? NormalizeSearch(string? search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return null;
            }

            return search.Trim();
        }

        public static AnnouncementTargetSearchMetaDto BuildMeta(int total, int limit, string? search, string domain)
        {
            return new AnnouncementTargetSearchMetaDto
            {
                Total = total,
                Limit = limit,
                Search = search,
                Domain = domain
            };
        }

        public static string ResolveStudentName(Entity.User user)
        {
            if (!string.IsNullOrWhiteSpace(user.UserProfile?.FullName))
            {
                return user.UserProfile.FullName;
            }

            var first = user.UserProfile?.FirstName;
            var last = user.UserProfile?.LastName;
            if (!string.IsNullOrWhiteSpace(first) || !string.IsNullOrWhiteSpace(last))
            {
                return $"{first} {last}".Trim();
            }

            return user.Username;
        }
    }
}
