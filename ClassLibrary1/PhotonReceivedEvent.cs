namespace ClassLibrary1
{
    public class PhotonReceivedEvent : DomainEvent
    {
        public PhotonReceivedEvent(Photon photon)
        {
            Photon = photon;
        }

        public Photon Photon { get; }
    }
}
