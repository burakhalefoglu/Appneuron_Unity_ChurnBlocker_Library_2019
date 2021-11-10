namespace AppneuronUnity.Core.CoreModule.Components.HardwareIndormationComponent.DataManager
{
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System.Threading.Tasks;
    using UnityEngine;
    using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
    using AppneuronUnity.Core.CoreModule.Components.HardwareIndormationComponent.DataModel;
    using Zenject;
    using AppneuronUnity.Core.AuthModule.ClientIdComponent.DataManager;

    internal class HardwareIndormationUnityManager : IHardwareIndormationUnityManager
    {
        private readonly IDataCreationClient _dataCreationClient;

        private readonly IClientIdUnityManager _clientIdUnityManager;

        [Inject]
        private readonly CoreHelper coreHelper;

        public HardwareIndormationUnityManager(IDataCreationClient dataCreationClient,
            IClientIdUnityManager clientIdUnityManager)
        {
            _dataCreationClient = dataCreationClient;
            _clientIdUnityManager = clientIdUnityManager;
        }

        public async Task SendData()
        {
            var hardwareInformation = new HardwareInformationModel();
            hardwareInformation.ClientId = _clientIdUnityManager.GetPlayerID();
            hardwareInformation.ProjectId = coreHelper.GetProjectInfo().ProjectID;
            hardwareInformation.CustomerId = coreHelper.GetProjectInfo().CustomerID;
            hardwareInformation.DeviceModel = SystemInfo.deviceModel;
            hardwareInformation.DeviceName = SystemInfo.deviceName;
            hardwareInformation.DeviceType = ((int)SystemInfo.deviceType);
            hardwareInformation.GraphicsDeviceName = SystemInfo.graphicsDeviceName;
            hardwareInformation.GraphicsDeviceType = ((int)SystemInfo.graphicsDeviceType);
            hardwareInformation.GraphicsDeviceVendor = SystemInfo.graphicsDeviceVendor;
            hardwareInformation.GraphicsDeviceVersion = SystemInfo.graphicsDeviceVersion;
            hardwareInformation.GraphicsMemorySize = SystemInfo.graphicsMemorySize;
            hardwareInformation.OperatingSystem = SystemInfo.operatingSystem;
            hardwareInformation.ProcessorCount = SystemInfo.processorCount;
            hardwareInformation.ProcessorType = SystemInfo.processorType;
            hardwareInformation.SystemMemorySize = SystemInfo.systemMemorySize;

            await _dataCreationClient.PushAsync(_clientIdUnityManager.GetPlayerID(),
            coreHelper.GetProjectInfo().ProjectID,
            hardwareInformation, (result) =>
            {
                if (!result)
                {
                    //TODO: Send Log

                }
            });
        }
    }
}
