using Appneuron.Zenject;
using AppneuronUnity.Core.Adapters.RestClientAdapter.Abstract;
using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
using AppneuronUnity.Core.AuthModule.ClientIdComponent.UnityManager;
using AppneuronUnity.ProductModules.ChurnPrediction.WebSocketWorkers.RemoteChurnSettings.InterstitialAd.Model;
using AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.InterstitialAd;
using System.Threading.Tasks;
using UnityEngine;

namespace AppneuronUnity.ProductModules.ChurnPrediction.WebSocketWorkers.RemoteChurnSettings.InterstitialAd.UnityWorker
{
    internal class InterstielAdUnityWorker : IInterstielAdUnityWorker
    {
        private int InterstielFrequency;

        private readonly IRemoteClient _remoteClient;
        private readonly IClientIdUnityManager _clientIdManager;
        private readonly IRestClientServices _restApiClient;

        public InterstielAdUnityWorker(IRemoteClient remoteClient,
            IClientIdUnityManager clientIdManager,
            IRestClientServices restApiClient)
        {
            _remoteClient = remoteClient;
            _clientIdManager = clientIdManager;
            _restApiClient = restApiClient;
        }

        public async Task StartListen()
        {
            await _remoteClient.SubscribeAsync<InterstielAdModel>(_clientIdManager.GetPlayerID(),
                (data) =>
                {
                    Debug.Log(data.frequency);
                    if (data.IsAdvSettingsActive)
                    {
                        InterstielFrequency = data.frequency;

                    }
                });
        }

        public async Task GetInterstielFrequencyFromServer()
        {
            //TODO: Remote adsettings linki eklenecek.
            var result = await _restApiClient.GetAsync<InterstielAdModel>("");
            InterstielFrequency = result.Data.frequency;
        }

        public int GetInterstielFrequency()
        {
            return InterstielFrequency;
        }
    }
}
