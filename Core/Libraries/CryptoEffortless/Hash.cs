namespace AppneuronUnity.Core.Libraries.CryptoEffortless
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    ///     The type of hashing function
    /// </summary>
    public enum HashType
    {
        /// <summary>
        /// Defines the MD5.
        /// </summary>
        MD5, // 128 bit
        /// <summary>
        /// Defines the SHA1.
        /// </summary>
        SHA1, // 160 bit
        /// <summary>
        /// Defines the SHA256.
        /// </summary>
        SHA256, // 256 bit
        /// <summary>
        /// Defines the SHA384.
        /// </summary>
        SHA384, // 384 bit
        /// <summary>
        /// Defines the SHA512.
        /// </summary>
        SHA512// 512 bit
    }

    /// <summary>
    /// A hash can help ensure authentication and integrity of data that may be
    ///     modified when transmitted between two parties. The sharedKey is shared by the two
    ///     parties who independently calculate the hash. The data is passed between parties
    ///     together with the hash. The hash will be identical if the data is unmodified.
    ///     Use a sharedKey that is sufficiently long and complex for the application -
    ///     https://www.grc.com/passwords.htm - and share the sharedKey once over a secure channel.
    ///     See http://en.wikipedia.org/wiki/Cryptographic_hash_function for more information.
    /// </summary>
    internal static class Hash
    {
        /// <summary>
        /// Defines the Md5.
        /// </summary>
        private static readonly Lazy<MD5> Md5 = new Lazy<MD5>(MD5.Create);

        /// <summary>
        /// Defines the Sha1.
        /// </summary>
        private static readonly Lazy<SHA1> Sha1 = new Lazy<SHA1>(SHA1.Create);

        /// <summary>
        /// Defines the Sha256.
        /// </summary>
        private static readonly Lazy<SHA256> Sha256 = new Lazy<SHA256>(SHA256.Create);

        /// <summary>
        /// Defines the Sha384.
        /// </summary>
        private static readonly Lazy<SHA384> Sha384 = new Lazy<SHA384>(SHA384.Create);

        /// <summary>
        /// Defines the Sha512.
        /// </summary>
        private static readonly Lazy<SHA512> Sha512 = new Lazy<SHA512>(SHA512.Create);

        /// <summary>
        /// Creates a hash and retuns the byte array of the hash.
        /// </summary>
        /// <param name="hashType">The type of hash algorithm to use.</param>
        /// <param name="data">The data to hash.</param>
        /// <param name="sharedKey">The shared secret key.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public static byte[] Create(HashType hashType, string data, string sharedKey = "")
        {
            switch (hashType)
            {
                case HashType.MD5:
                    return HashData(Md5.Value, data, sharedKey);

                case HashType.SHA1:
                    return HashData(Sha1.Value, data, sharedKey);

                case HashType.SHA256:
                    return HashData(Sha256.Value, data, sharedKey);

                case HashType.SHA384:
                    return HashData(Sha384.Value, data, sharedKey);

                case HashType.SHA512:
                    return HashData(Sha512.Value, data, sharedKey);

                default:
                    throw new ArgumentOutOfRangeException(nameof(hashType));
            }
        }

        /// <summary>
        /// Creates a hash.
        /// </summary>
        /// <param name="hashType">The type of hash algorithm to use.</param>
        /// <param name="data">The data to hash.</param>
        /// <param name="sharedKey">The shared secret key.</param>
        /// <param name="showBytes">The showBytes<see cref="bool"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string Create(HashType hashType, string data, string sharedKey, bool showBytes)
        {
            switch (hashType)
            {
                case HashType.MD5:
                    return HashData(Md5.Value, data, sharedKey, showBytes);

                case HashType.SHA1:
                    return HashData(Sha1.Value, data, sharedKey, showBytes);

                case HashType.SHA256:
                    return HashData(Sha256.Value, data, sharedKey, showBytes);

                case HashType.SHA384:
                    return HashData(Sha384.Value, data, sharedKey, showBytes);

                case HashType.SHA512:
                    return HashData(Sha512.Value, data, sharedKey, showBytes);

                default:
                    throw new ArgumentOutOfRangeException(nameof(hashType));
            }
        }

        /// <summary>
        /// The Verify.
        /// </summary>
        /// <param name="hashType">The hashType<see cref="HashType"/>.</param>
        /// <param name="data">The data<see cref="string"/>.</param>
        /// <param name="sharedKey">The sharedKey<see cref="string"/>.</param>
        /// <param name="showBytes">The showBytes<see cref="bool"/>.</param>
        /// <param name="hash">The hash<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool Verify(HashType hashType, string data, string sharedKey, bool showBytes, string hash)
        {
            return hash == Create(hashType, data, sharedKey, showBytes);
        }

        /// <summary>
        /// The HashData.
        /// </summary>
        /// <param name="hashAlgorithm">The hashAlgorithm<see cref="HashAlgorithm"/>.</param>
        /// <param name="data">The data<see cref="string"/>.</param>
        /// <param name="sharedKey">The sharedKey<see cref="string"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        private static byte[] HashData(HashAlgorithm hashAlgorithm, string data, string sharedKey)
        {
            if (hashAlgorithm == null) throw new ArgumentNullException(nameof(hashAlgorithm));
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (sharedKey == null) throw new ArgumentNullException(nameof(sharedKey));

            var input = Encoding.Unicode.GetBytes(data + sharedKey);
            return hashAlgorithm.ComputeHash(input);
        }

        /// <summary>
        /// The HashData.
        /// </summary>
        /// <param name="hashAlgorithm">The hashAlgorithm<see cref="HashAlgorithm"/>.</param>
        /// <param name="data">The data<see cref="string"/>.</param>
        /// <param name="sharedKey">The sharedKey<see cref="string"/>.</param>
        /// <param name="showBytes">The showBytes<see cref="bool"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private static string HashData(HashAlgorithm hashAlgorithm, string data, string sharedKey, bool showBytes)
        {
            var result = HashData(hashAlgorithm, data, sharedKey);
            return showBytes
                ? Bytes.ByteArrayToHex(result)
                : Convert.ToBase64String(result);
        }
    }
}
