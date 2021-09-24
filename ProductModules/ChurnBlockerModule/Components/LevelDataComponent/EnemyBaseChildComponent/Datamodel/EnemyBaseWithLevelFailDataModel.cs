namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.Datamodel
{
    using AppneuronUnity.Core.Models.Abstract;
    using System;

    /// <summary>
    /// Defines the <see cref="EnemyBaseWithLevelFailDataModel" />.
    /// </summary>
    [Serializable]
    internal class EnemyBaseWithLevelFailDataModel : IEntity
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
        /// Gets or sets the DiyingTimeAfterLevelStarting.
        /// </summary>
        public int DiyingTimeAfterLevelStarting { get; set; }

        /// <summary>
        /// Gets or sets the levelName.
        /// </summary>
        public string levelName { get; set; }

        /// <summary>
        /// Gets or sets the levelName.
        /// </summary>
        public int levelIndex { get; set; }

        /// <summary>
        /// Gets or sets the FailLocationX.
        /// </summary>
        public float FailLocationX { get; set; }

        /// <summary>
        /// Gets or sets the FailLocationY.
        /// </summary>
        public float FailLocationY { get; set; }

        /// <summary>
        /// Gets or sets the FailLocationZ.
        /// </summary>
        public float FailLocationZ { get; set; }

        /// <summary>
        /// Defines the DateTime.
        /// </summary>
        internal DateTime DateTime = DateTime.Now;
    }
}
