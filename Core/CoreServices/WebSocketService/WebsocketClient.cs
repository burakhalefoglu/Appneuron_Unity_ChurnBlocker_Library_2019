using Appneuron.Core.CoreServices.WebSocketService.Models;
using Appneuron.DifficultyManagerComponent;
using Appneuron.Models;
using Newtonsoft.Json;
using System;
using System.Threading;
using WebSocketSharp;

namespace Appneuron.Core.CoreServices.WebSocketService
{
    public class WebsocketClient
    {
        public static void ListenServerManager()
        {
            var connectedServer = ConnectServerManager("127.0.0.1", 3000);

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
                var webSocket = ConnectMlResultServer(data.Port, data.Host);
            };
        }

        private static WebSocket ConnectServerManager(string host, int port)
        {
            var ws = new WebSocket($"ws://{host}:{port}/ClientBehavior?clientid=" + new Random().Next(123, 999));

            try
            {
                ws.Connect();

            }
            catch (Exception exception)
            {

                Console.WriteLine("Hata: ", exception.Message);
                Thread.Sleep(60000);
                ConnectServerManager(host, port);
            }
            return ws;

        }

        private static WebSocket ConnectMlResultServer(int port, string host)
        {
            var ws = new WebSocket($"ws://{host}:{port}/MlResult?clientid=" + new Random().Next(123, 999));

            try
            {
                ws.OnMessage += (sender, e) =>
                {
                    MlResultModelDto mlResultModel = JsonConvert.DeserializeObject<MlResultModelDto>(e.Data);


                    DifficultyManager.AskDifficultyLevelFromServer(new DifficultyModel { 
                    
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
                ConnectServerManager("127.0.0.1", 3000);
            }
            return ws;

        }

        private static void Ws_OnClose(object sender, CloseEventArgs e)
        {
            //Server çökünce veya bağlantı kapanınca burası tetikleniyor...
            Console.WriteLine("Connection Closed. Reconnecting...", e.Reason);
            ListenServerManager();
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
