namespace ClassLibrary1
{
    public class PhotonSentEvent : DomainEvent
    {
        public PhotonSentEvent(Photon photon)
        {
            Photon = photon;
        }

        public Photon Photon { get; }
    }
}
