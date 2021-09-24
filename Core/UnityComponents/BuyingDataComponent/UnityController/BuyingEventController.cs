namespace AppneuronUnity.Core.UnityComponents.BuyingDataComponent.UnityController
{
    using AppneuronUnity.Core.UnityServices;
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using UnityEngine;
using AppneuronUnity.Core.UnityComponents.BuyingDataComponent.UnityManager;
    using Appneuron.Zenject;

    /// <summary>
    /// Defines the <see cref="BuyingEventController" />.
    /// </summary>
    public class BuyingEventController : MonoBehaviour
    {
        /// <summary>
        /// Defines the localDataService.
        /// </summary>
        private LocalDataService localDataService;

        [Inject]
        private IBuyingEventManager _buyingEventManager;

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
            await _buyingEventManager.CheckAdvFileAndSendData();

            localDataService.CheckLocalData += _buyingEventManager.CheckAdvFileAndSendData;
        }

        /// <summary>
        /// The OnApplicationQuit.
        /// </summary>
        private void OnApplicationQuit()
        {
            localDataService.CheckLocalData -= _buyingEventManager.CheckAdvFileAndSendData;
        }
    }
}
