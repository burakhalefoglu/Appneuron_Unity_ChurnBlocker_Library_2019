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
        /// <summary>
        /// Gets or sets the ClientId.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the ProjectID.
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the CustomerID.
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the MinorMine.
        /// </summary>
        public int MinorMine { get; set; }

        /// <summary>
        /// Gets or sets the ModerateMine.
        /// </summary>
        public int ModerateMine { get; set; }

        /// <summary>
        /// Gets or sets the PreciousMine.
        /// </summary>
        public int PreciousMine { get; set; }

        /// <summary>
        /// Gets or sets the Items.
        /// </summary>
        public Item[] Items { get; set; }

        /// <summary>
        /// Gets or sets the Skills.
        /// </summary>
        public Skill[] Skills { get; set; }

        /// <summary>
        /// Gets or sets the TemporaryAbilities.
        /// </summary>
        public TemporaryAbility[] TemporaryAbilities { get; set; }

        private readonly DateTime CreatedAt = DateTime.Now;
    }
}
