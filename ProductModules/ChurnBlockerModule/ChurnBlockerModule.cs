namespace AppneuronUnity.ProductModules.ChurnBlockerModule
{
    using Zenject;
    using AppneuronUnity.Core.CoreModule.Services;
    using System;
    using System.Threading.Tasks;
    using UnityEngine;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.DataManager;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.ManuelFlowComponent.DataManager;

    public class ChurnBlockerModule : MonoBehaviour
    {
        private LocalDataService localDataService;

        [Inject]
        private IManuelFlowDataManager _manuelFlowDataManager;

        [Inject]
        private IEnemybaseLevelManager _enemybaseLevelManager;

        private async void Start()
        {

            localDataService = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<LocalDataService>();
            await LateStart(3);
        }

        private async Task LateStart(float waitTime)
        {
            await Task.Delay(TimeSpan.FromSeconds(waitTime));
            await _enemybaseLevelManager.CheckEveryLoginLevelDatasAndSend();
            await _enemybaseLevelManager.CheckLevelbaseDieAndSend();
            await _manuelFlowDataManager.CheckAdvFileAndSendData();
            localDataService.CheckLocalData += _enemybaseLevelManager.CheckEveryLoginLevelDatasAndSend;
            localDataService.CheckLocalData += _enemybaseLevelManager.CheckLevelbaseDieAndSend;
            localDataService.CheckLocalData += _manuelFlowDataManager.CheckAdvFileAndSendData;
        }

        private void OnApplicationQuit()
        {
            localDataService.CheckLocalData -= _enemybaseLevelManager.CheckEveryLoginLevelDatasAndSend;
            localDataService.CheckLocalData -= _enemybaseLevelManager.CheckLevelbaseDieAndSend;
            localDataService.CheckLocalData -= _manuelFlowDataManager.CheckAdvFileAndSendData;
        }

    }
}
