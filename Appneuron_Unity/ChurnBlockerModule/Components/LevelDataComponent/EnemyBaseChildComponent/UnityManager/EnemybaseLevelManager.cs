using Appneuron.Services;
using AppneuronUnity.Core.UnityManager;
using Assets.Appneuron.Core.CoreServices.CryptoServices.Absrtact;
using Assets.Appneuron.Core.CoreServices.MessageBrockers.Kafka;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.DataAccess;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.Datamodel;
using Ninject;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace AppneuronUnity.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.UnityManager
{

    public class EnemybaseLevelManager
    {

        public async Task SendData
           (float TransformX,
           float TransformY,
           float TransformZ,
           bool IsDead,
           int AverageScores,
           int TotalPowerUsage, string sceneName,
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
                 transform);
            }

            await SendEveryLoginLevelDatas(sceneName,
                İnTime,
                AverageScores,
                İsDead,
                TotalPowerUsage);

        }

        private async Task SendLevelbaseDieDatas
            (string levelName,
            int minutes,
            Vector3 transform)
        {

            string filepath = ComponentsConfigService.LevelBaseDieDataPath;
            using (var kernel = new StandardKernel())
            {

                kernel.Load(Assembly.GetExecutingAssembly());
                var _levelBaseDieDal = kernel.Get<IEnemyBaseWithLevelDieDal>();
                var _kafkaMessageBroker = kernel.Get<IKafkaMessageBroker>();
                var _cryptoServices = kernel.Get<ICryptoServices>();

                var playerId = new IdUnityManager().GetPlayerID();
                var projectId = ChurnBlockerSingletonConfigService.Instance.GetProjectID();
                var customerId = ChurnBlockerSingletonConfigService.Instance.GetCustomerID();

                EnemyBaseWithLevelFailDataModel dataModel = new EnemyBaseWithLevelFailDataModel
                {

                    ClientId = playerId,
                    ProjectID = projectId,
                    CustomerID = customerId,
                    levelName = levelName,
                    DiyingTimeAfterLevelStarting = minutes,
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
                await _levelBaseDieDal.InsertAsync(filepath + fileName, dataModel);
            }
        }

        public async Task CheckLevelbaseDieAndSend()
        {
            using (var kernel = new StandardKernel())
            {

                kernel.Load(Assembly.GetExecutingAssembly());
                var _levelBaseDieDal = kernel.Get<IEnemyBaseWithLevelDieDal>();
                var _kafkaMessageBroker = kernel.Get<IKafkaMessageBroker>();


                List<string> FolderList = ComponentsConfigService.GetSavedDataFilesNames(
                    ComponentsConfigService.SaveTypePath.LevelBaseDieDataModel);
                foreach (var fileName in FolderList)
                {
                    var dataModel = await _levelBaseDieDal.SelectAsync(ComponentsConfigService.LevelBaseDieDataPath + fileName);
                    var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
                    if (result.Success)
                    {
                        await _levelBaseDieDal.DeleteAsync(ComponentsConfigService.LevelBaseDieDataPath + fileName);
                    }
                }
            }
        }





        private async Task SendEveryLoginLevelDatas
            (string levelname,
            int minutes,
            int averageScores,
            int isDead,
            int totalPowerUsage)
        {

            using (var kernel = new StandardKernel())
            {
                kernel.Load(Assembly.GetExecutingAssembly());
                var _everyLoginLevelDal = kernel.Get<IEnemyBaseEveryLoginLevelDal>();
                var _kafkaMessageBroker = kernel.Get<IKafkaMessageBroker>();
                var _cryptoServices = kernel.Get<ICryptoServices>();

                var playerId = new IdUnityManager().GetPlayerID();
                var projectId = ChurnBlockerSingletonConfigService.Instance.GetProjectID();
                var customerId = ChurnBlockerSingletonConfigService.Instance.GetCustomerID();

                EnemyBaseEveryLoginLevelDatasModel dataModel = new EnemyBaseEveryLoginLevelDatasModel
                {

                    ClientId = playerId,
                    ProjectID = projectId,
                    CustomerID = customerId,
                    Levelname = levelname,
                    PlayingTime = minutes,
                    AverageScores = averageScores,
                    IsDead = isDead,
                    TotalPowerUsage = totalPowerUsage

                };

                var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);

                if (result.Success)
                {
                    return;
                }
                string fileName = _cryptoServices.GenerateStringName(6);
                string filepath = ComponentsConfigService.EveryLoginLevelDatasPath + fileName;

                await _everyLoginLevelDal.InsertAsync(filepath, dataModel);
            }
        }


        public async Task CheckEveryLoginLevelDatasAndSend()
        {

            using (var kernel = new StandardKernel())
            {

                kernel.Load(Assembly.GetExecutingAssembly());
                var _everyLoginLevelDal = kernel.Get<IEnemyBaseEveryLoginLevelDal>();
                var _kafkaMessageBroker = kernel.Get<IKafkaMessageBroker>();


                List<string> FolderList = ComponentsConfigService.GetSavedDataFilesNames(ComponentsConfigService.SaveTypePath.EveryLoginLevelDatasModel);
                foreach (var fileName in FolderList)
                {
                    var dataModel = await _everyLoginLevelDal.SelectAsync(ComponentsConfigService.EveryLoginLevelDatasPath + fileName);
                    var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
                    if (result.Success)
                    {
                        await _everyLoginLevelDal.DeleteAsync(ComponentsConfigService.EveryLoginLevelDatasPath + fileName);
                    }
                }
            }

        }

    }
}
