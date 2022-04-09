using Zenject;
using AppneuronUnity.Core.Adapters.RestClientAdapter.Abstract;
using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
using System.Threading.Tasks;
using UnityEngine;
using AppneuronUnity.Core.Extentions;
using AppneuronUnity.Core.AuthModule.ClientIdComponent.DataManager;
using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;

namespace AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.InterstitialAd
{
    internal class InterstitialAdUnityWorker : IInterstitialAdUnityWorker
    {
        private SerializableDictionary<string,int> InterstitialFrequency;

        private readonly IRemoteClient _remoteClient;
        private readonly IClientIdUnityManager _clientIdManager;
        private readonly IRestClientServices _restApiClient;

        [Inject]
        private readonly CoreHelper coreHelper;

        public InterstitialAdUnityWorker(IRemoteClient remoteClient,
            IClientIdUnityManager clientIdManager,
            IRestClientServices restApiClient)
        {
            _remoteClient = remoteClient;
            _clientIdManager = clientIdManager;
            _restApiClient = restApiClient;
        }

        public async Task StartListen()
        {
            await _remoteClient.SubscribeAsync<InterstitialAdEventModel>(await _clientIdManager.GetPlayerIdAsync(),
                coreHelper.GetProjectInfo().ProjectId,
                (data) =>
                {
                    Debug.Log(data.IsAdvSettingsActive);
                    if (data.IsAdvSettingsActive)
                    {
                        InterstitialFrequency = data.AdvFrequencyStrategies;

                    }
                });
        }

        public async Task GetInterstitialFrequencyFromServer()
        {
            //TODO: Remote adsettings linki eklenecek.
            var result = await _restApiClient.GetAsync<InterstitialAdEventModel>("https://jsonplaceholder.typicode.com/posts");
            if(result.Data != null)
                InterstitialFrequency = result.Data.AdvFrequencyStrategies;
        }

        public SerializableDictionary<string,int> GetInterstitialFrequency()
        {
            return InterstitialFrequency;
        }
    }
}
