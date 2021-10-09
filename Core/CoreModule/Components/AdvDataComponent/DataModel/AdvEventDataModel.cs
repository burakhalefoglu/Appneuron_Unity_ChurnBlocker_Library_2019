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
        public string ClientId { get; set; }

        public string ProjectId { get; set; }

        public string CustomerId { get; set; }

        public string LevelName { get; set; }

        public int LevelIndex { get; set; }

        public string AdvType { get; set; }

        public float InMinutes { get; set; }

        public DateTime TrigerdTime { get; set; }
    }
}
