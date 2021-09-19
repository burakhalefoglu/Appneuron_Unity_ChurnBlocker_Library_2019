using Appneuron.Core.CoreServices.WebSocketService.Models;
using Appneuron.DifficultyManagerComponent;
using Appneuron.Models;
using AppneuronUnity.ChurnBlockerModule.Components.DifficultyComponent.FlowbaseDifficulty;
using AppneuronUnity.Core.UnityManager;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Appneuron.Core.CoreServices.WebSocketService
{
    public class MlResultClient
    {
        public static async Task ListenServerManager()
        {
            await Task.Run(() =>
            {
                var clientId = new IdUnityManager().GetPlayerID();

                var connectedServer = ConnectServerManager("127.0.0.1", 3000, clientId);

                connectedServer.OnMessage += (sender, e) =>
                {
                    Console.WriteLine("manager says: " + e.Data);
                    var data = JsonConvert.DeserializeObject<ServerModelDto>(e.Data,
                              new JsonSerializerSettings
                              {
                                  PreserveReferencesHandling = PreserveReferencesHandling.Objects
                              });
                    connectedServer.Close();

                    //TODO: Burada Ml result dışında bağlanacak bir yapı var ise burada eklenmeli!!!
                    var webSocket = ConnectMlResultServer(data.Port, data.Host, clientId);
                };
            });
        }

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

        private static WebSocket ConnectMlResultServer(int port, string host, string clientId)
        {
            var ws = new WebSocket($"ws://{host}:{port}/MlResult?clientid=" + clientId);

            try
            {
                ws.OnMessage += (sender, e) =>
                {
                    MlResultModelDto mlResultModel = JsonConvert.DeserializeObject<MlResultModelDto>(e.Data);


                    DifficultyHelper.AskDifficultyLevelFromServer(new DifficultyModel { 
                    
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

        private async static void Ws_OnClose(object sender, CloseEventArgs e)
        {
            //Server çökünce veya bağlantı kapanınca burası tetikleniyor...
            Console.WriteLine("Connection Closed. Reconnecting...", e.Reason);
            await ListenServerManager();
        }

        private static void Ws_OnError(object sender, ErrorEventArgs e)
        {
            //Bir hata oluşunca burası tetikleniyor....
            Console.WriteLine("Error: ", e.Message);
        }

        private static void Ws_OnOpen(object sender, EventArgs e)
        {
            //Bağlantı açılınca burası tetikleniyor.
            Console.WriteLine("Opened: ", e);
        }
    }
}
