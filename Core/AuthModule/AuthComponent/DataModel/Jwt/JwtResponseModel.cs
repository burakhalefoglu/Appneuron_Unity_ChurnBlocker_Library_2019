namespace AppneuronUnity.Core.AuthModule.AuthComponent.DataModel.Jwt
{
using AppneuronUnity.Core.AuthModule.AuthComponent.DataModel;

    /// <summary>
    /// Defines the <see cref="JwtResponseModel" />.
    /// </summary>
    internal class JwtResponseModel
    {
        /// <summary>
        /// Gets or sets the Data.
        /// </summary>
        public TokenDataModel Data { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Success.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the Message.
        /// </summary>
        public string Message { get; set; }
    }
}
