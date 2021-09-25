namespace AppneuronUnity.Core.CoreModule.Components.AdvDataComponent.DataModel
{
    using AppneuronUnity.Core.Models.Abstract;
    using System;

    /// <summary>
    /// Defines the <see cref="AdvEventDataModel" />.
    /// </summary>
    [Serializable]
    internal class AdvEventDataModel : IEntity
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
        /// Gets or sets the TrigersInlevelName.
        /// </summary>
        public string LevelName { get; set; }

        /// <summary>
        /// Gets or sets the TrigersInlevelName.
        /// </summary>
        public int LevelIndex { get; set; }

        /// <summary>
        /// Gets or sets the AdvType.
        /// </summary>
        public string AdvType { get; set; }

        /// <summary>
        /// Gets or sets the InMinutes.
        /// </summary>
        public float InMinutes { get; set; }

        /// <summary>
        /// Gets or sets the TrigerdTime.
        /// </summary>
        public DateTime TrigerdTime { get; set; }
    }
}
