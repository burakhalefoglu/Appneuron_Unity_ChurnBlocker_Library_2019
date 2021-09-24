namespace AppneuronUnity.Core.Models.Abstract
{
    /// <summary>
    /// Defines the <see cref="IEntity" />.
    /// </summary>
    internal interface IEntity
    {
        string ClientId { get; set; }
        string ProjectId { get; set; }
        string CustomerId { get; set; }

    }
}
 