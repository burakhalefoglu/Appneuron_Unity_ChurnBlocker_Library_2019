﻿namespace AppneuronUnity.Core.UnityWorkers.AuthWorker.DataModel
{
    using System;

    /// <summary>
    /// Defines the <see cref="TokenDataModel" />.
    /// </summary>
    [Serializable]
    internal class TokenDataModel
    {
        /// <summary>
        /// Gets or sets the Token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the Expiration.
        /// </summary>
        public DateTime Expiration { get; set; }
    }
}
