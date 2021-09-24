namespace AppneuronUnity.Core.UnityComponents.ScreenDataComponent.ModelData
{
    using System;

    /// <summary>
    /// Defines the <see cref="ClickDataDto" />.
    /// </summary>
    internal class ClickDataDto
    {
        /// <summary>
        /// Gets or sets the LocX.
        /// </summary>
        public float LocX { get; set; }

        /// <summary>
        /// Gets or sets the LocY.
        /// </summary>
        public float LocY { get; set; }

        /// <summary>
        /// Gets or sets the TabCount.
        /// </summary>
        public int TabCount { get; set; }

        /// <summary>
        /// Gets or sets the FingerID.
        /// </summary>
        public int FingerID { get; set; }

        /// <summary>
        /// Defines the CreatedAt.
        /// </summary>
        public readonly DateTime CreatedAt = DateTime.Now;
    }
}
