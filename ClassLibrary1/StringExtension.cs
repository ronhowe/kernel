namespace ClassLibrary1
{
    public static class StringExtension
    {
        private const string TagSeparator = " : ";

        private const string CommentPrefix = "//";
        private const string SecretPrefix = "##";
        private const string ToDoPrefix = "&&";

        private const string WarningPrefix = "**";
        private const string ErrorPrefix = "!!";

        private const string WhoPrefix = "@@";
        private const string WhatPrefix = "[]";
        private const string WherePrefix = "()";
        private const string WhenPrefix = "||";
        private const string WhyPrefix = "??";
        private const string HowPrefix = "{}";

        private static string TagPrefix()
        {
            // https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings
            return $"{DateTime.UtcNow:s}";
        }

        public static string TagComment(this String str)
        {
            return $"{TagPrefix()}{TagSeparator}{CommentPrefix}{str}";
        }

        public static string TagSecret(this String str)
        {
            if (str.Length > 0) {
                return $"{TagPrefix()}{TagSeparator}{SecretPrefix}REDACTED-SECRET";
            }
            else
            {
                return $"{TagPrefix()}{TagSeparator}{SecretPrefix}EMPTY-SECRET";
            }
        }

        public static string TagToDo(this String str)
        {
            return $"{TagPrefix()}{TagSeparator}{ToDoPrefix}{str}";
        }

        public static string TagWarning(this String str)
        {
            return $"{TagPrefix()}{TagSeparator}{WarningPrefix}{str}";
        }

        public static string TagError(this String str)
        {
            return $"{TagPrefix()}{TagSeparator}{ErrorPrefix}{str}";
        }

        public static string TagWho(this String str)
        {
            return $"{TagPrefix()}{TagSeparator}{WhoPrefix}{str}";
        }

        public static string TagWhat(this String str)
        {
            return $"{TagPrefix()}{TagSeparator}{WhatPrefix}{str}";
        }

        public static string TagWhere(this String str)
        {
            return $"{TagPrefix()}{TagSeparator}{WherePrefix}{str}";
        }

        public static string TagWhen(this String str)
        {
            return $"{TagPrefix()}{TagSeparator}{WhenPrefix}{str}";
        }

        public static string TagWhy(this String str)
        {
            return $"{TagPrefix()}{TagSeparator}{WhyPrefix}{str}";
        }

        public static string TagHow(this String str)
        {
            return $"{TagPrefix()}{TagSeparator}{HowPrefix}{str}";
        }
    }
}
