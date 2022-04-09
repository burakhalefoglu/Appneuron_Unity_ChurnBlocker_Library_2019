namespace AppneuronUnity.Core.CoreModule.Components.AdvDataComponent.DataModel
{
    using AppneuronUnity.Core.Models.Abstract;
    using System;

    /// <summary>
    /// Defines the <see cref="AdvEventDataModel" />.
    /// </summary>
    [Serializable]
    internal class AdvEventDataModel : IEntity
    {
        public long ClientId { get; set; }

        public long ProjectId { get; set; }

        public long CustomerId { get; set; }

        public string LevelName { get; set; }

        public int LevelIndex { get; set; }

        public string AdvType { get; set; }

        public float InMinutes { get; set; }

        public DateTime TrigerdTime { get; set; }
    }
}
