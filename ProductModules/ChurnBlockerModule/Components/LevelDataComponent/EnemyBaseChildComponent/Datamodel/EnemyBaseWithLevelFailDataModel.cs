namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.Datamodel
{
    using AppneuronUnity.Core.Models.Abstract;
    using System;

    [Serializable]
    internal class EnemyBaseWithLevelFailDataModel : IEntity
    {
        public long ClientId { get; set; }

        public long ProjectId { get; set; }

        public long CustomerId { get; set; }

        public int DiyingTimeAfterLevelStarting { get; set; }

        public string levelName { get; set; }

        public int levelIndex { get; set; }

        public float FailLocationX { get; set; }

        public float FailLocationY { get; set; }

        public float FailLocationZ { get; set; }

        internal DateTime dateTime = DateTime.Now;
    }
}
