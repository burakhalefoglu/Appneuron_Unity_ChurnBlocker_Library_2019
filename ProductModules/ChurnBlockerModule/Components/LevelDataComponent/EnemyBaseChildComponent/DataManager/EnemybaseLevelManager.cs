namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.UnityManager
{
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.DataAccess;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.Datamodel;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;
    using AppneuronUnity.Core.AuthModule.ClientIdComponent.UnityManager;
    using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
using AppneuronUnity.Core.Adapters.CryptoAdapter.Absrtact;

    /// <summary>
    /// Defines the <see cref="EnemybaseLevelManager" />.
    /// </summary>
    internal class EnemybaseLevelManager : IEnemybaseLevelManager
    {
        private readonly IEnemyBaseWithLevelDieDal _enemyBaseWithLevelDieDal;

        private readonly ICryptoServices _cryptoServices;

        private readonly IEnemyBaseEveryLoginLevelDal _enemyBaseEveryLoginLevelDal;

        private readonly IClientIdUnityManager _clientIdUnityManager;

        private readonly IDataCreationClient _dataCreationClient;

        public EnemybaseLevelManager(IEnemyBaseWithLevelDieDal enemyBaseWithLevelDieDal,
            IDataCreationClient dataCreationClient,
            ICryptoServices cryptoServices,
            IEnemyBaseEveryLoginLevelDal enemyBaseEveryLoginLevelDal,
            IClientIdUnityManager clientIdUnityManager)
        {
            _enemyBaseWithLevelDieDal = enemyBaseWithLevelDieDal;
            _dataCreationClient = dataCreationClient;
            _cryptoServices = cryptoServices;
            _enemyBaseEveryLoginLevelDal = enemyBaseEveryLoginLevelDal;
            _clientIdUnityManager = clientIdUnityManager;
        }

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

            await _dataCreationClient.PushAsync(_clientIdUnityManager.GetPlayerID(),
            dataModel, async (result) =>
            {
                if (!result)
                {
                    string fileName = _cryptoServices.GenerateStringName(6);
                    await _enemyBaseWithLevelDieDal.InsertAsync(filepath + fileName, dataModel);
                }
            });

        }

        public async Task CheckLevelbaseDieAndSend()
        {
            List<string> FolderList = ComponentsConfigs.GetSavedDataFilesNames(
                ComponentsConfigs.SaveTypePath.LevelBaseDieDataModel);
            foreach (var fileName in FolderList)
            {
                var dataModel = await _enemyBaseWithLevelDieDal.SelectAsync(ComponentsConfigs.LevelBaseDieDataPath + fileName);

                await _dataCreationClient.PushAsync(_clientIdUnityManager.GetPlayerID(),
                dataModel, async (result) =>
                {
                    if (result)
                    {
                        await _enemyBaseWithLevelDieDal.DeleteAsync(ComponentsConfigs.LevelBaseDieDataPath + fileName);

                    }
                });
            }
        }

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


            await _dataCreationClient.PushAsync(_clientIdUnityManager.GetPlayerID(),
            dataModel, async (result) =>
            {
                if (!result)
                {
                    string fileName = _cryptoServices.GenerateStringName(6);
                    string filepath = ComponentsConfigs.EveryLoginLevelDatasPath + fileName;
                    await _enemyBaseEveryLoginLevelDal.InsertAsync(filepath, dataModel);
                }
            });

        }


        public async Task CheckEveryLoginLevelDatasAndSend()
        {
            List<string> FolderList = ComponentsConfigs.GetSavedDataFilesNames(ComponentsConfigs.SaveTypePath.EveryLoginLevelDatasModel);
            foreach (var fileName in FolderList)
            {
                var dataModel = await _enemyBaseEveryLoginLevelDal.SelectAsync(ComponentsConfigs.EveryLoginLevelDatasPath + fileName);
                await _dataCreationClient.PushAsync(_clientIdUnityManager.GetPlayerID(),
                dataModel, async (result) =>
                {
                    if (result)
                    {
                        await _enemyBaseEveryLoginLevelDal.DeleteAsync(ComponentsConfigs.EveryLoginLevelDatasPath + fileName);

                    }
                });
            }
        }
    }
}
