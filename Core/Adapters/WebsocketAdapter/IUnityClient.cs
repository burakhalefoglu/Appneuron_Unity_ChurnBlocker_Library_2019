using System;
using System.Threading.Tasks;

namespace AppneuronUnity.Core.Adapters.WebsocketAdapter
{
    internal interface IUnityClient
    {
        Task PushAsync<T>(string userId, T model, Action<bool> callback);
        Task SubscribeAsync<T>(string userId, Action<T> callback);

    }
}
