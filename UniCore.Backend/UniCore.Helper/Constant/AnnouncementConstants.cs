namespace UniCore.Helper.Constant
{
    public static class AnnouncementConstants
    {
        public static class Status
        {
            public const string Draft = "DRAFT";
            public const string Published = "PUBLISHED";
            public const string Cancelled = "CANCELLED";
        }

        public static class Type
        {
            public const string Normal = "NORMAL";
            public const string Important = "IMPORTANT";
            public const string Urgent = "URGENT";

            public static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase)
            {
                Normal, Important, Urgent
            };
        }

        public static class Scope
        {
            public const string Public = "PUBLIC";
            public const string Department = "DEPARTMENT";
            public const string Class = "CLASS";
            public const string Student = "STUDENT";

            public static readonly HashSet<string> Supported = new(StringComparer.OrdinalIgnoreCase)
            {
                Public, Department, Class, Student
            };
        }
    }
}
