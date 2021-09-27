namespace AppneuronUnity.Core.CoreModule.Components.SessionDataComponent.UnityManager
{
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AppneuronUnity.Core.AuthModule.ClientIdComponent.UnityManager;
    using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
using AppneuronUnity.Core.Adapters.CryptoAdapter.Absrtact;
using AppneuronUnity.Core.CoreModule.Components.SessionDataComponent.DataAccess;
using AppneuronUnity.Core.CoreModule.Components.SessionDataComponent.DataModel;

    internal class SessionManager : ISessionManager
    {

        private readonly ILevelBaseSessionDal _levelBaseSessionDal;

        private readonly IDataCreationClient _dataCreationClient;

        private readonly ICryptoServices _cryptoServices;

        private readonly IGameSessionEveryLoginDal _gameSessionEveryLoginDal;

        private readonly IClientIdUnityManager _clientIdUnityManager;

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
            string filepath = ComponentsConfigs.LevelBaseSessionDataPath;
            DateTime levelBaseGameSessionFinish = levelBaseGameSessionStart.AddSeconds(sessionSeconds);
            float minutes = sessionSeconds / 60;

            var playerId = _clientIdUnityManager.GetPlayerID();
            var projectId = ChurnBlockerSingletonConfigs.Instance.GetProjectID();
            var customerId = ChurnBlockerSingletonConfigs.Instance.GetCustomerID();

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
                    await _levelBaseSessionDal.InsertAsync(filepath + fileName, dataModel);
                }
            });
        }

        public async Task CheckLevelBaseSessionDataAndSend()
        {
            List<string> FolderList = ComponentsConfigs.GetSavedDataFilesNames(ComponentsConfigs.SaveTypePath.LevelBaseSessionDataModel);
            foreach (var fileName in FolderList)
            {
                var dataModel = await _levelBaseSessionDal.SelectAsync(ComponentsConfigs.LevelBaseSessionDataPath + fileName);

                await _dataCreationClient.PushAsync(_clientIdUnityManager.GetPlayerID(),
                dataModel, async (result) =>
                {
                    if (result)
                    {
                        await _levelBaseSessionDal.DeleteAsync(ComponentsConfigs.LevelBaseSessionDataPath + fileName);

                    }
                });
            }
        }

        public async Task SendGameSessionEveryLoginData(DateTime sessionStartTime,
            DateTime sessionFinishTime,
            float minutes)
        {
            string filepath = ComponentsConfigs.GameSessionEveryLoginDataPath;
            var playerId = _clientIdUnityManager.GetPlayerID();
            var projectId = ChurnBlockerSingletonConfigs.Instance.GetProjectID();
            var customerId = ChurnBlockerSingletonConfigs.Instance.GetCustomerID();

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
                    await _gameSessionEveryLoginDal.InsertAsync(filepath + fileName, dataModel);
                }
            });
        }

        public async Task CheckGameSessionEveryLoginDataAndSend()
        {
            List<string> FolderList = ComponentsConfigs.GetSavedDataFilesNames(ComponentsConfigs.SaveTypePath.GameSessionEveryLoginDataModel);
            foreach (var fileName in FolderList)
            {
                var dataModel = await _gameSessionEveryLoginDal.SelectAsync(ComponentsConfigs.GameSessionEveryLoginDataPath + fileName);

                await _dataCreationClient.PushAsync(_clientIdUnityManager.GetPlayerID(),
                dataModel, async (result) =>
                {
                    if (result)
                    {
                        await _gameSessionEveryLoginDal.DeleteAsync(ComponentsConfigs.GameSessionEveryLoginDataPath + fileName);

                    }
                });
            }
        }
    }
}
