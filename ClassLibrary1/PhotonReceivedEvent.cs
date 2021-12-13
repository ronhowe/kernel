namespace ClassLibrary1
{
    public class PhotonReceivedEvent : DomainEvent
    {
        public PhotonReceivedEvent(Photon packet)
        {
            Packet = packet;
        }

        public Photon Packet { get; }
    }
}
