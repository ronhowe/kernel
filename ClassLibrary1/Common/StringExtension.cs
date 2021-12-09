namespace ClassLibrary1.Common
{
    public static class StringExtension
    {
        private static string TagPrefix()
        {
            // https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings
            return $"{DateTime.UtcNow:s}";
        }

        private static string TagSeparator()
        {
            return $" : ";
        }

        public static string TagComment(this String str)
        {
            return $"{TagPrefix()}{TagSeparator()}//{str}";
        }

        public static string TagWarning(this String str)
        {
            return $"{TagPrefix()}{TagSeparator()}**{str}";
        }

        public static string TagError(this String str)
        {
            return $"{TagPrefix()}{TagSeparator()}!!{str}";
        }

        public static string TagSecret(this String str)
        {
            return $"{TagPrefix()}{TagSeparator()}##SECRET";
        }

        public static string TagWho(this String str)
        {
            return $"{TagPrefix()}{TagSeparator()}@@{str}";
        }

        public static string TagWhat(this String str)
        {
            return $"{TagPrefix()}{TagSeparator()}[]{str}";
        }

        public static string TagWhere(this String str)
        {
            return $"{TagPrefix()}{TagSeparator()}(){str}";
        }

        public static string TagWhen(this String str)
        {
            return $"{TagPrefix()}{TagSeparator()}||{str}";
        }

        public static string TagWhy(this String str)
        {
            return $"{TagPrefix()}{TagSeparator()}??{str}";
        }

        public static string TagHow(this String str)
        {
            return $"{TagPrefix()}{TagSeparator()}{{}}{str}";
        }
    }
}
