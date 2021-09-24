namespace AppneuronUnity.Core.UnityComponents.AdvDataComponent.UnityListener
{
    using AppneuronUnity.Core.UnityServices;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;
    using AppneuronUnity.Core.UnityComponents.AdvDataComponent.UnityManager;
    using AppneuronUnity.Core.UnityWorkers.ClientIdWorker.UnityManager;
    using Zenject;

    /// <summary>
    /// Defines the <see cref="AppneuronAdvListener" />.
    /// </summary>
    public class AppneuronAdvListener : MonoBehaviour
    {
        /// <summary>
        /// Defines the counterServices.
        /// </summary>
        private CounterServices counterServices;

        [Inject]
        private IClientIdUnityManager _clientIdUnityManager;

        [Inject]
        private IAdvEventUnityManager _advEventUnityManager;

        /// <summary>
        /// The Start.
        /// </summary>
        internal void Start()
        {

            counterServices = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<CounterServices>();
            GameObject gameObject = this.gameObject;
            Button button = gameObject.GetComponent<Button>();

            button.onClick.AddListener(async () =>
            {
                await _advEventUnityManager.SendAdvEventData(this.gameObject.tag,
                 SceneManager.GetActiveScene().name,
                SceneManager.GetActiveScene().buildIndex,
                counterServices.LevelBaseGameTimer,
                _clientIdUnityManager.GetPlayerID());
            });
        }
    }
}
