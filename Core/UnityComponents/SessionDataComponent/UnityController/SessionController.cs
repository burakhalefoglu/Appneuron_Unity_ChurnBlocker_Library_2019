namespace AppneuronUnity.Core.UnityComponents.SessionDataComponent.UnityController
{
    using AppneuronUnity.Core.UnityServices;
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.SceneManagement;
using AppneuronUnity.Core.UnityComponents.SessionDataComponent.UnityManager;
    using Appneuron.Zenject;

    /// <summary>
    /// Defines the <see cref="SessionController" />.
    /// </summary>
    public class SessionController : MonoBehaviour
    {
        /// <summary>
        /// Defines the isNewLevel.
        /// </summary>
        private bool isNewLevel = true;

        /// <summary>
        /// Defines the levelName.
        /// </summary>
        private string levelName;

        /// <summary>
        /// Defines the levelName.
        /// </summary>
        private int levelIndex;

        /// <summary>
        /// Defines the counterServices.
        /// </summary>
        private CounterServices counterServices;

        /// <summary>
        /// Defines the localDataService.
        /// </summary>
        private LocalDataService localDataService;

        [Inject]
        private ISessionManager _sessionManager;

        /// <summary>
        /// The Start.
        /// </summary>
        private async void Start()
        {

            counterServices = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<CounterServices>();
            localDataService = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<LocalDataService>();
            await LateStart(3);
        }

        /// <summary>
        /// The LateStart.
        /// </summary>
        /// <param name="waitTime">The waitTime<see cref="float"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task LateStart(float waitTime)
        {
            await Task.Delay(TimeSpan.FromSeconds(waitTime));
            await _sessionManager.CheckGameSessionEveryLoginDataAndSend();
            await _sessionManager.CheckLevelBaseSessionDataAndSend();
            localDataService.CheckLocalData += _sessionManager.CheckGameSessionEveryLoginDataAndSend;
            localDataService.CheckLocalData += _sessionManager.CheckLevelBaseSessionDataAndSend;
        }

        /// <summary>
        /// The OnApplicationQuit.
        /// </summary>
        private async void OnApplicationQuit()
        {
            DateTime gameSessionEveryLoginFinish = counterServices.GameSessionEveryLoginStart
            .AddSeconds(counterServices.TimerForGeneralSession);
            float minutes = counterServices.TimerForGeneralSession / 60;

            await _sessionManager.SendGameSessionEveryLoginData(counterServices.GameSessionEveryLoginStart,
                gameSessionEveryLoginFinish,
                minutes);

            localDataService.CheckLocalData -= _sessionManager.CheckGameSessionEveryLoginDataAndSend;
            localDataService.CheckLocalData -= _sessionManager.CheckLevelBaseSessionDataAndSend;
        }

        /// <summary>
        /// The OnEnable.
        /// </summary>
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        /// <summary>
        /// The OnSceneLoaded.
        /// </summary>
        /// <param name="scene">The scene<see cref="Scene"/>.</param>
        /// <param name="mode">The mode<see cref="LoadSceneMode"/>.</param>
        private async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (isNewLevel)
            {
                levelName = scene.name;
                levelIndex = scene.buildIndex;
                isNewLevel = false;
            }
            else
            {
                await _sessionManager.SendLevelbaseSessionData(counterServices.LevelBaseGameTimer,
                    levelName,
                    levelIndex,
                    counterServices.LevelBaseGameSessionStart);
                levelName = scene.name;
                levelIndex = scene.buildIndex;
            }
        }

        /// <summary>
        /// The OnDisable.
        /// </summary>
        private void OnDisable()
        {
            Debug.Log("OnDisable");
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
