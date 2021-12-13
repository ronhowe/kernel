namespace ClassLibrary1
{
    public class PhotonSentEvent : DomainEvent
    {
        public PhotonSentEvent(Photon packet)
        {
            Packet = packet;
        }

        public Photon Packet { get; }
    }
}
