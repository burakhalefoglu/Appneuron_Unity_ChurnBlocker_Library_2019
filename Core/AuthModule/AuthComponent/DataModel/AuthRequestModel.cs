namespace AppneuronUnity.Core.AuthModule.AuthComponent.DataModel
{
    using AppneuronUnity.Core.Models.Abstract;

    /// <summary>
    /// Defines the <see cref="AuthRequestModel" />.
    /// </summary>
    internal class AuthRequestModel
    {
        /// <summary>
        /// Gets or sets the ClientId.
        /// </summary>
        public long ClientId { get; set; }

        /// <summary>
        /// Gets or sets the ProjectId.
        /// </summary>
        public long ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the CustomerId.
        /// </summary>
        public long CustomerId { get; set; }
    }
}
