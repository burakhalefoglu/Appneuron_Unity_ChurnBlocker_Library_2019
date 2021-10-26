using Zenject;
using AppneuronUnity.Core.Adapters.RestClientAdapter.Abstract;
using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
using AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.InterstitialAd;
using System.Threading.Tasks;
using UnityEngine;
using AppneuronUnity.Core.Extentions;
using AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.RemoteOffer;
using AppneuronUnity.Core.AuthModule.ClientIdComponent.DataManager;
using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;

namespace AppneuronUnity.ProductModules.ChurnPredictionModule.Workers
{
    public class ChurnPredictionWorkerModule : MonoBehaviour
    {

        [Inject]
        private IRemoteClient remoteClient;

        [Inject]
        private IClientIdUnityManager clientIdManager;

        [Inject]
        private IRestClientServices restApiClient;

        [Inject]
        private IInterstitialAdUnityWorker interstitialAdUnityWorker;

        [Inject]
        private IRemoteOfferUnityWorker remoteOfferUnityWorker;

        [Inject]
        private readonly CoreHelper coreHelper;

        public DefaultIAdvFrequencyStrategy defaultIAdvFrequencyStrategy;

        private bool isChurnUser;

        private RemoteOfferModel remoteOfferModel;

        public delegate Task OnIsChurnResult();

        public event OnIsChurnResult IsChurnResult;

        private async void Start()
        {
            isChurnUser = false;
            await interstitialAdUnityWorker.GetInterstitialFrequencyFromServer();
            await remoteOfferUnityWorker.GetRemoteOfferFromServer();

            await AskMlResult();

            await interstitialAdUnityWorker.StartListen();
            await remoteOfferUnityWorker.StartListen();

            await remoteClient.SubscribeAsync<ChurnMlResultModel>(clientIdManager.GetPlayerID(),
                coreHelper.GetProjectInfo().ProjectID,
                (data) =>
                {
                    Debug.Log(data.ThreeDayChurn);
                    if (data.ThreeDayChurn)
                    {
                        defaultIAdvFrequencyStrategy = (DefaultIAdvFrequencyStrategy)interstitialAdUnityWorker.GetInterstitialFrequency();
                        remoteOfferModel = remoteOfferUnityWorker.GetRemoteOffer();
                        IsChurnResult();
                    }


                });
        }

        public SerializableDictionary<string, int> GetDefaultInterstitialFrequency()
        {
            return defaultIAdvFrequencyStrategy;
        }

        public RemoteOfferModel GetRemoteOffer()
        {
            return remoteOfferModel;
        }

        private async Task AskMlResult()
        {
            //TODO: Churn Ml Result linki eklenecek.
            var result = await restApiClient.GetAsync<ChurnMlResultModel>("https://jsonplaceholder.typicode.com/posts");
            if (result.Data == null)
            {
                return;
            }
            if (result.Data.ThreeDayChurn)
            {
                defaultIAdvFrequencyStrategy = (DefaultIAdvFrequencyStrategy)interstitialAdUnityWorker.GetInterstitialFrequency();
                remoteOfferModel = remoteOfferUnityWorker.GetRemoteOffer();
            }
        }

        public bool GetChurnResult()
        {
            return isChurnUser;
        }

    }
}
