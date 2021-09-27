namespace AppneuronUnity.ProductModules.ChurnBlockerModule
{
    using Appneuron.Zenject;
    using AppneuronUnity.Core.CoreModule.Services;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.UnityManager;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.UnityManager;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System;
    using System.Threading.Tasks;
    using UnityEngine;

    public class ChurnBlockerModule : MonoBehaviour
    {
        private LocalDataService localDataService;

        [Inject]
        private IDifficultyManager _difficultyManager;

        [Inject]
        private IEnemybaseLevelManager _enemybaseLevelManager;

        private void Awake()
        {
            ComponentsConfigs.CreateFileLocalDataDirectories();
        }

        private async void Start()
        {
            await _difficultyManager.AskDifficulty();
            localDataService = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<LocalDataService>();
            await LateStart(3);
        }

        private async Task LateStart(float waitTime)
        {
            await Task.Delay(TimeSpan.FromSeconds(waitTime));
            await _enemybaseLevelManager.CheckEveryLoginLevelDatasAndSend();
            await _enemybaseLevelManager.CheckLevelbaseDieAndSend();
            localDataService.CheckLocalData += _enemybaseLevelManager.CheckEveryLoginLevelDatasAndSend;
            localDataService.CheckLocalData += _enemybaseLevelManager.CheckLevelbaseDieAndSend;
        }

        private void OnApplicationQuit()
        {
            localDataService.CheckLocalData -= _enemybaseLevelManager.CheckEveryLoginLevelDatasAndSend;
            localDataService.CheckLocalData -= _enemybaseLevelManager.CheckLevelbaseDieAndSend;
        }

    }
}
