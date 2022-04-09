namespace AppneuronUnity.Core.CoreModule.Components.SessionDataComponent.DataModel
{
    using Models.Abstract;
    using System;

    [Serializable]
    internal class LevelBaseSessionDataModel : IEntity
    {

        public long ClientId { get; set; }

        public long ProjectId { get; set; }

        public long CustomerId { get; set; }

        public string LevelName { get; set; }

        public int LevelIndex { get; set; }

        public float SessionTimeMinute { get; set; }

        public DateTime SessionStartTime { get; set; }

        public DateTime SessionFinishTime { get; set; }
    }
}
