namespace AppneuronUnity.Core.CoreModule.Components.ScreenDataComponent.DataModel
{
    using System;

    /// <summary>
    /// Defines the <see cref="SwipeModelDto" />.
    /// </summary>
    internal class SwipeModelDto
    {
        /// <summary>
        /// Gets or sets the SwipeDirection.
        /// </summary>
        public int SwipeDirection { get; set; }

        /// <summary>
        /// Gets or sets the LocX.
        /// </summary>
        public float LocX { get; set; }

        /// <summary>
        /// Gets or sets the LocY.
        /// </summary>
        public float LocY { get; set; }

        /// <summary>
        /// Defines the CreatedAt.
        /// </summary>
        public readonly DateTime CreatedAt = DateTime.Now;
    }
}
