using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using AppneuronUnity.Core.Libraries.WebSocketSharp;

namespace AppneuronUnity.Core.ObjectBases.WebSocketHelper.WebsocketSharp
{
    internal class WebSocketBase
    {
        private WebSocketSharp webSocket;
        private string userId { get; set;} 
        private string host { get; set; }
        private int port { get; set; }

        public async Task<WebSocketSharp> ListenServerAsync<T>(string userId,
           string websocketServer,
           int websocketPort)
        {
            if (webSocket.IsConnected)
            {
                return webSocket;
            }

            webSocket = SubscribeChannel<T>(websocketServer,
            websocketPort,
            userId);
            this.userId = userId;
            this.host = websocketServer;
            this.port = websocketPort;
            return await Task.FromResult(webSocket);

        }


        private WebSocketSharp SubscribeChannel<T>(string host, int port, string clientId)
        {

            var ChannelName = typeof(T).Name;
            var ws = new WebSocketSharp($"ws://{host}:{port}/" + ChannelName + "?clientid=" + clientId);

            try
            {
                ws.OnOpen += Ws_OnOpen;
                ws.OnError += Ws_OnError;
                ws.OnClose += Ws_OnClose<T>;

                ws.Connect();
                ws.Send("I Am Client...");

            }
            catch (Exception exception)
            {
                //TODO: LOG message;
                Debug.Log(exception.Message);
                Thread.Sleep(5000);
                webSocket = SubscribeChannel<T>(host,
                port,
                clientId);
            }
            return ws;
        }

        private void Ws_OnClose<T>(object sender, CloseEventArgs e)
        {
            //TODO: LOG message
            Debug.Log("Connection Closed. Reconnecting..." + e.Reason);
            Thread.Sleep(5000);
            webSocket = SubscribeChannel<T>(this.host,
                              this.port,
                              this.userId);
        }

        private void Ws_OnError(object sender, ErrorEventArgs e)
        {
            //TODO: LOG message
            Debug.Log("Error: " + e.Message);
        }

        private void Ws_OnOpen(object sender, EventArgs e)
        {
            Debug.Log("Opened: " + e);
        }
    }
}
