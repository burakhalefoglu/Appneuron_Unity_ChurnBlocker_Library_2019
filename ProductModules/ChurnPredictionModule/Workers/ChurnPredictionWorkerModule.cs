using Appneuron.Zenject;
using AppneuronUnity.Core.Adapters.RestClientAdapter.Abstract;
using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
using AppneuronUnity.Core.AuthModule.ClientIdComponent.UnityManager;
using AppneuronUnity.ProductModules.ChurnPrediction.WebSocketWorkers.ChurnMLResult.Model;
using AppneuronUnity.ProductModules.ChurnPrediction.Workers.RemoteChurnSettings.RemoteOffer;
using AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.InterstitialAd;
using System.Threading.Tasks;
using UnityEngine;

namespace AppneuronUnity.ProductModules.ChurnPrediction.Workers
{
    public class ChurnPredictionWorkerModule: MonoBehaviour
    {
        [Inject]
        private IRemoteClient remoteClient;

        [Inject]
        private IClientIdUnityManager clientIdManager;

        [Inject]
        private IRestClientServices restApiClient;

        [Inject]
        private IInterstielAdUnityWorker interstielAdUnityWorker;

        [Inject]
        private IRemoteOfferUnityWorker remoteOfferUnityWorker;



        [SerializeField]
        int DefaultInterstielFrequency;
        private RemoteOfferModel remoteOfferModel;

        private async void Start()
        {
            await interstielAdUnityWorker.GetInterstielFrequencyFromServer();
            await remoteOfferUnityWorker.GetRemoteOfferFromServer();

            await AskMlResult();

            await interstielAdUnityWorker.StartListen();
            await remoteOfferUnityWorker.StartListen();

            await remoteClient.SubscribeAsync<ChurnMlResultModel>(clientIdManager.GetPlayerID(),
                (data) =>
                {
                    Debug.Log(data.ThreeDayChurn);
                    if (data.ThreeDayChurn)
                    {
                        DefaultInterstielFrequency = interstielAdUnityWorker.GetInterstielFrequency();
                        remoteOfferModel = remoteOfferUnityWorker.GetRemoteOffer();
                    }


                });
        }

        public int GetDefaultInterstielFrequency()
        {
            return DefaultInterstielFrequency;
        }

        public RemoteOfferModel GetRemoteOffer()
        {
            return remoteOfferModel;
        }

        private async Task AskMlResult()
        {
            //TODO: Churn Ml Result linki eklenecek.
            var result = await restApiClient.GetAsync<ChurnMlResultModel>("");

            if (result.Data.ThreeDayChurn)
            {
                DefaultInterstielFrequency = interstielAdUnityWorker.GetInterstielFrequency();
                remoteOfferModel = remoteOfferUnityWorker.GetRemoteOffer();
            }
        }
    }
}
