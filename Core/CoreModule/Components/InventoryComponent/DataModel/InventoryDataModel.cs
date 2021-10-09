using AppneuronUnity.Core.Models.Abstract;
using System;
using AppneuronUnity.Core.CoreModule.Components.InventoryComponent.DataModel;

namespace AppneuronUnity.Core.CoreModule.Components.InventoryComponent.DataModel
{
    /// <summary>
    /// Defines the <see cref="InventoryDataModel" />.
    /// </summary>
    internal class InventoryDataModel: IEntity
    {

        public string ClientId { get; set; }

        public string ProjectId { get; set; }

        public string CustomerId { get; set; }

        public double MinorMine { get; set; }

        public double ModerateMine { get; set; }

        public double PreciousMine { get; set; }

        public Item[] Items { get; set; }

        public Skill[] Skills { get; set; }

        public TemporaryAbility[] TemporaryAbilities { get; set; }

        private readonly DateTime CreatedAt = DateTime.Now;
    }
}
