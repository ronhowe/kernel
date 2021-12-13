namespace ClassLibrary1
{
    public static class PhotonFactory
    {
        public static Photon Create(Color color)
        {
            return new() { Id = Guid.NewGuid(), Color = color };
        }

        public static Photon Empty()
        {
            return new() { Id = Guid.Empty, Color = Color.Black};
        }
    }
}
