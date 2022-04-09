using AppneuronUnity.Core.BaseServices.WebsocketAdapter.WebsocketSharp;

namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.DataManager
{
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.DataAccess;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.Datamodel;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;
    using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
    using AppneuronUnity.Core.Adapters.CryptoAdapter.Absrtact;
    using Zenject;
    using AppneuronUnity.Core.AuthModule.ClientIdComponent.DataManager;

    internal class EnemybaseLevelManager : IEnemybaseLevelManager
    {
        private readonly IEnemyBaseWithLevelDieDal _enemyBaseWithLevelDieDal;

        private readonly ICryptoServices _cryptoServices;

        private readonly IEnemyBaseEveryLoginLevelDal _enemyBaseEveryLoginLevelDal;

        private readonly IClientIdUnityManager _clientIdUnityManager;

        private readonly IDataCreationClient _dataCreationClient;

        [Inject]
        private readonly CoreHelper coreHelper;

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
            var playerId = await _clientIdUnityManager.GetPlayerIdAsync();
            var projectId = coreHelper.GetProjectInfo().ProjectId;
            var customerId = coreHelper.GetProjectInfo().CustomerId;

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

            await _dataCreationClient.PushAsync(await _clientIdUnityManager.GetPlayerIdAsync(),
            coreHelper.GetProjectInfo().ProjectId,
            dataModel, async (result) =>
            {
                if (!result)
                {
                    string fileName = _cryptoServices.GenerateStringName(6);
                    await _enemyBaseWithLevelDieDal.InsertAsync(fileName, dataModel);
                }
            });

        }

        public async Task CheckLevelbaseDieAndSend()
        {
            List<string> FolderList = coreHelper.GetSavedDataFilesNames<EnemyBaseWithLevelFailDataModel>();
            if (FolderList.Count == 0)
                return;
            foreach (var fileName in FolderList)
            {
                var dataModel = await _enemyBaseWithLevelDieDal.SelectAsync(fileName);

                await _dataCreationClient.PushAsync(await _clientIdUnityManager.GetPlayerIdAsync(),
                coreHelper.GetProjectInfo().ProjectId,
                dataModel, async (result) =>
                {
                    if (result)
                    {
                        await _enemyBaseWithLevelDieDal.DeleteAsync(fileName);

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
            var playerId = await _clientIdUnityManager.GetPlayerIdAsync();
            var projectId = coreHelper.GetProjectInfo().ProjectId;
            var customerId = coreHelper.GetProjectInfo().CustomerId;

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


            await _dataCreationClient.PushAsync(await _clientIdUnityManager.GetPlayerIdAsync(),
            coreHelper.GetProjectInfo().ProjectId,
            dataModel, async (result) =>
            {
                if (!result)
                {
                    string fileName = _cryptoServices.GenerateStringName(6);
                    await _enemyBaseEveryLoginLevelDal.InsertAsync(fileName, dataModel);
                }
            });

        }


        public async Task CheckEveryLoginLevelDatasAndSend()
        {
            List<string> FolderList = coreHelper.GetSavedDataFilesNames<EnemyBaseEveryLoginLevelDatasModel>();
            if (FolderList.Count == 0)
                return;
            foreach (var fileName in FolderList)
            {
                var dataModel = await _enemyBaseEveryLoginLevelDal.SelectAsync(fileName);
                await _dataCreationClient.PushAsync(await _clientIdUnityManager.GetPlayerIdAsync(),
                coreHelper.GetProjectInfo().ProjectId,
                dataModel, async (result) =>
                {
                    if (result)
                    {
                        await _enemyBaseEveryLoginLevelDal.DeleteAsync(fileName);

                    }
                });
            }
        }
    }
}
