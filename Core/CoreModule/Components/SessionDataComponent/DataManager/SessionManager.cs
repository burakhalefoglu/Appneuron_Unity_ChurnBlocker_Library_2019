namespace AppneuronUnity.Core.CoreModule.Components.SessionDataComponent.DataManager
{
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
    using AppneuronUnity.Core.Adapters.CryptoAdapter.Absrtact;
    using AppneuronUnity.Core.CoreModule.Components.SessionDataComponent.DataAccess;
    using AppneuronUnity.Core.CoreModule.Components.SessionDataComponent.DataModel;
    using Zenject;
    using AppneuronUnity.Core.AuthModule.ClientIdComponent.DataManager;

    internal class SessionManager : ISessionManager
    {

        private readonly ILevelBaseSessionDal _levelBaseSessionDal;

        private readonly IDataCreationClient _dataCreationClient;

        private readonly ICryptoServices _cryptoServices;

        private readonly IGameSessionEveryLoginDal _gameSessionEveryLoginDal;

        private readonly IClientIdUnityManager _clientIdUnityManager;

        [Inject]
        private readonly CoreHelper coreHelper;

        public SessionManager(ILevelBaseSessionDal levelBaseSessionDal,
            IDataCreationClient dataCreationClient,
            ICryptoServices cryptoServices,
            IGameSessionEveryLoginDal gameSessionEveryLoginDal,
            IClientIdUnityManager clientIdUnityManager)
        {
            _levelBaseSessionDal = levelBaseSessionDal;
            _dataCreationClient = dataCreationClient;
            _cryptoServices = cryptoServices;
            _gameSessionEveryLoginDal = gameSessionEveryLoginDal;
            _clientIdUnityManager = clientIdUnityManager;
        }

        public async Task SendLevelbaseSessionData(float sessionSeconds,
                    string levelName,
                    int levelIndex,
                    DateTime levelBaseGameSessionStart)
        {
            DateTime levelBaseGameSessionFinish = levelBaseGameSessionStart.AddSeconds(sessionSeconds);
            float minutes = sessionSeconds / 60;

            var playerId = _clientIdUnityManager.GetPlayerID();
            var projectId = coreHelper.GetProjectInfo().ProjectID;
            var customerId = coreHelper.GetProjectInfo().CustomerID;

            LevelBaseSessionDataModel dataModel = new LevelBaseSessionDataModel
            {
                ClientId = playerId,
                ProjectId = projectId,
                CustomerId = customerId,
                levelName = levelName,
                levelIndex = levelIndex,
                DifficultyLevel = 0,
                SessionStartTime = levelBaseGameSessionStart,
                SessionFinishTime = levelBaseGameSessionFinish,
                SessionTimeMinute = minutes
            };

            await _dataCreationClient.PushAsync(_clientIdUnityManager.GetPlayerID(),
            dataModel, async (result) =>
            {
                if (!result)
                {
                    string fileName = _cryptoServices.GenerateStringName(6);
                    await _levelBaseSessionDal.InsertAsync(fileName, dataModel);
                }
            });
        }

        public async Task CheckLevelBaseSessionDataAndSend()
        {
            List<string> FolderList = coreHelper.GetSavedDataFilesNames<LevelBaseSessionDataModel>();
            if (FolderList.Count == 0)
                return;
            foreach (var fileName in FolderList)
            {
                var dataModel = await _levelBaseSessionDal.SelectAsync(fileName);

                await _dataCreationClient.PushAsync(_clientIdUnityManager.GetPlayerID(),
                dataModel, async (result) =>
                {
                    if (result)
                    {
                        await _levelBaseSessionDal.DeleteAsync(fileName);

                    }
                });
            }
        }

        public async Task SendGameSessionEveryLoginData(DateTime sessionStartTime,
            DateTime sessionFinishTime,
            float minutes)
        {
            var playerId = _clientIdUnityManager.GetPlayerID();
            var projectId = coreHelper.GetProjectInfo().ProjectID;
            var customerId = coreHelper.GetProjectInfo().CustomerID;

            GameSessionEveryLoginDataModel dataModel = new GameSessionEveryLoginDataModel
            {
                ClientId = playerId,
                ProjectId = projectId,
                CustomerId = customerId,
                SessionStartTime = sessionStartTime,
                SessionFinishTime = sessionFinishTime,
                SessionTimeMinute = minutes
            };


            await _dataCreationClient.PushAsync(_clientIdUnityManager.GetPlayerID(),
            dataModel, async (result) =>
            {
                if (!result)
                {
                    string fileName = _cryptoServices.GenerateStringName(6);
                    await _gameSessionEveryLoginDal.InsertAsync(fileName, dataModel);
                }
            });
        }

        public async Task CheckGameSessionEveryLoginDataAndSend()
        {
            List<string> FolderList = coreHelper.GetSavedDataFilesNames<GameSessionEveryLoginDataModel>();
            if (FolderList.Count == 0)
                return;
            foreach (var fileName in FolderList)
            {
                var dataModel = await _gameSessionEveryLoginDal.SelectAsync(fileName);

                await _dataCreationClient.PushAsync(_clientIdUnityManager.GetPlayerID(),
                dataModel, async (result) =>
                {
                    if (result)
                    {
                        await _gameSessionEveryLoginDal.DeleteAsync(fileName);

                    }
                });
            }
        }
    }
}
