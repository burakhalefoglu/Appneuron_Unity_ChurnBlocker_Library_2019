namespace AppneuronUnity.Core.CoreModule.Components.LocationComponent.UnityController
{
    using UnityEngine;
    using Appneuron.Zenject;
using AppneuronUnity.Core.CoreModule.Components.LocationComponent.UnityManager;

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
