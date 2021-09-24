namespace AppneuronUnity.Core.UnityComponents.ScreenDataComponent.UnityController
{
    using AppneuronUnity.Core.UnityServices;
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using UnityEngine;
using AppneuronUnity.Core.UnityComponents.ScreenDataComponent.UnityManager;
    using Zenject;

    /// <summary>
    /// Defines the <see cref="SwipeDataUnityController" />.
    /// </summary>
    public class SwipeDataUnityController : MonoBehaviour
    {
        /// <summary>
        /// Defines the localDataService.
        /// </summary>
        private LocalDataService localDataService;

        [Inject]
        private ISwipeDataUnityManager _swipeDataUnityManager;

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
            await _swipeDataUnityManager.CheckAdvFileAndSendData();
            localDataService.CheckLocalData += _swipeDataUnityManager.CheckAdvFileAndSendData;
        }

        /// <summary>
        /// The OnApplicationQuit.
        /// </summary>
        private void OnApplicationQuit()
        {
            localDataService.CheckLocalData -= _swipeDataUnityManager.CheckAdvFileAndSendData;
        }
    }
}
