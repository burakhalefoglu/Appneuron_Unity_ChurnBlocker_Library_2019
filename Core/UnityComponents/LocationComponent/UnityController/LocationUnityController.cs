namespace AppneuronUnity.Core.UnityComponents.LocationComponent.UnityController
{
    using UnityEngine;
using AppneuronUnity.Core.UnityComponents.LocationComponent.UnityManager;
    using Appneuron.Zenject;

    /// <summary>
    /// Defines the <see cref="LocationUnityController" />.
    /// </summary>
    public class LocationUnityController : MonoBehaviour
    {
        [Inject]
        private ILocationUnityManager _locationUnityManager;

        /// <summary>
        /// The Start.
        /// </summary>
        private async void Start()
        {

            await _locationUnityManager.SendMessage();
        }
    }
}
