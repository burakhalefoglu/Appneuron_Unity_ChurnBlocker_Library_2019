namespace AppneuronUnity.Core.AuthModule.AuthComponent.UnityController
{
    using System;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;
    using Appneuron.Zenject;
using AppneuronUnity.Core.AuthModule.AuthComponent.UnityManager;

    /// <summary>
    /// Defines the <see cref="AuthUnityController" />.
    /// </summary>
    public class AuthUnityController : MonoBehaviour
    {
        [Inject]
        private IAuthUnityManager _authUnityManager;

        /// <summary>
        /// The Start.
        /// </summary>
        private async void Start()
        {
            await LateStart(3);
        }
        private async Task LateStart(float waitTime)
        {
            await Task.Delay(TimeSpan.FromSeconds(waitTime));
            await _authUnityManager.Login();
        }
    }
}
