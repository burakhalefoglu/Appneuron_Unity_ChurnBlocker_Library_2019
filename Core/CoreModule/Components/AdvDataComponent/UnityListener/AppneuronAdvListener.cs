namespace AppneuronUnity.Core.CoreModule.Components.AdvDataComponent.UnityListener
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;
    using Appneuron.Zenject;
using AppneuronUnity.Core.AuthModule.ClientIdComponent.UnityManager;
using AppneuronUnity.Core.CoreModule.Components.AdvDataComponent.UnityManager;
using AppneuronUnity.Core.CoreModule.Services;

    public class AppneuronAdvListener : MonoBehaviour
    {

        private CounterServices counterServices;

        [Inject]
        private IClientIdUnityManager _clientIdUnityManager;

        [Inject]
        private IAdvEventUnityManager _advEventUnityManager;

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
