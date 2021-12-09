namespace ClassLibrary1.Common
{
    public static class StringExtension
    {
        public static int TagCount(this String str)
        {
            return str.Split(new char[] { '#', '?', '@' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        public static string TagError(this String str)
        {
            return $"!{str}";
        }

        public static string TagComment(this String str)
        {
            return $"//{str}";
        }

        public static string TagWarning(this String str)
        {
            return $"^{str}";
        }

        public static string TagWhen(this String str)
        {
            // https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings
            return $"[{DateTime.UtcNow.ToString("s")}]{str}";
        }

        public static string TagWho(this String str)
        {
            return $"@{str}";
        }

        public static string TagHow(this String str)
        {
            return $"#{str}";
        }

        public static string TagWhy(this String str)
        {
            return $"?{str}";
        }

        public static string TagWhat(this String str)
        {
            return $".{str}";
        }

        public static string TagWhere(this String str)
        {
            return $"(){str}";
        }
    }
}
