namespace AppneuronUnity.Core.UnityWorkers.ClientIdWorker.UnityController
{
    using System.Reflection;
    using UnityEngine;
    using static AppneuronUnity.Core.UnityWorkers.ClientIdWorker.Helper.ClientIdConfigServices;
using AppneuronUnity.Core.UnityWorkers.ClientIdWorker.UnityManager;
    using Zenject;

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
