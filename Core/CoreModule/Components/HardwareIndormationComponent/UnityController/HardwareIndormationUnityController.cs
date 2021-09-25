namespace AppneuronUnity.Core.CoreModule.Components.HardwareIndormationComponent.UnityController
{
    using UnityEngine;
    using Appneuron.Zenject;
using AppneuronUnity.Core.CoreModule.Components.HardwareIndormationComponent.UnityManager;

    /// <summary>
    /// Defines the <see cref="HardwareIndormationUnityController" />.
    /// </summary>
    public class HardwareIndormationUnityController : MonoBehaviour
    {
        [Inject]
        private IHardwareIndormationUnityManager _hardwareIndormationUnityManager;

        /// <summary>
        /// The Start.
        /// </summary>
        private async void Start()
        {
            await _hardwareIndormationUnityManager.SendData();
        }
    }
}
