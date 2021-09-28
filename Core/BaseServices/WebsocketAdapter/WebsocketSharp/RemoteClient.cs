using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
using AppneuronUnity.Core.Libraries.LitJson;
using System;
using System.Text;
using System.Threading.Tasks;
using AppneuronUnity.Core.ObjectBases.WebSocketHelper.WebsocketSharp;

namespace AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp
{
    internal class RemoteClient : WebSocketBase, IRemoteClient
    {
        public async Task PushAsync<T>(string userId , T model, Action<bool> callback)
        {
            var ws = await ListenServerAsync<T>(userId, Appsettings.WebsocketRemoteServer, Appsettings.WebsocketDataRemotePort);
            var jsonObject = JsonMapper.ToJson(model);

            ws.SendAsync(Encoding.UTF8.GetBytes(jsonObject), (iscompleted)=> {
                callback(iscompleted);
            });
        }


        public async Task SubscribeAsync<T>(string userId, Action<T> callback)
        {
            var ws = await ListenServerAsync<T>(userId, Appsettings.WebsocketRemoteServer, Appsettings.WebsocketDataRemotePort);
            ws.OnMessage += (sender, e) =>
            {
                var model = JsonMapper.ToObject<T>(e.Data);
                callback(model);
            };
        }
    }
}
