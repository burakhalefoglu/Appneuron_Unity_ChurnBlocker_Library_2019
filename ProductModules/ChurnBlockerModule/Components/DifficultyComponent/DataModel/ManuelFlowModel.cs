namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.DataModel
{
    using AppneuronUnity.Core.Models.Abstract;
    using System;

    /// <summary>
    /// Defines the <see cref="ManuelFlowModel" />.
    /// </summary>
    [Serializable]
    internal class ManuelFlowModel: IEntity
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
        /// Gets or sets the DifficultyLevel.
        /// </summary>
        /// 
        public int DifficultyLevel { get; set; }

        /// <summary>
        /// Defines the DateTime.
        /// </summary>
        private DateTime DateTime = DateTime.Now;
    }
}
