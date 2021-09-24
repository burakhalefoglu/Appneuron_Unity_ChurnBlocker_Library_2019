namespace AppneuronUnity.Core.Models.Concrete.Jwt
{
    /// <summary>
    /// Defines the <see cref="TokenSingletonModel" />.
    /// </summary>
    internal class TokenSingletonModel
    {
        /// <summary>
        /// Defines the instance.
        /// </summary>
        private static readonly TokenSingletonModel instance = new TokenSingletonModel();

        /// <summary>
        /// Initializes static members of the <see cref="TokenSingletonModel"/> class.
        /// </summary>
        static TokenSingletonModel()
        {
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="TokenSingletonModel"/> class from being created.
        /// </summary>
        private TokenSingletonModel()
        {
        }

        /// <summary>
        /// Gets the Instance.
        /// </summary>
        public static TokenSingletonModel Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Gets or sets the Token.
        /// </summary>
        public string Token { get; set; }
    }
}
