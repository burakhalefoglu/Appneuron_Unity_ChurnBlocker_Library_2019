namespace AppneuronUnity.Core.CoreModule.Components.SessionDataComponent.DataModel
{
    using AppneuronUnity.Core.Models.Abstract;
    using System;

    /// <summary>
    /// Defines the <see cref="GameSessionEveryLoginDataModel" />.
    /// </summary>
    [Serializable]
    internal class GameSessionEveryLoginDataModel : IEntity
    {

        public GameSessionEveryLoginDataModel()
        {
            CreatedAt = DateTime.Now;
            Status = true;
        }

        public long ClientId { get; set; }

        public long ProjectId { get; set; }

        public long CustomerId { get; set; }

        public DateTime SessionStartTime { get; set; }

        public DateTime SessionFinishTime { get; set; }

        public float SessionTime { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool Status { get; set; }

    }
}
