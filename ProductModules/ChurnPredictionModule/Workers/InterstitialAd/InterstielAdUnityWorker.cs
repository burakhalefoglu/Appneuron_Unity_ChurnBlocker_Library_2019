using Zenject;
using AppneuronUnity.Core.Adapters.RestClientAdapter.Abstract;
using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
using AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.InterstitialAd;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using AppneuronUnity.Core.Extentions;
using AppneuronUnity.Core.AuthModule.ClientIdComponent.DataManager;

namespace AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.InterstitialAd
{
    internal class InterstielAdUnityWorker : IInterstielAdUnityWorker
    {
        private SerializableDictionary<string,int> InterstielFrequency;

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
                    Debug.Log(data.IsAdvSettingsActive);
                    if (data.IsAdvSettingsActive)
                    {
                        InterstielFrequency = data.DefaultIAdvFrequencyStrategy;

                    }
                });
        }

        public async Task GetInterstielFrequencyFromServer()
        {
            //TODO: Remote adsettings linki eklenecek.
            var result = await _restApiClient.GetAsync<InterstielAdModel>("https://jsonplaceholder.typicode.com/posts");
            if(result.Data != null)
                InterstielFrequency = result.Data.DefaultIAdvFrequencyStrategy;
        }

        public SerializableDictionary<string,int> GetInterstielFrequency()
        {
            return InterstielFrequency;
        }
    }
}
