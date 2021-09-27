
using AppneuronUnity.Core.Adapters.RestClientAdapter.Abstract;
using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
using AppneuronUnity.Core.AuthModule.ClientIdComponent.UnityManager;
using System.Threading.Tasks;
using UnityEngine;

namespace AppneuronUnity.ProductModules.ChurnPrediction.Workers.RemoteChurnSettings.RemoteOffer
{
    internal class RemoteOfferUnityWorker : IRemoteOfferUnityWorker
    {
        private readonly IRemoteClient _remoteClient;
        private readonly IClientIdUnityManager _clientIdManager;
        private readonly IRestClientServices _restApiClient;


        private RemoteOfferModel remoteOfferModel;

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

            await _remoteClient.SubscribeAsync<RemoteOfferModel>(_clientIdManager.GetPlayerID(),
                (data) =>
                {
                    Debug.Log(data.FirstPrice);
                    remoteOfferModel = data;
                });
        }

        public async Task GetRemoteOfferFromServer()
        {
            //TODO: Burada remote link eklenecek.
            var result = await _restApiClient.GetAsync<RemoteOfferModel>("");
            remoteOfferModel = result.Data;
        }

        public RemoteOfferModel GetRemoteOffer()
        {
            return remoteOfferModel;
        }

    }
}
