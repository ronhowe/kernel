namespace ClassLibrary1
{
    public static class Constant
    {
        public const string Id = "00000000-0000-0000-0000-000000000000";
        public const string Undefined = "Undefined";
#if DEBUG
        public const string BuildConfiguration = "Debug";
#else
        public const string BuildConfiguration = "Release";
#endif
        public const string LocalApiEndpoint = "https://localhost:9999";
        public const string RemoteApiEndpoint = "https://api.ronhowe.org";
    }
}
