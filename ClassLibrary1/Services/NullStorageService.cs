namespace ClassLibrary1
{
    public static class NullStorageService<T> where T : Photon //new() //ISnack<T>
    {
        //private static T t = (T)PhotonFactory.Empty();
        private static T X;

        //public static async Task<T> IO(T t) => await Task.Run(() => t); // temp implementation
        public static async Task<T> IO(T t)
        {

            Tag.Where("IO");

            Tag.Why("IOStart");

            Tag.Why("PreInputCall");

            Tag.What($"{typeof(T)}={t}");

            await Write(t);

            Tag.Why("PostInputCall");

            t.Sent = true;

            Tag.Why("PreOuput");

            var receivedPhoton = await Read(t.Id);

            Tag.What($"receivedPhoton={receivedPhoton}");

            Tag.Why("PostOuput");

            t.Received = true;

            t.Color = receivedPhoton.Color;

            Tag.Why("IOComplete");

            return t;
        }

        public static async Task Write(T t)
        {
            Tag.Where("Input");

            Tag.Why("InputStart");

            await Task.Run(() => X = t);

            Tag.Why("InputComplete");
        }

        public static async Task<T> Read(Guid id)
        {
            Tag.Where("Output");

            Tag.Why("OutputStart");

            Tag.What($"id={id}");

            var t = await Task.Run(() => X);

            Tag.Why("OutputComplete");

            return t;
        }
    }

    public static class NullStorageService
    {
        private static Photon Photon = PhotonFactory.Empty();

        public static async Task<Photon> IO(Photon photon)
        {
            Tag.Where("IO");

            Tag.Why("IOStart");

            Tag.Why("PreInputCall");

            Tag.What($"photon={photon}");

            await Write(photon);

            Tag.Why("PostInputCall");

            photon.Sent = true;

            Tag.Why("PreOuput");

            var receivedPhoton = await Read(photon.Id);

            Tag.What($"receivedPhoton={receivedPhoton}");

            Tag.Why("PostOuput");

            photon.Received = true;

            photon.Color = receivedPhoton.Color;

            Tag.Why("IOComplete");

            return photon;
        }

        public static async Task Write(Photon photon)
        {
            Tag.Where("Input");

            Tag.Why("InputStart");

            await Task.Run(() => Photon = photon);

            Tag.Why("InputComplete");
        }

        public static async Task<Photon> Read(Guid id)
        {
            Tag.Where("Output");

            Tag.Why("OutputStart");

            Tag.What($"id={id}");

            var photon = await Task.Run(() => Photon);

            Tag.Why("OutputComplete");

            return photon;
        }
    }
}