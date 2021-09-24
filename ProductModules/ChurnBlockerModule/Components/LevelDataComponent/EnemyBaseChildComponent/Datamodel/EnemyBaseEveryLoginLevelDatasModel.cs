namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.Datamodel
{
    using AppneuronUnity.Core.Models.Abstract;
    using System;

    /// <summary>
    /// Defines the <see cref="EnemyBaseEveryLoginLevelDatasModel" />.
    /// </summary>
    [Serializable]
    internal class EnemyBaseEveryLoginLevelDatasModel : IEntity
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
        /// Gets or sets the Levelname.
        /// </summary>
        public string Levelname { get; set; }


        /// <summary>
        /// Gets or sets the levelName.
        /// </summary>
        public int levelIndex { get; set; }

        /// <summary>
        /// Gets or sets the PlayingTime.
        /// </summary>
        public int PlayingTime { get; set; }

        /// <summary>
        /// Gets or sets the AverageScores.
        /// </summary>
        public int AverageScores { get; set; }

        /// <summary>
        /// Gets or sets the IsDead.
        /// </summary>
        public int IsDead { get; set; }

        /// <summary>
        /// Gets or sets the TotalPowerUsage.
        /// </summary>
        public int TotalPowerUsage { get; set; }

        /// <summary>
        /// Defines the DateTime.
        /// </summary>
        private DateTime DateTime = DateTime.Now;
    }
}
