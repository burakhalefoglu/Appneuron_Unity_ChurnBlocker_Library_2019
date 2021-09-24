namespace AppneuronUnity.Core.CoreServices.CryptoServices.Absrtact
{
    /// <summary>
    /// Defines the <see cref="ICryptoServices" />.
    /// </summary>
    internal interface ICryptoServices
    {
        /// <summary>
        /// The GetRandomHexNumber.
        /// </summary>
        /// <param name="digits">The digits<see cref="int"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        string GetRandomHexNumber(int digits);

        /// <summary>
        /// The EnCrypto.
        /// </summary>
        /// <param name="longString">The longString<see cref="int"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        string EnCrypto(int longString);

        /// <summary>
        /// The DeCrypto.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        string DeCrypto(string value);

        /// <summary>
        /// The GenerateStringName.
        /// </summary>
        /// <param name="longString">The longString<see cref="int"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        string GenerateStringName(int longString);
    }
}
