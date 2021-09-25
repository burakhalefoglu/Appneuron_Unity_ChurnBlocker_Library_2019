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
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the ProjectId.
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the CustomerId.
        /// </summary>
        public string CustomerId { get; set; }
    }
}
