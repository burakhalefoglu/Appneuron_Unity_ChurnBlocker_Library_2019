using Appneuron.Core.Components.HardwareIndormationCompenent.Models;
using Assets.Appneuron.Core.CoreServices.MessageBrockers.Kafka;
using Assets.Appneuron.Core.CoreServices.RestClientServices.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Appneuron.Core.Components.HardwareIndormationCompenent
{
    public class GeneralInformationWorker
    {
        IKafkaMessageBroker kafkaMessageBroker;
        IRestClientServices restClientService;

        public GeneralInformationWorker(IKafkaMessageBroker _kafkaMessageBroker,
            IRestClientServices _restClientService)
        {
            kafkaMessageBroker = _kafkaMessageBroker;
            restClientService = _restClientService;

        }

        public async Task SendGeneralInformation()
        {
            await simplehardwareInformationWorker();
            await LocationWorker();
        }

        private async Task LocationWorker()
        {
            var resultLocation = await restClientService.GetAsync<LocationModel>
                ("https://extreme-ip-lookup.com/json");
            var locationData = resultLocation.Data;
            var resultKafka = await kafkaMessageBroker.SendMessageAsync(locationData);

            if (!resultKafka.Success)
            {
                //TODO: Send Log
            }
        }

        private async Task simplehardwareInformationWorker()
        {
            var hardwareInformation = new HardwareIndormationModel();
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

            var result = await kafkaMessageBroker.SendMessageAsync(hardwareInformation);
            if (!result.Success)
            {
                //TODO: Send Log
            }
        }
    }
}
