namespace AppneuronUnity.Core.CoreModule.Components.AdvDataComponent.UnityController
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using UnityEngine;
    using Appneuron.Zenject;
using AppneuronUnity.Core.CoreModule.Components.AdvDataComponent.UnityManager;
using AppneuronUnity.Core.CoreModule.Services;

    /// <summary>
    /// Defines the <see cref="AdvEventUnityController" />.
    /// </summary>
    public class AdvEventUnityController : MonoBehaviour
    {
        /// <summary>
        /// Defines the localDataService.
        /// </summary>
        private LocalDataService localDataService;

        [Inject]
        private IAdvEventUnityManager _advEventUnityManager;

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
            await _advEventUnityManager.CheckAdvFileAndSendData();
            localDataService.CheckLocalData += _advEventUnityManager.CheckAdvFileAndSendData;
        }

        /// <summary>
        /// The OnApplicationQuit.
        /// </summary>
        private void OnApplicationQuit()
        {
            localDataService.CheckLocalData -= _advEventUnityManager.CheckAdvFileAndSendData;
        }
    }
}
