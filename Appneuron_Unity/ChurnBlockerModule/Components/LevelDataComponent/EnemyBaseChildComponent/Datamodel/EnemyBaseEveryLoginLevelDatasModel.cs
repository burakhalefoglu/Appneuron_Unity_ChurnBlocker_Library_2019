using Assets.Appneuron.Core.DataModelBase.Abstract;
using System;


namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.Datamodel
{
    [Serializable]
    public class EnemyBaseEveryLoginLevelDatasModel : IEntity
    {
        public string ClientId { get; set; }
        public string ProjectID { get; set; }
        public string CustomerID { get; set; }
        public string Levelname { get; set; }
        public int LevelsDifficultylevel { get; set; }
        public int PlayingTime { get; set; }
        public int AverageScores { get; set; }
        public int IsDead { get; set; }
        public int TotalPowerUsage { get; set; }

        private DateTime DateTime = DateTime.Now;
    }
}
