namespace AppneuronUnity.Core.UnityComponents.ScreenDataComponent.ModelData
{
    using AppneuronUnity.Core.Models.Abstract;
    using System;

    /// <summary>
    /// Defines the <see cref="SwipeDataModel" />.
    /// </summary>
    [Serializable]
    internal class SwipeDataModel: IEntity
    {

        public string ClientId { get; set; }

        public string ProjectId { get; set; }

        public string CustomerId { get; set; }

        public int SwipeDirection { get; set; }

        public float StartLocX { get; set; }

        public float StartLocY { get; set; }

        public float FinishLocX { get; set; }

        public float FinishLocY { get; set; }

        public DateTime CreatedAt { get; set; }

        public string LevelName { get; set; }

        public int LevelIndex { get; set; }
    }
}
