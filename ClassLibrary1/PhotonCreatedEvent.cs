namespace ClassLibrary1
{
    public class PhotonCreatedEvent : DomainEvent
    {
        public PhotonCreatedEvent(Photon packet)
        {
            Packet = packet;
        }

        public Photon Packet { get; }
    }
}
