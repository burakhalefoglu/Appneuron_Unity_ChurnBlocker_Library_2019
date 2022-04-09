namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.DataModel
{
    using AppneuronUnity.Core.Models.Abstract;
    using System;

    [Serializable]
    internal class ManuelFlowModel: IEntity
    {
        public long ClientId { get; set; } 
        public long ProjectId { get; set; }
        public long CustomerId { get; set; }
        public int DifficultyLevel { get; set; }
        private DateTime dateTime = DateTime.Now;
    }
}
