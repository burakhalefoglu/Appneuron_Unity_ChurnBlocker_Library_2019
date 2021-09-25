namespace AppneuronUnity.Core.AuthModule.ClientIdComponent.UnityController
{
    using System.Reflection;
    using UnityEngine;
    using static AppneuronUnity.Core.AuthModule.ClientIdComponent.Helper.ClientIdConfigServices;
    using Appneuron.Zenject;
using AppneuronUnity.Core.AuthModule.ClientIdComponent.UnityManager;

    /// <summary>
    /// Defines the <see cref="ClientIdUnityController" />.
    /// </summary>
    public class ClientIdUnityController : MonoBehaviour
    {
        [Inject]
        private IClientIdUnityManager _clientIdUnityManager;

        /// <summary>
        /// The Awake.
        /// </summary>
        private async void Awake()
        {
            CreateFileDirectories();
            await _clientIdUnityManager.SaveIdOnLocalStorage();
        }
    }
}
