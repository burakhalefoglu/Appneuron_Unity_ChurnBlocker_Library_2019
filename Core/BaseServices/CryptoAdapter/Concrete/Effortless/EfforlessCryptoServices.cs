namespace AppneuronUnity.Core.Adapters.CryptoAdapter.Concrete.Effortless
{
    using AppneuronUnity.Core.Libraries.CryptoEffortless;
    using System;
    using System.Linq;
using AppneuronUnity.Core.Adapters.CryptoAdapter.Absrtact;

    /// <summary>
    /// Defines the <see cref="EfforlessCryptoServices" />.
    /// </summary>
    internal class EfforlessCryptoServices : ICryptoServices
    {
        /// <summary>
        /// The DeCrypto.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string DeCrypto(string value)
        {
            return null;
        }

        /// <summary>
        /// The EnCrypto.
        /// </summary>
        /// <param name="longString">The longString<see cref="int"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string EnCrypto(int longString)
        {
            return null;
        }

        /// <summary>
        /// The GetRandomHexNumber.
        /// </summary>
        /// <param name="digits">The digits<see cref="int"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetRandomHexNumber(int digits)
        {
            Random random = new Random();
            byte[] buffer = new byte[digits / 2];
            random.NextBytes(buffer);
            string result = string.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
                return result;
            return result + random.Next(16).ToString("X");
        }

        /// <summary>
        /// The GenerateStringName.
        /// </summary>
        /// <param name="longString">The longString<see cref="int"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GenerateStringName(int longString)
        {
            string salt = StringHash.CreateSalt(longString);
            return salt;
        }
    }
}
