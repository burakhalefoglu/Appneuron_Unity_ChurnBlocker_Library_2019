namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.UnityManager
{
    using AppneuronUnity.Core.CoreServices.CryptoServices.Absrtact;
    using AppneuronUnity.Core.CoreServices.MessageBrokers;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.DataAccess;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.Datamodel;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;
using AppneuronUnity.Core.UnityWorkers.ClientIdWorker.UnityManager;

    /// <summary>
    /// Defines the <see cref="EnemybaseLevelManager" />.
    /// </summary>
    internal class EnemybaseLevelManager : IEnemybaseLevelManager
    {
        /// <summary>
        /// Defines the _enemyBaseWithLevelDieDal.
        /// </summary>
        private readonly IEnemyBaseWithLevelDieDal _enemyBaseWithLevelDieDal;

        /// <summary>
        /// Defines the _kafkaMessageBroker.
        /// </summary>
        private readonly IMessageBrokerService _kafkaMessageBroker;

        /// <summary>
        /// Defines the _cryptoServices.
        /// </summary>
        private readonly ICryptoServices _cryptoServices;

        /// <summary>
        /// Defines the _enemyBaseEveryLoginLevelDal.
        /// </summary>
        private readonly IEnemyBaseEveryLoginLevelDal _enemyBaseEveryLoginLevelDal;

        /// <summary>
        /// Defines the _clientIdUnityManager.
        /// </summary>
        private readonly IClientIdUnityManager _clientIdUnityManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnemybaseLevelManager"/> class.
        /// </summary>
        /// <param name="enemyBaseWithLevelDieDal">The enemyBaseWithLevelDieDal<see cref="IEnemyBaseWithLevelDieDal"/>.</param>
        /// <param name="kafkaMessageBroker">The kafkaMessageBroker<see cref="IMessageBrokerService"/>.</param>
        /// <param name="cryptoServices">The cryptoServices<see cref="ICryptoServices"/>.</param>
        /// <param name="enemyBaseEveryLoginLevelDal">The enemyBaseEveryLoginLevelDal<see cref="IEnemyBaseEveryLoginLevelDal"/>.</param>
        /// <param name="clientIdUnityManager">The clientIdUnityManager<see cref="IClientIdUnityManager"/>.</param>
        public EnemybaseLevelManager(IEnemyBaseWithLevelDieDal enemyBaseWithLevelDieDal,
            IMessageBrokerService kafkaMessageBroker,
            ICryptoServices cryptoServices,
            IEnemyBaseEveryLoginLevelDal enemyBaseEveryLoginLevelDal,
            IClientIdUnityManager clientIdUnityManager)
        {
            _enemyBaseWithLevelDieDal = enemyBaseWithLevelDieDal;
            _kafkaMessageBroker = kafkaMessageBroker;
            _cryptoServices = cryptoServices;
            _enemyBaseEveryLoginLevelDal = enemyBaseEveryLoginLevelDal;
            _clientIdUnityManager = clientIdUnityManager;
        }

        /// <summary>
        /// The SendData.
        /// </summary>
        /// <param name="TransformX">The TransformX<see cref="float"/>.</param>
        /// <param name="TransformY">The TransformY<see cref="float"/>.</param>
        /// <param name="TransformZ">The TransformZ<see cref="float"/>.</param>
        /// <param name="IsDead">The IsDead<see cref="bool"/>.</param>
        /// <param name="AverageScores">The AverageScores<see cref="int"/>.</param>
        /// <param name="TotalPowerUsage">The TotalPowerUsage<see cref="int"/>.</param>
        /// <param name="sceneName">The sceneName<see cref="string"/>.</param>
        /// <param name="İnTime">The İnTime<see cref="int"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SendData
           (float TransformX,
           float TransformY,
           float TransformZ,
           bool IsDead,
           int AverageScores,
           int TotalPowerUsage, string sceneName,
           int levelIndex,

           int İnTime)
        {
            Vector3 transform = new Vector3(TransformX,
             TransformY,
             TransformZ);
            int İsDead = 0;
            if (IsDead)
            {
                İsDead = 1;
                await SendLevelbaseDieDatas
                 (sceneName,
                 İnTime,
                  levelIndex,
                 transform);
            }

            await SendEveryLoginLevelDatas(sceneName,
                İnTime,
                AverageScores,
                levelIndex,
                İsDead,
                TotalPowerUsage);
        }

        /// <summary>
        /// The SendLevelbaseDieDatas.
        /// </summary>
        /// <param name="levelName">The levelName<see cref="string"/>.</param>
        /// <param name="minutes">The minutes<see cref="int"/>.</param>
        /// <param name="transform">The transform<see cref="Vector3"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task SendLevelbaseDieDatas
            (string levelName,
            int minutes,
            int levelIndex,
            Vector3 transform)
        {
            string filepath = ComponentsConfigs.LevelBaseDieDataPath;

            var playerId = _clientIdUnityManager.GetPlayerID();
            var projectId = ChurnBlockerSingletonConfigs.Instance.GetProjectID();
            var customerId = ChurnBlockerSingletonConfigs.Instance.GetCustomerID();

            EnemyBaseWithLevelFailDataModel dataModel = new EnemyBaseWithLevelFailDataModel
            {
                ClientId = playerId,
                ProjectId = projectId,
                CustomerId = customerId,
                levelName = levelName,
                DiyingTimeAfterLevelStarting = minutes,
                levelIndex = levelIndex,
                FailLocationX = transform.x,
                FailLocationY = transform.y,
                FailLocationZ = transform.z
            };

            var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
            if (result.Success)
            {
                return;
            }
            string fileName = _cryptoServices.GenerateStringName(6);
            await _enemyBaseWithLevelDieDal.InsertAsync(filepath + fileName, dataModel);
        }

        /// <summary>
        /// The CheckLevelbaseDieAndSend.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task CheckLevelbaseDieAndSend()
        {
            List<string> FolderList = ComponentsConfigs.GetSavedDataFilesNames(
                ComponentsConfigs.SaveTypePath.LevelBaseDieDataModel);
            foreach (var fileName in FolderList)
            {
                var dataModel = await _enemyBaseWithLevelDieDal.SelectAsync(ComponentsConfigs.LevelBaseDieDataPath + fileName);
                var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
                if (result.Success)
                {
                    await _enemyBaseWithLevelDieDal.DeleteAsync(ComponentsConfigs.LevelBaseDieDataPath + fileName);
                }
            }
        }

        /// <summary>
        /// The SendEveryLoginLevelDatas.
        /// </summary>
        /// <param name="levelname">The levelname<see cref="string"/>.</param>
        /// <param name="minutes">The minutes<see cref="int"/>.</param>
        /// <param name="averageScores">The averageScores<see cref="int"/>.</param>
        /// <param name="isDead">The isDead<see cref="int"/>.</param>
        /// <param name="totalPowerUsage">The totalPowerUsage<see cref="int"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task SendEveryLoginLevelDatas
            (string levelname,
            int minutes,
            int averageScores,
            int levelIndex,
            int isDead,
            int totalPowerUsage)
        {
            var playerId = _clientIdUnityManager.GetPlayerID();
            var projectId = ChurnBlockerSingletonConfigs.Instance.GetProjectID();
            var customerId = ChurnBlockerSingletonConfigs.Instance.GetCustomerID();

            EnemyBaseEveryLoginLevelDatasModel dataModel = new EnemyBaseEveryLoginLevelDatasModel
            {
                ClientId = playerId,
                ProjectId = projectId,
                CustomerId = customerId,
                Levelname = levelname,
                PlayingTime = minutes,
                AverageScores = averageScores,
                levelIndex = levelIndex,
                IsDead = isDead,
                TotalPowerUsage = totalPowerUsage
            };

            var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);

            if (result.Success)
            {
                return;
            }
            string fileName = _cryptoServices.GenerateStringName(6);
            string filepath = ComponentsConfigs.EveryLoginLevelDatasPath + fileName;

            await _enemyBaseEveryLoginLevelDal.InsertAsync(filepath, dataModel);
        }

        /// <summary>
        /// The CheckEveryLoginLevelDatasAndSend.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task CheckEveryLoginLevelDatasAndSend()
        {
            List<string> FolderList = ComponentsConfigs.GetSavedDataFilesNames(ComponentsConfigs.SaveTypePath.EveryLoginLevelDatasModel);
            foreach (var fileName in FolderList)
            {
                var dataModel = await _enemyBaseEveryLoginLevelDal.SelectAsync(ComponentsConfigs.EveryLoginLevelDatasPath + fileName);
                var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
                if (result.Success)
                {
                    await _enemyBaseEveryLoginLevelDal.DeleteAsync(ComponentsConfigs.EveryLoginLevelDatasPath + fileName);
                }
            }
        }
    }
}
