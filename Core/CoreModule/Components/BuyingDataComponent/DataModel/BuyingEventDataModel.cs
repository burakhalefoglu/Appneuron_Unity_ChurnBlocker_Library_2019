namespace AppneuronUnity.Core.CoreModule.Components.BuyingDataComponent.DataModel
{
    using AppneuronUnity.Core.Models.Abstract;
    using System;


    [Serializable]
    internal class BuyingEventDataModel : IEntity
    {
        public long ClientId { get; set; }

        public long ProjectId { get; set; }

        public long CustomerId { get; set; }

        public string LevelName { get; set; }

        public int LevelIndex { get; set; }

        public string ProductType { get; set; }

        public float InWhatMinutes { get; set; }

        public DateTime TrigerdTime { get; set; }
    }
}
