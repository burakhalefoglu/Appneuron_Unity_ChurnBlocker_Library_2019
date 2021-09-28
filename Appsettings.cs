namespace AppneuronUnity
{
    internal static class Appsettings
    {
        public const string BootstrapServers = "192.168.43.178:9092";
        public const string Connection = "tcp://127.0.0.1:5557";

        public const string WebsocketDataCreationServer = "127.0.0.1";
        public const int WebsocketDataCreationPort = 3000;

        public const string WebsocketRemoteServer = "127.0.0.1";
        public const int WebsocketDataRemotePort = 3001;

        public const string AuthWebApiLink = "http://localhost:52039/api/Auth/";
        public const string ClientWebApiLink = "https://localhost:44370/WebAPI/api/";
        public const string ClientTokenRequestName = "clienttoken";
        public const string MlResultRequestName = "MlResultModels/getbyproductId";

        public const string TokenName = "JwtToken";

        public const int VisualData = 1;
        public const int ChurnPrediction = 2;
        public const int ChurnBlocker = 3;

    }
}
