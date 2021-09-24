namespace AppneuronUnity.Core.UnityComponents.HardwareIndormationComponent.UnityController
{
    using UnityEngine;
    using AppneuronUnity.Core.UnityComponents.HardwareIndormationComponent.UnityManager;
    using Appneuron.Zenject;

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
