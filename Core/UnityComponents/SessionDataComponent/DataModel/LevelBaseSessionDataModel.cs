namespace AppneuronUnity.Core.UnityComponents.SessionDataComponent.DataModel
{
    using AppneuronUnity.Core.Models.Abstract;
    using System;

    /// <summary>
    /// Defines the <see cref="LevelBaseSessionDataModel" />.
    /// </summary>
    [Serializable]
    internal class LevelBaseSessionDataModel : IEntity
    {

        public string ClientId { get; set; }

        public string ProjectId { get; set; }

        public string CustomerId { get; set; }

        public string levelName { get; set; }

        public int levelIndex { get; set; }

        public int DifficultyLevel { get; set; }

        public float SessionTimeMinute { get; set; }

        public DateTime SessionStartTime { get; set; }

        public DateTime SessionFinishTime { get; set; }
    }
}
