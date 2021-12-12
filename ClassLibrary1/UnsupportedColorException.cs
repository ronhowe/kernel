namespace ClassLibrary1
{
    public class UnsupportedColorException : Exception
    {
        public UnsupportedColorException(string code)
            : base($"UnsupportedColorException={code}")
        {
        }
    }
}
