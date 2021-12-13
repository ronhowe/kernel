namespace ClassLibrary1
{
    public static class Constant
    {
        public const string Id = "00000000-0000-0000-0000-000000000000";
        public const string Undefined = "Undefined";
#if DEBUG // Case Sensitive
        public const string BuildConfiguration = "Debug";
#else
        public const string BuildConfiguration = "Release";
#endif
    }
}
