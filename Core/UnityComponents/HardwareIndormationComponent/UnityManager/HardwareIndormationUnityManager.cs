namespace AppneuronUnity.Core.UnityComponents.HardwareIndormationComponent.UnityManager
{
    using AppneuronUnity.Core.CoreServices.MessageBrokers;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System.Threading.Tasks;
    using UnityEngine;
using AppneuronUnity.Core.UnityComponents.HardwareIndormationComponent.DataModel;
using AppneuronUnity.Core.UnityComponents.HardwareIndormationComponent.UnityManager;
using AppneuronUnity.Core.UnityWorkers.ClientIdWorker.UnityManager;

    /// <summary>
    /// Defines the <see cref="HardwareIndormationUnityManager" />.
    /// </summary>
    internal class HardwareIndormationUnityManager : IHardwareIndormationUnityManager
    {
        /// <summary>
        /// Defines the _kafkaMessageBroker.
        /// </summary>
        private readonly IMessageBrokerService _kafkaMessageBroker;

        /// <summary>
        /// Defines the _kafkaMessageBroker.
        /// </summary>
        private readonly IClientIdUnityManager _clientIdUnityManager;


        /// <summary>
        /// Initializes a new instance of the <see cref="HardwareIndormationUnityManager"/> class.
        /// </summary>
        /// <param name="kafkaMessageBroker">The kafkaMessageBroker<see cref="IMessageBrokerService"/>.</param>
        /// <param name="clientIdUnityManager">The kafkaMessageBroker<see cref="IClientIdUnityManager"/>.</param>
        public HardwareIndormationUnityManager(IMessageBrokerService kafkaMessageBroker,
            IClientIdUnityManager clientIdUnityManager )
        {
            _kafkaMessageBroker = kafkaMessageBroker;
            _clientIdUnityManager = clientIdUnityManager;
        }

        /// <summary>
        /// The SendData.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SendData()
        {
            var hardwareInformation = new HardwareIndormationModel();
            hardwareInformation.ClientId = _clientIdUnityManager.GetPlayerID();
            hardwareInformation.ProjectId = ChurnBlockerSingletonConfigs.Instance.GetProjectID();
            hardwareInformation.CustomerId = ChurnBlockerSingletonConfigs.Instance.GetCustomerID();
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

            var result = await _kafkaMessageBroker.SendMessageAsync(hardwareInformation);
            if (!result.Success)
            {
                //TODO: Send Log
            }
        }
    }
}
