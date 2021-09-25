using AppneuronUnity.Core.Models.Abstract;
using System;

namespace AppneuronUnity.Core.CoreModule.Components.HardwareIndormationComponent.DataModel
{
    /// <summary>
    /// Defines the <see cref="HardwareIndormationModel" />.
    /// </summary>
    internal class HardwareIndormationModel: IEntity
    {
        /// <summary>
        /// Gets or sets the ClientId.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the ProjectID.
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the CustomerID.
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the DeviceModel.
        /// </summary>
        public string DeviceModel { get; set; }

        /// <summary>
        /// Gets or sets the DeviceName.
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// Gets or sets the DeviceType.
        /// </summary>
        public int DeviceType { get; set; }

        /// <summary>
        /// Gets or sets the GraphicsDeviceName.
        /// </summary>
        public string GraphicsDeviceName { get; set; }

        /// <summary>
        /// Gets or sets the GraphicsDeviceType.
        /// </summary>
        public int GraphicsDeviceType { get; set; }

        /// <summary>
        /// Gets or sets the GraphicsDeviceVendor.
        /// </summary>
        public string GraphicsDeviceVendor { get; set; }

        /// <summary>
        /// Gets or sets the GraphicsDeviceVersion.
        /// </summary>
        public string GraphicsDeviceVersion { get; set; }

        /// <summary>
        /// Gets or sets the GraphicsMemorySize.
        /// </summary>
        public int GraphicsMemorySize { get; set; }

        /// <summary>
        /// Gets or sets the OperatingSystem.
        /// </summary>
        public string OperatingSystem { get; set; }

        /// <summary>
        /// Gets or sets the ProcessorCount.
        /// </summary>
        public int ProcessorCount { get; set; }

        /// <summary>
        /// Gets or sets the ProcessorType.
        /// </summary>
        public string ProcessorType { get; set; }

        /// <summary>
        /// Gets or sets the SystemMemorySize.
        /// </summary>
        public int SystemMemorySize { get; set; }

        private readonly DateTime dateTime = DateTime.Now;
    }
}
