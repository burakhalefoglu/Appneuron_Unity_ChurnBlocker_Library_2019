namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Workers.DifficultyResult.Clients
{
    using AppneuronUnity.Core.Libraries.LitJson;
    using AppneuronUnity.Core.Libraries.WebSocket;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.Helper;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Workers.DifficultyResult.Models;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="DifficultyClient" />.
    /// </summary>
    internal class DifficultyClient
    {
        /// <summary>
        /// The ListenServerManager.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        internal static async Task ListenServerManager()
        {
            await Task.Run(() =>
            {
                //var clientId = new ClientIdUnityManager().GetPlayerID();

                var connectedServer = ConnectServerManager("127.0.0.1", 3000, "" /*clientId*/);

                connectedServer.OnMessage += (sender, e) =>
                {
                    Console.WriteLine("manager says: " + e.Data);
                    var data = JsonMapper.ToObject<ServerModelDto>(e.Data);
                    connectedServer.Close();

                    //TODO: Burada Ml result dışında bağlanacak bir yapı var ise burada eklenmeli!!!
                    var webSocket = ConnectMlResultServer(data.Port, data.Host, "" /*clientId*/);
                };
            });
        }

        /// <summary>
        /// The ConnectServerManager.
        /// </summary>
        /// <param name="host">The host<see cref="string"/>.</param>
        /// <param name="port">The port<see cref="int"/>.</param>
        /// <param name="clientId">The clientId<see cref="string"/>.</param>
        /// <returns>The <see cref="WebSocket"/>.</returns>
        private static WebSocket ConnectServerManager(string host, int port, string clientId)
        {
            var ws = new WebSocket($"ws://{host}:{port}/MlResultClientBehavior?clientid=" + clientId);

            try
            {
                ws.Connect();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Hata: ", exception.Message);
                Thread.Sleep(60000);
                ConnectServerManager(host, port, clientId);
            }
            return ws;
        }

        /// <summary>
        /// The ConnectMlResultServer.
        /// </summary>
        /// <param name="port">The port<see cref="int"/>.</param>
        /// <param name="host">The host<see cref="string"/>.</param>
        /// <param name="clientId">The clientId<see cref="string"/>.</param>
        /// <returns>The <see cref="WebSocket"/>.</returns>
        private static WebSocket ConnectMlResultServer(int port, string host, string clientId)
        {
            var ws = new WebSocket($"ws://{host}:{port}/MlResult?clientid=" + clientId);

            try
            {
                ws.OnMessage += (sender, e) =>
                {
                    DifficultyModelDto mlResultModel = 
                    JsonMapper.ToObject<DifficultyModelDto>(e.Data);
                    DifficultyHelper.AskDifficultyLevelFromServer(new DifficultyModel
                    {
                        CenterOfDifficultyLevel = (int)mlResultModel.ResultValue,
                        RangeCount = 2
                    });

                    Console.WriteLine("MyMlResultValue: " + mlResultModel.ResultValue);
                };

                ws.OnOpen += Ws_OnOpen;
                ws.OnError += Ws_OnError;
                ws.OnClose += Ws_OnClose;

                ws.Connect();
                ws.Send("I Am Client...");
                Console.ReadKey(true);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Hata: ", exception.Message);
                Thread.Sleep(5000);
                ConnectServerManager("127.0.0.1", 3000, clientId);
            }
            return ws;
        }

        /// <summary>
        /// The Ws_OnClose.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="CloseEventArgs"/>.</param>
        private async static void Ws_OnClose(object sender, CloseEventArgs e)
        {
            //Server çökünce veya bağlantı kapanınca burası tetikleniyor...
            Console.WriteLine("Connection Closed. Reconnecting...", e.Reason);
            await ListenServerManager();
        }

        /// <summary>
        /// The Ws_OnError.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="ErrorEventArgs"/>.</param>
        private static void Ws_OnError(object sender, ErrorEventArgs e)
        {
            //Bir hata oluşunca burası tetikleniyor....
            Console.WriteLine("Error: ", e.Message);
        }

        /// <summary>
        /// The Ws_OnOpen.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private static void Ws_OnOpen(object sender, EventArgs e)
        {
            //Bağlantı açılınca burası tetikleniyor.
            Console.WriteLine("Opened: ", e);
        }
    }
}
