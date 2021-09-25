namespace AppneuronUnity.Core.CoreModule.Components.BuyingDataComponent.UnityListener
{
    using System.Reflection;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;
    using Appneuron.Zenject;
using AppneuronUnity.Core.AuthModule.ClientIdComponent.UnityManager;
using AppneuronUnity.Core.CoreModule.Components.BuyingDataComponent.UnityManager;
using AppneuronUnity.Core.CoreModule.Services;

    /// <summary>
    /// Defines the <see cref="BuyingEventListener" />.
    /// </summary>
    public class BuyingEventListener : MonoBehaviour
    {
        /// <summary>
        /// Defines the counterServices.
        /// </summary>
        private CounterServices counterServices;

        [Inject]
        private IClientIdUnityManager _clientIdUnityManager;

        [Inject]
        private IBuyingEventManager _buyingEventManager;

        /// <summary>
        /// The Start.
        /// </summary>
        private void Start()
        {
            counterServices = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<CounterServices>();
            Button button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(async () =>
            {
                await _buyingEventManager.SendAdvEventData(gameObject.tag,
                 SceneManager.GetActiveScene().name,
                 SceneManager.GetActiveScene().buildIndex,
                    counterServices.LevelBaseGameTimer,
                    _clientIdUnityManager.GetPlayerID());
            });
        }
    }
}
