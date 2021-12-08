namespace ClassLibrary1.Domain.Exceptions
{
    public class UnsupportedColorException : Exception
    {
        public UnsupportedColorException(string code)
            : base($"@UnsupportedColorException={code}")
        {
        }
    }
}
