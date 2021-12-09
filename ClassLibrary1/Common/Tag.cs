﻿using System.Diagnostics;

namespace ClassLibrary1.Common
{
    public static class Tag
    {
        public static int Count(this String str)
        {
            if (true)
            {
                return str.Split(new char[] { '#', '?', '@' }, StringSplitOptions.RemoveEmptyEntries).Length;
            }
        }

        public static string Comment(this String str)
        {
            if (true)
            {
                Trace.WriteLine(str.TagComment());
                return $"{str.TagComment()}";
            }
        }

        public static string Warning(this String str)
        {
            if (true)
            {
                Trace.TraceWarning(str.TagWarning());
                return $"{str.TagWarning()}";
            }
        }

        public static string Error(this String str)
        {
            if (true)
            {
                Trace.TraceError(str.TagError());
                return $"{str.TagError()}";
            }
        }

        public static string When(this String str)
        {
            if (true)
            {
                Trace.TraceInformation(str.TagWhen());
                return $"{str.TagWhen()}";
            }
        }

        public static string Why(this String str)
        {
            if (true)
            {
                Trace.TraceInformation(str.TagWhy());
                return $"{str.TagWhy()}";
            }
        }

        public static string Who(this String str)
        {
            if (true)
            {
                Trace.TraceInformation(str.TagWho());
                return $"{str.TagWho()}";
            }
        }

        public static string What(this String str)
        {
            if (true)
            {
                Trace.TraceInformation(str.TagWhat());
                return $"{str.TagWhat()}";
            }
        }

        public static string How(this String str)
        {
            if (true)
            {
                Trace.TraceInformation(str.TagHow());
                return $"{str.TagHow()}";
            }
        }

        public static string Where(this String str)
        {
            if (true)
            {
                Trace.TraceInformation(str.TagWhere());
                return $"{str.TagWhere()}";
            }
        }
    }
}