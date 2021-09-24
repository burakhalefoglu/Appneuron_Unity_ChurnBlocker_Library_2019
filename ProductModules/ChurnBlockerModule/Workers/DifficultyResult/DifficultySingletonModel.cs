namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Workers.DifficultyResult
{
    /// <summary>
    /// Defines the <see cref="DifficultySingletonModel" />.
    /// </summary>
    public sealed class DifficultySingletonModel
    {
        /// <summary>
        /// Defines the instance.
        /// </summary>
        private static readonly DifficultySingletonModel instance = new DifficultySingletonModel();

        /// <summary>
        /// Initializes static members of the <see cref="DifficultySingletonModel"/> class.
        /// </summary>
        static DifficultySingletonModel()
        {
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="DifficultySingletonModel"/> class from being created.
        /// </summary>
        private DifficultySingletonModel()
        {
        }

        /// <summary>
        /// Gets the Instance.
        /// </summary>
        public static DifficultySingletonModel Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Gets or sets the CurrentDifficultyLevel.
        /// </summary>
        public int CurrentDifficultyLevel { get; set; }

        /// <summary>
        /// Gets or sets the CenterOfDifficultyLevel.
        /// </summary>
        public int CenterOfDifficultyLevel { get; set; }

        /// <summary>
        /// Gets or sets the MinOfDifficultyLevelRange.
        /// </summary>
        public int MinOfDifficultyLevelRange { get; set; }

        /// <summary>
        /// Gets or sets the MaxOfDifficultyLevelRange.
        /// </summary>
        public int MaxOfDifficultyLevelRange { get; set; }

        /// <summary>
        /// Gets or sets the RangeCount.
        /// </summary>
        public int RangeCount { get; set; }
    }
}
