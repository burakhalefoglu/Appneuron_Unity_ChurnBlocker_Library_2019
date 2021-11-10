using AppneuronUnity.Core.Models.Abstract;
using System;

namespace AppneuronUnity.Core.CoreModule.Components.HardwareIndormationComponent.DataModel
{

    internal class HardwareInformationModel : IEntity
    {

        public string ClientId { get; set; }

        public string ProjectId { get; set; }

        public string CustomerId { get; set; }

        public string DeviceModel { get; set; }

        public string DeviceName { get; set; }

        public int DeviceType { get; set; }

        public string GraphicsDeviceName { get; set; }

        public int GraphicsDeviceType { get; set; }

        public string GraphicsDeviceVendor { get; set; }

        public string GraphicsDeviceVersion { get; set; }

        public int GraphicsMemorySize { get; set; }

        public string OperatingSystem { get; set; }

        public int ProcessorCount { get; set; }

        public string ProcessorType { get; set; }

        public int SystemMemorySize { get; set; }

        private readonly DateTime dateTime = DateTime.Now;
    }
}
