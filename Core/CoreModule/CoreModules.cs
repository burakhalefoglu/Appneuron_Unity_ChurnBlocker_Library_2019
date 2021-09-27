using Appneuron.Zenject;
using AppneuronUnity.Core.CoreModule.Components.AdvDataComponent.UnityManager;
using AppneuronUnity.Core.CoreModule.Components.BuyingDataComponent.UnityManager;
using AppneuronUnity.Core.CoreModule.Components.HardwareIndormationComponent.UnityManager;
using AppneuronUnity.Core.CoreModule.Components.InventoryComponent.UnityManager;
using AppneuronUnity.Core.CoreModule.Components.LocationComponent.UnityManager;
using AppneuronUnity.Core.CoreModule.Components.ScreenDataComponent.UnityManager;
using AppneuronUnity.Core.CoreModule.Components.SessionDataComponent.UnityManager;
using AppneuronUnity.Core.CoreModule.Services;
using System;
using System.Threading.Tasks;
using UnityEngine;
using static AppneuronUnity.Core.AuthModule.ClientIdComponent.Helper.ClientIdConfigServices;

namespace AppneuronUnity.Core.CoreModule
{
    public class CoreModule: MonoBehaviour
    {

        private LocalDataService localDataService;

        [Inject]
        private IAdvEventUnityManager _advEventUnityManager;

        [Inject]
        private IBuyingEventManager _buyingEventManager;

        [Inject]
        private IHardwareIndormationUnityManager _hardwareIndormationUnityManager;

        [Inject]
        private IInventoryUnityManager _inventoryUnityManager;

        [Inject]
        private ILocationUnityManager _locationUnityManager;

        [Inject]
        private IClickDataUnityManager _clickDataUnityManager;

        [Inject]
        private ISwipeDataUnityManager _swipeDataUnityManager;

        [Inject]
        private ISessionManager _sessionManager;

        private void Awake()
        {
            CreateFileDirectories();
        }

        private async void Start()
        {
            localDataService = GameObject.FindGameObjectWithTag("Appneuron").GetComponent<LocalDataService>();
            await LateStart(3);

            //TODO: after token get or is valid in location
            await _hardwareIndormationUnityManager.SendData();
            await _locationUnityManager.SendMessage();


        }

        private async Task LateStart(float waitTime)
        {
            await Task.Delay(TimeSpan.FromSeconds(waitTime));
            await _advEventUnityManager.CheckAdvFileAndSendData();
            await _buyingEventManager.CheckAdvFileAndSendData();
            await _inventoryUnityManager.CheckFileExistAndSend();
            await _clickDataUnityManager.CheckAdvFileAndSendData();
            await _swipeDataUnityManager.CheckAdvFileAndSendData();
            await _sessionManager.CheckGameSessionEveryLoginDataAndSend();
            await _sessionManager.CheckLevelBaseSessionDataAndSend();

            localDataService.CheckLocalData += _advEventUnityManager.CheckAdvFileAndSendData;
            localDataService.CheckLocalData += _buyingEventManager.CheckAdvFileAndSendData;
            localDataService.CheckLocalData += _inventoryUnityManager.CheckFileExistAndSend;
            localDataService.CheckLocalData += _clickDataUnityManager.CheckAdvFileAndSendData;
            localDataService.CheckLocalData += _swipeDataUnityManager.CheckAdvFileAndSendData;
            localDataService.CheckLocalData += _sessionManager.CheckGameSessionEveryLoginDataAndSend;
            localDataService.CheckLocalData += _sessionManager.CheckLevelBaseSessionDataAndSend;
        }

        private void OnApplicationQuit()
        {
            localDataService.CheckLocalData -= _advEventUnityManager.CheckAdvFileAndSendData;
            localDataService.CheckLocalData -= _buyingEventManager.CheckAdvFileAndSendData;
            localDataService.CheckLocalData -= _inventoryUnityManager.CheckFileExistAndSend;
            localDataService.CheckLocalData -= _clickDataUnityManager.CheckAdvFileAndSendData;
            localDataService.CheckLocalData -= _swipeDataUnityManager.CheckAdvFileAndSendData;
            localDataService.CheckLocalData -= _sessionManager.CheckGameSessionEveryLoginDataAndSend;
            localDataService.CheckLocalData -= _sessionManager.CheckLevelBaseSessionDataAndSend;
        }
    }
}
