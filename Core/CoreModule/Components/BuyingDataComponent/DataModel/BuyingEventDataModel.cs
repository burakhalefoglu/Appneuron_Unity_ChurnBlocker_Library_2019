namespace AppneuronUnity.Core.CoreModule.Components.BuyingDataComponent.DataModel
{
    using AppneuronUnity.Core.Models.Abstract;
    using System;

    /// <summary>
    /// Defines the <see cref="BuyingEventDataModel" />.
    /// </summary>
    [Serializable]
    internal class BuyingEventDataModel : IEntity
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
        /// Gets or sets the LevelName.
        /// </summary>
        public string LevelName { get; set; }

        /// <summary>
        /// Gets or sets the LevelIndex.
        /// </summary>
        public int LevelIndex { get; set; }

        /// <summary>
        /// Gets or sets the ProductType.
        /// </summary>
        public string ProductType { get; set; }

        /// <summary>
        /// Gets or sets the InWhatMinutes.
        /// </summary>
        public float InWhatMinutes { get; set; }

        /// <summary>
        /// Gets or sets the TrigerdTime.
        /// </summary>
        public DateTime TrigerdTime { get; set; }
    }
}
