namespace ClassLibrary1
{
    public class PhotonCreatedEvent : DomainEvent
    {
        public PhotonCreatedEvent(Photon photon)
        {
            Photon = photon;
        }

        public Photon Photon { get; }
    }
}
