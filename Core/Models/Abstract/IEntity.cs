namespace AppneuronUnity.Core.Models.Abstract
{
    /// <summary>
    /// Defines the <see cref="IEntity" />.
    /// </summary>
    internal interface IEntity
    {
        long ClientId { get; set; }
        long ProjectId { get; set; }
        long CustomerId { get; set; }

    }
}
 