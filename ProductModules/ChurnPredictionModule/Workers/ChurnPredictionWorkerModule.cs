﻿using Zenject;
using AppneuronUnity.Core.Adapters.RestClientAdapter.Abstract;
using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
using AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.InterstitialAd;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using AppneuronUnity.Core.Extentions;
using System;
using AppneuronUnity.ProductModules.ChurnPredictionModule.Workers.RemoteOffer;
using AppneuronUnity.ProductModules.ChurnPredictionModule.Workers;
using AppneuronUnity.Core.AuthModule.ClientIdComponent.DataManager;

namespace AppneuronUnity.ProductModules.ChurnPredictionModule.Workers
{
    [Serializable] public class MyDictionary1 : SerializableDictionary<string, int> { }


    public class ChurnPredictionWorkerModule : MonoBehaviour
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

        public MyDictionary1 DefaultIAdvFrequencyStrategy;


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
                        DefaultIAdvFrequencyStrategy = (MyDictionary1)interstielAdUnityWorker.GetInterstielFrequency();
                        remoteOfferModel = remoteOfferUnityWorker.GetRemoteOffer();
                    }


                });
        }

        public SerializableDictionary<string, int> GetDefaultInterstielFrequency()
        {
            return DefaultIAdvFrequencyStrategy;
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
                DefaultIAdvFrequencyStrategy = (MyDictionary1)interstielAdUnityWorker.GetInterstielFrequency();
                remoteOfferModel = remoteOfferUnityWorker.GetRemoteOffer();
            }
        }
    }
}
