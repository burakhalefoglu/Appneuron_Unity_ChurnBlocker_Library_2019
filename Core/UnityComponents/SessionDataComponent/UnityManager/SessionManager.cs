namespace AppneuronUnity.Core.UnityComponents.SessionDataComponent.UnityManager
{
    using AppneuronUnity.Core.CoreServices.CryptoServices.Absrtact;
    using AppneuronUnity.Core.CoreServices.MessageBrokers;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
using AppneuronUnity.Core.UnityComponents.SessionDataComponent.DataAccess;
using AppneuronUnity.Core.UnityComponents.SessionDataComponent.DataModel;
using AppneuronUnity.Core.UnityComponents.SessionDataComponent.UnityManager;
using AppneuronUnity.Core.UnityWorkers.ClientIdWorker.UnityManager;

    /// <summary>
    /// Defines the <see cref="SessionManager" />.
    /// </summary>
    internal class SessionManager : ISessionManager
    {
        /// <summary>
        /// Defines the _levelBaseSessionDal.
        /// </summary>
        private readonly ILevelBaseSessionDal _levelBaseSessionDal;

        /// <summary>
        /// Defines the _kafkaMessageBroker.
        /// </summary>
        private readonly IMessageBrokerService _kafkaMessageBroker;

        /// <summary>
        /// Defines the _cryptoServices.
        /// </summary>
        private readonly ICryptoServices _cryptoServices;

        /// <summary>
        /// Defines the _gameSessionEveryLoginDal.
        /// </summary>
        private readonly IGameSessionEveryLoginDal _gameSessionEveryLoginDal;

        /// <summary>
        /// Defines the _clientIdUnityManager.
        /// </summary>
        private readonly IClientIdUnityManager _clientIdUnityManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionManager"/> class.
        /// </summary>
        /// <param name="levelBaseSessionDal">The levelBaseSessionDal<see cref="ILevelBaseSessionDal"/>.</param>
        /// <param name="kafkaMessageBroker">The kafkaMessageBroker<see cref="IMessageBrokerService"/>.</param>
        /// <param name="cryptoServices">The cryptoServices<see cref="ICryptoServices"/>.</param>
        /// <param name="gameSessionEveryLoginDal">The gameSessionEveryLoginDal<see cref="IGameSessionEveryLoginDal"/>.</param>
        /// <param name="clientIdUnityManager">The clientIdUnityManager<see cref="IClientIdUnityManager"/>.</param>
        public SessionManager(ILevelBaseSessionDal levelBaseSessionDal,
            IMessageBrokerService kafkaMessageBroker,
            ICryptoServices cryptoServices,
            IGameSessionEveryLoginDal gameSessionEveryLoginDal,
            IClientIdUnityManager clientIdUnityManager)
        {
            _levelBaseSessionDal = levelBaseSessionDal;
            _kafkaMessageBroker = kafkaMessageBroker;
            _cryptoServices = cryptoServices;
            _gameSessionEveryLoginDal = gameSessionEveryLoginDal;
            _clientIdUnityManager = clientIdUnityManager;
        }

        /// <summary>
        /// The SendLevelbaseSessionData.
        /// </summary>
        /// <param name="sessionSeconds">The sessionSeconds<see cref="float"/>.</param>
        /// <param name="levelName">The levelName<see cref="string"/>.</param>
        /// <param name="levelBaseGameSessionStart">The levelBaseGameSessionStart<see cref="DateTime"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
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
            var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
            if (result.Success)
            {
                return;
            }
            string fileName = _cryptoServices.GenerateStringName(6);
            await _levelBaseSessionDal.InsertAsync(filepath + fileName, dataModel);
        }

        /// <summary>
        /// The CheckLevelBaseSessionDataAndSend.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task CheckLevelBaseSessionDataAndSend()
        {
            List<string> FolderList = ComponentsConfigs.GetSavedDataFilesNames(ComponentsConfigs.SaveTypePath.LevelBaseSessionDataModel);
            foreach (var fileName in FolderList)
            {
                var dataModel = await _levelBaseSessionDal.SelectAsync(ComponentsConfigs.LevelBaseSessionDataPath + fileName);
                var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
                if (result.Success)
                {
                    await _levelBaseSessionDal.DeleteAsync(ComponentsConfigs.LevelBaseSessionDataPath + fileName);
                }
            }
        }

        /// <summary>
        /// The SendGameSessionEveryLoginData.
        /// </summary>
        /// <param name="sessionStartTime">The sessionStartTime<see cref="DateTime"/>.</param>
        /// <param name="sessionFinishTime">The sessionFinishTime<see cref="DateTime"/>.</param>
        /// <param name="minutes">The minutes<see cref="float"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
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

            var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
            if (result.Success)
            {
                return;
            }
            string fileName = _cryptoServices.GenerateStringName(6);
            await _gameSessionEveryLoginDal.InsertAsync(filepath + fileName, dataModel);
        }

        /// <summary>
        /// The CheckGameSessionEveryLoginDataAndSend.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task CheckGameSessionEveryLoginDataAndSend()
        {
            List<string> FolderList = ComponentsConfigs.GetSavedDataFilesNames(ComponentsConfigs.SaveTypePath.GameSessionEveryLoginDataModel);
            foreach (var fileName in FolderList)
            {
                var dataModel = await _gameSessionEveryLoginDal.SelectAsync(ComponentsConfigs.GameSessionEveryLoginDataPath + fileName);
                var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
                if (result.Success)
                {
                    await _gameSessionEveryLoginDal.DeleteAsync(ComponentsConfigs.GameSessionEveryLoginDataPath + fileName);
                }
            }
        }
    }
}
