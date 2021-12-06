namespace ncrunch
{
    public static class NCrunchAppRegistration
    {
        public const string ClientID = "3b6a2d06-654c-471e-aec4-8fa6b73cc85e"; // Keep Secret
        public const string ClientSecret = "vCn7Q~lwyjAe-C.OjhK1dTQkuqbPPognRMJtZ"; // Keep Secret
        public const string Authority = "https://login.microsoftonline.com/d3efb988-727d-47ea-adb8-cce6dc17857d"; // Keep Secret
        public static string? AccessToken { get; internal set; }
    }
}