namespace AppneuronUnity.ProductModules.ChurnBlockerModule.Components.DifficultyComponent.DataModel
{
    using AppneuronUnity.Core.Models.Abstract;
    using System;

    [Serializable]
    internal class ManuelFlowModel: IEntity
    {
        public string ClientId { get; set; } 
        public string ProjectId { get; set; }
        public string CustomerId { get; set; }
        public int DifficultyLevel { get; set; }
        private DateTime dateTime = DateTime.Now;
    }
}
