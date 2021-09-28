using Zenject;
using AppneuronUnity.Core.CoreModule.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using AppneuronUnity.Core.CoreModule.Components.ScreenDataComponent.DataManager;
using AppneuronUnity.Core.CoreModule.Components.SessionDataComponent.DataManager;

namespace AppneuronUnity.Core.CoreModule
{
    public class CoreInternalService : MonoBehaviour
    {
        private bool isNewLevel = true;
        private string levelName;
        private int levelIndex;
        private CounterServices counterServices;
        private LocalDataService localDataService;

        [Inject]
        internal IClickDataUnityManager _clickDataUnityManager;

        [Inject]
        internal ISwipeDataUnityManager _swipeDataUnityManager;

        [Inject]
        private ISessionManager _sessionManager;

        private void Start()
        {
            counterServices = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<CounterServices>();
            localDataService = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<LocalDataService>();
        }

        private async void Update()
        {
            await ClickDataManager();
            await SwipeDataManager();
        }

        private async void OnApplicationQuit()
        {
            DateTime gameSessionEveryLoginFinish = counterServices.GameSessionEveryLoginStart
            .AddSeconds(counterServices.TimerForGeneralSession);
            float minutes = counterServices.TimerForGeneralSession / 60;

            await _sessionManager.SendGameSessionEveryLoginData(counterServices.GameSessionEveryLoginStart,
                gameSessionEveryLoginFinish,
                minutes);


        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

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
        private void OnDisable()
        {
            Debug.Log("OnDisable");
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private async Task SwipeDataManager()
        {
            var swipeModelDto = _swipeDataUnityManager.ListenTouchDirection();
            if (swipeModelDto != null)
                await _swipeDataUnityManager.CalculateSwipeDirection(swipeModelDto,
                    SceneManager.GetActiveScene().name,
                    SceneManager.GetActiveScene().buildIndex);
        }

        private async Task ClickDataManager()
        {
            var resultClickDtoList = _clickDataUnityManager.DetectDetaildRawTouchInformation();
            if (resultClickDtoList != null)
            {
                await _clickDataUnityManager.ClickCalculater(resultClickDtoList);
            }
        }
    }
}
