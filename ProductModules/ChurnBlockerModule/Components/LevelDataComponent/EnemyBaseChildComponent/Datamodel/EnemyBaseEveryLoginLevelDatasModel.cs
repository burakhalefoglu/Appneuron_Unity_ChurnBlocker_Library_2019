namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.Datamodel
{
    using AppneuronUnity.Core.Models.Abstract;
    using System;

    [Serializable]
    internal class EnemyBaseEveryLoginLevelDatasModel : IEntity
    {
        public long ClientId { get; set; }

        public long ProjectId { get; set; }

        public long CustomerId { get; set; }

        public string Levelname { get; set; }

        public int levelIndex { get; set; }

        public int PlayingTime { get; set; }

        public int AverageScores { get; set; }

        public int IsDead { get; set; }

        public int TotalPowerUsage { get; set; }

        private DateTime dateTime = DateTime.Now;
    }
}
