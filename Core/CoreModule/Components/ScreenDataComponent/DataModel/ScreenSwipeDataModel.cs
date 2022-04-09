namespace AppneuronUnity.Core.CoreModule.Components.ScreenDataComponent.DataModel
{
    using AppneuronUnity.Core.Models.Abstract;
    using System;

    [Serializable]
    internal class ScreenSwipeDataModel: IEntity
    {

        public long ClientId { get; set; }

        public long ProjectId { get; set; }

        public long CustomerId { get; set; }

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
