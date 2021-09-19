
using Assets.Appneuron.Core.DataModelBase.Abstract;
using System;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.Components.LevelDataComponent.EnemyBaseChildComponent.Datamodel
{

    [Serializable]
    public class EnemyBaseWithLevelFailDataModel : IEntity
    {
        public string ClientId { get; set; }
        public string ProjectID { get; set; } 
        public string CustomerID { get; set; } 
        public int DiyingTimeAfterLevelStarting { get; set; }
        public string levelName { get; set; }
        public float FailLocationX { get; set; }
        public float FailLocationY { get; set; }
        public float FailLocationZ { get; set; }

        DateTime DateTime = DateTime.Now;

    }
}
