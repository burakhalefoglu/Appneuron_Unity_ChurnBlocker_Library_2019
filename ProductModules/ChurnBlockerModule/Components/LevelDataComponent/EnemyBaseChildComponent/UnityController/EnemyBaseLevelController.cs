namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.UnityController
{
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.UnityManager;
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using UnityEngine;
    using Appneuron.Zenject;
using AppneuronUnity.Core.CoreModule.Services;

    /// <summary>
    /// Defines the <see cref="EnemyBaseLevelController" />.
    /// </summary>
    public class EnemyBaseLevelController : MonoBehaviour
    {
        /// <summary>
        /// Defines the localDataService.
        /// </summary>
        private LocalDataService localDataService;

        [Inject]
        private IEnemybaseLevelManager _enemybaseLevelManager;

        /// <summary>
        /// The Start.
        /// </summary>
        private async void Start()
        {

            localDataService = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<LocalDataService>();
            await LateStart(3);
        }

        /// <summary>
        /// The LateStart.
        /// </summary>
        /// <param name="waitTime">The waitTime<see cref="float"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task LateStart(float waitTime)
        {
            await Task.Delay(TimeSpan.FromSeconds(waitTime));
            await _enemybaseLevelManager.CheckEveryLoginLevelDatasAndSend();
            await _enemybaseLevelManager.CheckLevelbaseDieAndSend();
            localDataService.CheckLocalData += _enemybaseLevelManager.CheckEveryLoginLevelDatasAndSend;
            localDataService.CheckLocalData += _enemybaseLevelManager.CheckLevelbaseDieAndSend;
        }

        /// <summary>
        /// The OnApplicationQuit.
        /// </summary>
        private void OnApplicationQuit()
        {
            localDataService.CheckLocalData -= _enemybaseLevelManager.CheckEveryLoginLevelDatasAndSend;
            localDataService.CheckLocalData -= _enemybaseLevelManager.CheckLevelbaseDieAndSend;
        }
    }
}
