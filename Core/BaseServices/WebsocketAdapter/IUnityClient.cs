using System;
using System.Threading.Tasks;

namespace AppneuronUnity.Core.BaseServices.WebsocketAdapter
{
    internal interface IUnityClient
    {
        Task PushAsync<T>(long userId, long projectId, T model, Action<bool> callback);
        Task SubscribeAsync<T>(long userId, long projectId, Action<T> callback);

    }
}
