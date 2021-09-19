using Appneuron.Services;
using AppneuronUnity.Core.UnityManager;
using Assets.Appneuron.Core.CoreServices.CryptoServices.Absrtact;
using Assets.Appneuron.Core.CoreServices.MessageBrockers.Kafka;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.SessionComponent.DataModel;
using Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.SessionDataComponent.DataAccess;
using Ninject;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace AppneuronUnity.ChurnBlockerModule.Components.SessionDataComponent.UnityManager
{
    public class SessionManager
    {
        public async Task SendLevelbaseSessionData(float sessionSeconds,
                    string levelName,
                    DateTime levelBaseGameSessionStart)
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Load(Assembly.GetExecutingAssembly());
                var _levelBaseSessionDal = kernel.Get<ILevelBaseSessionDal>();
                var _kafkaMessageBroker = kernel.Get<IKafkaMessageBroker>();
                var _cryptoServices = kernel.Get<ICryptoServices>();

                string filepath = ComponentsConfigService.LevelBaseSessionDataPath;
                DateTime levelBaseGameSessionFinish = levelBaseGameSessionStart.AddSeconds(sessionSeconds);
                float minutes = sessionSeconds / 60;

                var playerId = new IdUnityManager().GetPlayerID();
                var projectId = ChurnBlockerSingletonConfigService.Instance.GetProjectID();
                var customerId = ChurnBlockerSingletonConfigService.Instance.GetCustomerID();

                LevelBaseSessionDataModel dataModel = new LevelBaseSessionDataModel
                {

                    ClientId = playerId,
                    ProjectID = projectId,
                    CustomerID = customerId,
                    levelName = levelName,
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
        }
        public async Task CheckLevelBaseSessionDataAndSend()
        {
            using (var kernel = new StandardKernel())
            {

                kernel.Load(Assembly.GetExecutingAssembly());
                var _levelBaseSessionDal = kernel.Get<ILevelBaseSessionDal>();
                var _kafkaMessageBroker = kernel.Get<IKafkaMessageBroker>();
                var _cryptoServices = kernel.Get<ICryptoServices>();

                List<string> FolderList = ComponentsConfigService.GetSavedDataFilesNames(ComponentsConfigService.SaveTypePath.LevelBaseSessionDataModel);
                foreach (var fileName in FolderList)
                {
                    var dataModel = await _levelBaseSessionDal.SelectAsync(ComponentsConfigService.LevelBaseSessionDataPath + fileName);
                    var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
                    if (result.Success)
                    {
                        await _levelBaseSessionDal.DeleteAsync(ComponentsConfigService.LevelBaseSessionDataPath + fileName);
                    }
                }
            }
        }

        public async Task SendGameSessionEveryLoginData(DateTime sessionStartTime,
            DateTime sessionFinishTime,
            float minutes)
        {
            
            using (var kernel = new StandardKernel())
            {
                kernel.Load(Assembly.GetExecutingAssembly());
                var _gameSessionEveryLoginDal = kernel.Get<IGameSessionEveryLoginDal>();
                var _kafkaMessageBroker = kernel.Get<IKafkaMessageBroker>();
                var _cryptoServices = kernel.Get<ICryptoServices>();

                string filepath = ComponentsConfigService.GameSessionEveryLoginDataPath;

                var playerId = new IdUnityManager().GetPlayerID();
                var projectId = ChurnBlockerSingletonConfigService.Instance.GetProjectID();
                var customerId = ChurnBlockerSingletonConfigService.Instance.GetCustomerID();

                GameSessionEveryLoginDataModel dataModel = new GameSessionEveryLoginDataModel
                {

                    ClientId = playerId,
                    ProjectID = projectId,
                    CustomerID = customerId,
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
        }

        public async Task CheckGameSessionEveryLoginDataAndSend()
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Load(Assembly.GetExecutingAssembly());
                var _gameSessionEveryLoginDal = kernel.Get<IGameSessionEveryLoginDal>();
                var _kafkaMessageBroker = kernel.Get<IKafkaMessageBroker>();
                var _cryptoServices = kernel.Get<ICryptoServices>();

                Debug.Log("başarılı bir şekilde gerçekleşti");
                List<string> FolderList = ComponentsConfigService.GetSavedDataFilesNames(ComponentsConfigService.SaveTypePath.GameSessionEveryLoginDataModel);
                foreach (var fileName in FolderList)
                {
                    var dataModel = await _gameSessionEveryLoginDal.SelectAsync(ComponentsConfigService.GameSessionEveryLoginDataPath + fileName);
                    var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
                    if (result.Success)
                    {
                        await _gameSessionEveryLoginDal.DeleteAsync(ComponentsConfigService.GameSessionEveryLoginDataPath + fileName);
                    }
                }

            }
        }
    }
}
