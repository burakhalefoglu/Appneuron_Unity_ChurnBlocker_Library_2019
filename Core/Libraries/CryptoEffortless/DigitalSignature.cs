namespace AppneuronUnity.Core.Libraries.CryptoEffortless
{
    using System.Security.Cryptography;

    /// <summary>
    /// Digital sign and verify a hash using RSACryptoServiceProvider.
    /// </summary>
    internal class DigitalSignature
    {
        /// <summary>
        /// Defines the _publicKey.
        /// </summary>
        private RSAParameters _publicKey;

        /// <summary>
        /// Defines the _privateKey.
        /// </summary>
        private RSAParameters _privateKey;

        /// <summary>
        /// Defines the _keySize.
        /// </summary>
        private readonly int _keySize;

        /// <summary>
        /// Defines the _hashAlgorithm.
        /// </summary>
        private readonly string _hashAlgorithm;

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalSignature"/> class.
        /// </summary>
        public DigitalSignature()
        {
            _keySize = 2048;
            _hashAlgorithm = "SHA256";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalSignature"/> class.
        /// </summary>
        /// <param name="keySize">The keySize<see cref="int"/>.</param>
        /// <param name="hashAlgorithm">Specify the hash algorithm for RSAPKCS1SignatureFormatter. SHA1, SHA256, SHA384, SHA512.</param>
        public DigitalSignature(int keySize, string hashAlgorithm)
        {
            _keySize = keySize;
            _hashAlgorithm = hashAlgorithm;
        }

        /// <summary>
        /// Generate new public and private keys.
        /// </summary>
        public void AssignNewKey()
        {
            using (var rsa = new RSACryptoServiceProvider(_keySize))
            {
                rsa.PersistKeyInCsp = false;
                _publicKey = rsa.ExportParameters(false);
                _privateKey = rsa.ExportParameters(true);
            }
        }

        /// <summary>
        /// Loads a previously generated public key.
        /// </summary>
        /// <param name="exponent">The exponent<see cref="string"/>.</param>
        /// <param name="modulus">The modulus<see cref="string"/>.</param>
        public void LoadPublicKey(string exponent, string modulus)
        {
            _publicKey = new RSAParameters
            {
                Exponent = Bytes.HexToByteArray(exponent),
                Modulus = Bytes.HexToByteArray(modulus)
            };
        }

        /// <summary>
        /// The SavePublicKey.
        /// </summary>
        /// <param name="exponent">The exponent<see cref="string"/>.</param>
        /// <param name="modulus">The modulus<see cref="string"/>.</param>
        public void SavePublicKey(out string exponent, out string modulus)
        {
            exponent = Bytes.ByteArrayToHex(_publicKey.Exponent);
            modulus = Bytes.ByteArrayToHex(_publicKey.Modulus);
        }

        /// <summary>
        /// Loads a previously generated private key.
        /// </summary>
        /// <param name="exponent">The exponent<see cref="string"/>.</param>
        /// <param name="modulus">The modulus<see cref="string"/>.</param>
        /// <param name="p">The p<see cref="string"/>.</param>
        /// <param name="q">The q<see cref="string"/>.</param>
        /// <param name="dp">The dp<see cref="string"/>.</param>
        /// <param name="dq">The dq<see cref="string"/>.</param>
        /// <param name="inverseQ">The inverseQ<see cref="string"/>.</param>
        /// <param name="d">The d<see cref="string"/>.</param>
        public void LoadPrivateKey(string exponent, string modulus, string p, string q, string dp, string dq, string inverseQ, string d)
        {
            _privateKey = new RSAParameters
            {
                Exponent = Bytes.HexToByteArray(exponent),
                Modulus = Bytes.HexToByteArray(modulus),
                P = Bytes.HexToByteArray(p),
                Q = Bytes.HexToByteArray(q),
                DP = Bytes.HexToByteArray(dp),
                DQ = Bytes.HexToByteArray(dq),
                InverseQ = Bytes.HexToByteArray(inverseQ),
                D = Bytes.HexToByteArray(d)
            };
        }

        /// <summary>
        /// The SavePrivateKey.
        /// </summary>
        /// <param name="exponent">The exponent<see cref="string"/>.</param>
        /// <param name="modulus">The modulus<see cref="string"/>.</param>
        /// <param name="p">The p<see cref="string"/>.</param>
        /// <param name="q">The q<see cref="string"/>.</param>
        /// <param name="dp">The dp<see cref="string"/>.</param>
        /// <param name="dq">The dq<see cref="string"/>.</param>
        /// <param name="inverseQ">The inverseQ<see cref="string"/>.</param>
        /// <param name="d">The d<see cref="string"/>.</param>
        public void SavePrivateKey(out string exponent, out string modulus, out string p, out string q, out string dp, out string dq, out string inverseQ, out string d)
        {
            exponent = _publicKey.Exponent == null ? string.Empty : Bytes.ByteArrayToHex(_publicKey.Exponent);
            modulus = _publicKey.Modulus == null ? string.Empty : Bytes.ByteArrayToHex(_publicKey.Modulus);
            p = _publicKey.P == null ? string.Empty : Bytes.ByteArrayToHex(_publicKey.P);
            q = _publicKey.Q == null ? string.Empty : Bytes.ByteArrayToHex(_publicKey.Q);
            dp = _publicKey.DP == null ? string.Empty : Bytes.ByteArrayToHex(_publicKey.DP);
            dq = _publicKey.DQ == null ? string.Empty : Bytes.ByteArrayToHex(_publicKey.DQ);
            inverseQ = _publicKey.InverseQ == null ? string.Empty : Bytes.ByteArrayToHex(_publicKey.InverseQ);
            d = _publicKey.D == null ? string.Empty : Bytes.ByteArrayToHex(_publicKey.D);
        }

        /// <summary>
        /// Sign a hash.
        /// </summary>
        /// <param name="hashOfDataToSign">Data to sign.</param>
        /// <returns>Signature.</returns>
        public byte[] SignData(byte[] hashOfDataToSign)
        {
            using (var rsa = new RSACryptoServiceProvider(_keySize))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(_privateKey);

                var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                rsaFormatter.SetHashAlgorithm(_hashAlgorithm);
                return rsaFormatter.CreateSignature(hashOfDataToSign);
            }
        }

        /// <summary>
        /// Verify a signature.
        /// </summary>
        /// <param name="hashOfDataToSign">Data to verify.</param>
        /// <param name="signature">Signature to verify.</param>
        /// <returns>True if the signature matches.</returns>
        public bool VerifySignature(byte[] hashOfDataToSign, byte[] signature)
        {
            using (var rsa = new RSACryptoServiceProvider(_keySize))
            {
                rsa.ImportParameters(_publicKey);
                var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                rsaDeformatter.SetHashAlgorithm(_hashAlgorithm);
                return rsaDeformatter.VerifySignature(hashOfDataToSign, signature);
            }
        }
    }
}
