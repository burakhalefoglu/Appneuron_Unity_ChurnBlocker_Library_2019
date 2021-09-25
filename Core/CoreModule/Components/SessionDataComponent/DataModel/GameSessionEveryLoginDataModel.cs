namespace AppneuronUnity.Core.CoreModule.Components.SessionDataComponent.DataModel
{
    using AppneuronUnity.Core.Models.Abstract;
    using System;

    /// <summary>
    /// Defines the <see cref="GameSessionEveryLoginDataModel" />.
    /// </summary>
    [Serializable]
    internal class GameSessionEveryLoginDataModel : IEntity
    {

        public string ClientId { get; set; }

        public string ProjectId { get; set; }

        public string CustomerId { get; set; }

        public DateTime SessionStartTime { get; set; }

        public DateTime SessionFinishTime { get; set; }

        public float SessionTimeMinute { get; set; }
    }
}
