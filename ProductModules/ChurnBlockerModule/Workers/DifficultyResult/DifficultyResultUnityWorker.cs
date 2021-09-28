using AppneuronUnity.Core.Adapters.RestClientAdapter.Abstract;
using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
using AppneuronUnity.Core.AuthModule.ClientIdComponent.DataManager;
using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.Helper;
using AppneuronUnity.ProductModules.ChurnBlockerModule.WeboscketWorkers.DifficultyResult.Models;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Workers.DifficultyResult
{
    internal class DifficultyResultUnityWorker: IDifficultyResultUnityWorker
    {
        private readonly IRemoteClient _remoteClient;
        private readonly IClientIdUnityManager _clientIdManager;
        private readonly IRestClientServices _restApiClient;

        [Inject]
        private readonly DifficultyHelper difficultyHelper;

        public DifficultyResultUnityWorker(IRemoteClient remoteClient,
            IClientIdUnityManager clientIdManager,
            IRestClientServices restApiClient)
        {
            _remoteClient = remoteClient;
            _clientIdManager = clientIdManager;
            _restApiClient = restApiClient;
        }

        public async Task StartListen()
        {

            await _remoteClient.SubscribeAsync<DifficultyServerResultModel>(_clientIdManager.GetPlayerID(),
                async (data) =>
                {
                    Debug.Log(data.CenterOfDifficultyLevel);
                    if (data.CenterOfDifficultyLevel != 0)
                    {
                        await difficultyHelper.AskDifficultyLevelFromServer(data);

                    }
                });
        }

        public async Task GetDifficultyFromServer()
        {
            var productId = Appsettings.ChurnBlocker;
            var url = Appsettings.ClientWebApiLink + Appsettings.MlResultRequestName + "?productId=" + productId;

            DifficultyServerResultModel difficultyModel = new DifficultyServerResultModel();
            var result = await _restApiClient.GetAsync<DifficultyServerResultModel>(url);

            if (result.Data == null)
            {
                difficultyModel.CenterOfDifficultyLevel = 0;
                difficultyModel.RangeCount = 2;
            }
            else
            {
                difficultyModel = result.Data;
            }

            await difficultyHelper.AskDifficultyLevelFromServer(difficultyModel);
        }

    }
}
