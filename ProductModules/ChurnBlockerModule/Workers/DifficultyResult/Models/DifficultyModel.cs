namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Workers.DifficultyResult.Models
{
    using AppneuronUnity.Core.Models.Abstract;
    using System;

    /// <summary>
    /// Defines the <see cref="DifficultyModel" />.
    /// </summary>
    [Serializable]
    public class DifficultyModel
    {
        /// <summary>
        /// Gets or sets the CenterOfDifficultyLevel.
        /// </summary>
        public int CenterOfDifficultyLevel { get; set; }

        /// <summary>
        /// Gets or sets the RangeCount.
        /// </summary>
        public int RangeCount { get; set; }
    }
}
