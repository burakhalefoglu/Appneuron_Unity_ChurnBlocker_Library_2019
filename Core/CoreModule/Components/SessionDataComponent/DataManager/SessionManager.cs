using AppneuronUnity.Core.BaseServices.WebsocketAdapter.WebsocketSharp;

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

            var playerId = await _clientIdUnityManager.GetPlayerIdAsync();
            var projectId = coreHelper.GetProjectInfo().ProjectId;
            var customerId = coreHelper.GetProjectInfo().CustomerId;

            LevelBaseSessionDataModel dataModel = new LevelBaseSessionDataModel
            {
                ClientId = playerId,
                ProjectId = projectId,
                CustomerId = customerId,
                LevelName = levelName,
                LevelIndex = levelIndex,
                SessionStartTime = levelBaseGameSessionStart,
                SessionFinishTime = levelBaseGameSessionFinish,
                SessionTimeMinute = minutes
            };

            await _dataCreationClient.PushAsync(await _clientIdUnityManager.GetPlayerIdAsync(),
            coreHelper.GetProjectInfo().ProjectId,
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

                await _dataCreationClient.PushAsync(await _clientIdUnityManager.GetPlayerIdAsync(),
                coreHelper.GetProjectInfo().ProjectId,
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
            var playerId = await _clientIdUnityManager.GetPlayerIdAsync();
            var projectId = coreHelper.GetProjectInfo().ProjectId;
            var customerId = coreHelper.GetProjectInfo().CustomerId;

            GameSessionEveryLoginDataModel dataModel = new GameSessionEveryLoginDataModel
            {
                ClientId = playerId,
                ProjectId = projectId,
                CustomerId = customerId,
                SessionStartTime = sessionStartTime,
                SessionFinishTime = sessionFinishTime,
                SessionTime = minutes
            };


            await _dataCreationClient.PushAsync(await _clientIdUnityManager.GetPlayerIdAsync(),
            coreHelper.GetProjectInfo().ProjectId,
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

                await _dataCreationClient.PushAsync(await _clientIdUnityManager.GetPlayerIdAsync(),
                coreHelper.GetProjectInfo().ProjectId,
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
