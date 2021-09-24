namespace AppneuronUnity.Core.UnityComponents.ScreenDataComponent.ModelData
{
    using AppneuronUnity.Core.Models.Abstract;
    using System;

    /// <summary>
    /// Defines the <see cref="ClickDataModel" />.
    /// </summary>
    [Serializable]
    internal class ClickDataModel: IEntity
    {
        public string ClientId { get; set; }

        public string ProjectId { get; set; }

        public string CustomerId { get; set; }

        public float StartLocX { get; set; }

        public float StartLocY { get; set; }

        public float FinishLocX { get; set; }

        public float FinishLocY { get; set; }

        public int TabCount { get; set; }

        public int FingerID { get; set; }

        public DateTime CreatedAt { get; set; }

        public string LevelName { get; set; }

        public int LevelIndex { get; set; }
    }
}
