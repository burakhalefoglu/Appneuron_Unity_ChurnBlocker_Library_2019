namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Workers.DifficultyResult.UnityController
{
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Workers.DifficultyResult.Clients;
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="DifficultyUnityController" />.
    /// </summary>
    public class DifficultyUnityController : MonoBehaviour
    {
        /// <summary>
        /// The Start.
        /// </summary>
        private async void Start()
        {
            await DifficultyClient.ListenServerManager();
        }
    }
}
