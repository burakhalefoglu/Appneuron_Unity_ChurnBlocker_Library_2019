
using AppneuronUnity.Core.Adapters.RestClientAdapter.Abstract;
using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
using System.Threading.Tasks;
using UnityEngine;
using AppneuronUnity.Core.AuthModule.ClientIdComponent.DataManager;
using Zenject;
using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;

namespace AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.RemoteOffer
{
    internal class RemoteOfferUnityWorker : IRemoteOfferUnityWorker
    {
        private readonly IRemoteClient _remoteClient;
        private readonly IClientIdUnityManager _clientIdManager;
        private readonly IRestClientServices _restApiClient;


        private RemoteOfferEventModel remoteOfferModel;

        [Inject]
        private readonly CoreHelper coreHelper;

        public RemoteOfferUnityWorker(IRemoteClient remoteClient,
            IClientIdUnityManager clientIdManager,
            IRestClientServices restApiClient)
        {
            _remoteClient = remoteClient;
            _clientIdManager = clientIdManager;
            _restApiClient = restApiClient;
        }

        public async Task StartListen()
        { 

            await _remoteClient.SubscribeAsync<RemoteOfferEventModel>(await _clientIdManager.GetPlayerIdAsync(),
                coreHelper.GetProjectInfo().ProjectId,
                (data) =>
                {
                    Debug.Log(data.FirstPrice);
                    remoteOfferModel = data;
                });
        }

        public async Task GetRemoteOfferFromServer()
        {
            //TODO: Burada remote link eklenecek.
            var result = await _restApiClient.GetAsync<RemoteOfferEventModel>("https://jsonplaceholder.typicode.com/posts");
            if(result.Data != null)
                remoteOfferModel = result.Data;
        }

        public RemoteOfferEventModel GetRemoteOffer()
        {
            return remoteOfferModel;
        }

    }
}
