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
        private long userId { get; set; }
        private long projectId { get; set; }
        private string host { get; set; }
        private int port { get; set; }

        public async Task<WebSocketSharp> ListenServerAsync<T>(long userId,
            long projectId,
           string websocketServer,
           int websocketPort)
        {
            return await Task.Run(async () =>
            {
                if (webSocket == null || !webSocket.IsConnected)
                {
                    webSocket = await SubscribeChannel<T>(websocketServer,
                    websocketPort,
                    userId, projectId);
                    this.userId = userId;
                    this.projectId = projectId;
                    this.host = websocketServer;
                    this.port = websocketPort;
                }

                return webSocket;
            });
           
        }


        private async Task<WebSocketSharp> SubscribeChannel<T>(string host, int port, long clientId, long projectId)
        {
            return await Task.Run( async () =>
            {
                var ChannelName = typeof(T).Name;
                var ws = new WebSocketSharp($"ws://{host}:{port}/" + ChannelName + "?clientId=" + clientId + "&projectId=" + projectId);

                try
                {
                    //TODO: after log open it.
                    //ws.OnOpen += Ws_OnOpen;
                    //ws.OnError += Ws_OnError;
                    ws.OnClose += Ws_OnClose<T>;

                    ws.Connect();
                    ws.Send("I Am Client...");

                }
                catch (Exception exception)
                {
                    //TODO: LOG message;
                    Debug.Log(exception.Message);
                    Thread.Sleep(50000);
                    webSocket = await SubscribeChannel<T>(host,
                    port,
                    clientId, projectId);
                }
                return ws;
            });

          
        }

        private async void Ws_OnClose<T>(object sender, CloseEventArgs e)
        {
            //TODO: LOG message
            Debug.Log("Connection Closed. Reconnecting..." + e.Reason);
            Thread.Sleep(50000);
            webSocket = await SubscribeChannel<T>(this.host,
                              this.port,
                              this.userId,
                              this.projectId);
        }

        //private async void Ws_OnError(object sender, ErrorEventArgs e)
        //{
        //    //TODO: LOG message
        //    Debug.Log("Error: " + e.Message);
        //}

        //private async void Ws_OnOpen(object sender, EventArgs e)
        //{
        //    Debug.Log("Opened: " + e);
        //}
    }
}
