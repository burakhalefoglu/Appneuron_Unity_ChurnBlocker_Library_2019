
using Appneuron.Zenject;
using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
using AppneuronUnity.Core.AuthModule.ClientIdComponent.UnityManager;
using UnityEngine;
using AppneuronUnity.ProductModules.ChurnPrediction.WebSocketWorkers.ChurnMLResult.Model;

namespace AppneuronUnity.ProductModules.ChurnPrediction.WebSocketWorkers.ChurnMLResult.UnityWorker
{
    public class ChurnMLResultUnityWorker : MonoBehaviour
    {
        [Inject]
        private IRemoteClient remoteClient;
        [Inject]
        private IClientIdUnityManager clientIdManager;

        private async void Start()
        {
            await remoteClient.SubscribeAsync<MlResultModel>(clientIdManager.GetPlayerID(),
                (data) =>
                {
                    Debug.Log(data.OneDayChurn);
                    //TODO:

                });
        }
    }
}
