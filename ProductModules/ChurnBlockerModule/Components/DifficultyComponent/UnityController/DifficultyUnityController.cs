namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.UnityController
{
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.UnityManager;
    using System.Reflection;
    using UnityEngine;
    using Appneuron.Zenject;

    /// <summary>
    /// Defines the <see cref="DifficultyUnityController" />.
    /// </summary>
    public class DifficultyUnityController : MonoBehaviour
    {
        [Inject]
        internal IDifficultyManager _difficultyManager;

        /// <summary>
        /// The Start.
        /// </summary>
        private async void Start()
        {

            await _difficultyManager.AskDifficulty();
        }
    }
}
