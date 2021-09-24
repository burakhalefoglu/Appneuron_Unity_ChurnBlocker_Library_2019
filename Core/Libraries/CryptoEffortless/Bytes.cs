namespace AppneuronUnity.Core.Libraries.CryptoEffortless
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Defines the <see cref="Bytes" />.
    /// </summary>
    internal static class Bytes
    {
        /// <summary>
        /// Defines the BufferLen.
        /// </summary>
        public static int BufferLen = 4096;

        /// <summary>
        /// Defines the Rng.
        /// </summary>
        private static readonly RNGCryptoServiceProvider Rng = new RNGCryptoServiceProvider();

        /// <summary>
        /// Defines the _paddingMode.
        /// </summary>
        private static PaddingMode _paddingMode = PaddingMode.ISO10126;

        /// <summary>
        /// Defines the _cipherMode.
        /// </summary>
        private static CipherMode _cipherMode = CipherMode.CBC;

        /// <summary>
        /// Defines the BlockSize.
        /// </summary>
        public enum BlockSize
        {
            /// <summary>
            /// Defines the Default.
            /// </summary>
            Default = 256,
            /// <summary>
            /// Defines the Size128.
            /// </summary>
            Size128 = 128,
            /// <summary>
            /// Defines the Size192.
            /// </summary>
            Size192 = 192,
            /// <summary>
            /// Defines the Size256.
            /// </summary>
            Size256 = 256
        }

        /// <summary>
        /// Defines the KeySize.
        /// </summary>
        public enum KeySize
        {
            /// <summary>
            /// Defines the Default.
            /// </summary>
            Default = 256,
            /// <summary>
            /// Defines the Size128.
            /// </summary>
            Size128 = 128,
            /// <summary>
            /// Defines the Size192.
            /// </summary>
            Size192 = 192,
            /// <summary>
            /// Defines the Size256.
            /// </summary>
            Size256 = 256
        }

        /// <summary>
        /// The ResetPaddingAndCipherModes.
        /// </summary>
        public static void ResetPaddingAndCipherModes()
        {
            _paddingMode = PaddingMode.ISO10126;
            _cipherMode = CipherMode.CBC;
        }

        /// <summary>
        /// The SetPaddingAndCipherModes.
        /// </summary>
        /// <param name="paddingMode">The paddingMode<see cref="PaddingMode"/>.</param>
        /// <param name="cipherMode">The cipherMode<see cref="CipherMode"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool SetPaddingAndCipherModes(PaddingMode paddingMode, CipherMode cipherMode)
        {
            if (paddingMode == PaddingMode.PKCS7 && (cipherMode == CipherMode.OFB || cipherMode == CipherMode.CTS))
                return false; // invalid
            if (paddingMode == PaddingMode.Zeros)
                return false; // invalid and/or encrypt/decrypt will mismatch
            if (paddingMode == PaddingMode.ANSIX923 && (cipherMode == CipherMode.OFB || cipherMode == CipherMode.CTS))
                return false; // invalid
            if (paddingMode == PaddingMode.ISO10126 && (cipherMode == CipherMode.OFB || cipherMode == CipherMode.CTS))
                return false; // invalid

            _paddingMode = paddingMode;
            _cipherMode = cipherMode;

            return true;
        }

        /// <summary>
        /// The GetRijndaelManaged.
        /// </summary>
        /// <param name="key">The key<see cref="byte[]"/>.</param>
        /// <param name="iv">The iv<see cref="byte[]"/>.</param>
        /// <param name="keySize">The keySize<see cref="KeySize"/>.</param>
        /// <param name="blockSize">The blockSize<see cref="BlockSize"/>.</param>
        /// <returns>The <see cref="RijndaelManaged"/>.</returns>
        private static RijndaelManaged GetRijndaelManaged(byte[] key, byte[] iv, KeySize keySize, BlockSize blockSize)
        {
            var rm = new RijndaelManaged
            {
                KeySize = (int)keySize,
                BlockSize = (int)blockSize,
                Padding = _paddingMode,
                Mode = _cipherMode
            };

            if (key != null)
                rm.Key = key;

            if (iv != null)
                rm.IV = iv;

            return rm;
        }

        /// <summary>
        /// Returns an encryption key to be used with the Rijndael algorithm.
        /// </summary>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public static byte[] GenerateKey()
        {
            return GenerateKey(KeySize.Default, BlockSize.Default);
        }

        /// <summary>
        /// Returns an encryption key to be used with the Rijndael algorithm.
        /// </summary>
        /// <param name="keySize">The keySize<see cref="KeySize"/>.</param>
        /// <param name="blockSize">The blockSize<see cref="BlockSize"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public static byte[] GenerateKey(KeySize keySize, BlockSize blockSize)
        {
            using (var rm = GetRijndaelManaged(null, null, keySize, blockSize))
            {
                rm.GenerateKey();
                return rm.Key;
            }
        }

        /// <summary>
        /// Returns an encryption key to be used with the Rijndael algorithm.
        /// </summary>
        /// <param name="password">Password to create key with.</param>
        /// <param name="salt">Salt to create key with.</param>
        /// <param name="keySize">Can be 128, 192, or 256.</param>
        /// <param name="iterationCount">The number of iterations to derive the key.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public static byte[] GenerateKey(string password, string salt, KeySize keySize, int iterationCount)
        {
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrEmpty(salt)) throw new ArgumentNullException(nameof(salt));

            var saltValueBytes = Encoding.Unicode.GetBytes(salt);
            if (saltValueBytes.Length < 8)
                throw new ArgumentException("Salt is not at least eight bytes");

            var derivedPassword = new Rfc2898DeriveBytes(password, saltValueBytes, iterationCount);
            return derivedPassword.GetBytes((int)keySize / 8);
        }

        /// <summary>
        /// Returns the encryption IV to be used with the Rijndael algorithm.
        /// </summary>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public static byte[] GenerateIV()
        {
            return GenerateIV(KeySize.Default, BlockSize.Default);
        }

        /// <summary>
        /// Returns the encryption IV to be used with the Rijndael algorithm.
        /// </summary>
        /// <param name="keySize">The keySize<see cref="KeySize"/>.</param>
        /// <param name="blockSize">The blockSize<see cref="BlockSize"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public static byte[] GenerateIV(KeySize keySize, BlockSize blockSize)
        {
            using (var rm = GetRijndaelManaged(null, null, keySize, blockSize))
            {
                rm.GenerateIV();
                return rm.IV;
            }
        }

        /// <summary>
        /// Encrypt a byte array into a byte array using the given Key and an IV.
        /// </summary>
        /// <param name="clearData">The clearData<see cref="byte[]"/>.</param>
        /// <param name="key">The key<see cref="byte[]"/>.</param>
        /// <param name="iv">The iv<see cref="byte[]"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public static byte[] Encrypt(byte[] clearData, byte[] key, byte[] iv)
        {
            return Encrypt(clearData, key, iv, KeySize.Default, BlockSize.Default);
        }

        /// <summary>
        /// Encrypt a byte array into a byte array using the given Key and an IV.
        /// </summary>
        /// <param name="clearData">The clearData<see cref="byte[]"/>.</param>
        /// <param name="key">The key<see cref="byte[]"/>.</param>
        /// <param name="iv">The iv<see cref="byte[]"/>.</param>
        /// <param name="keySize">The keySize<see cref="KeySize"/>.</param>
        /// <param name="blockSize">The blockSize<see cref="BlockSize"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public static byte[] Encrypt(byte[] clearData, byte[] key, byte[] iv, KeySize keySize, BlockSize blockSize)
        {
            if (clearData == null || clearData.Length <= 0) throw new ArgumentNullException(nameof(clearData));
            if (key == null || key.Length <= 0) throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0) throw new ArgumentNullException(nameof(iv));

            // Create a MemoryStream to accept the encrypted bytes
            var memoryStream = new MemoryStream();

            // Create a symmetric algorithm.
            // We are going to use Rijndael because it is strong and available on all platforms.
            // You can use other algorithms, to do so substitute the next line with something like
            // TripleDES alg = TripleDES.Create();
            using (var alg = GetRijndaelManaged(key, iv, keySize, blockSize))
            {
                // Create a CryptoStream through which we are going to be pumping our data.
                // CryptoStreamMode.Write means that we are going to be writing data to the stream and the
                // output will be written in the MemoryStream we have provided.
                using (var cs = new CryptoStream(memoryStream, alg.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    // Write the data and make it do the encryption
                    cs.Write(clearData, 0, clearData.Length);

                    // Close the crypto stream (or do FlushFinalBlock).
                    // This will tell it that we have done our encryption and there is no more data coming in,
                    // and it is now a good time to apply the padding and finalize the encryption process.
                    cs.FlushFinalBlock();
                    cs.Close();
                }
            }

            // Now get the encrypted data from the MemoryStream.
            // Some people make a mistake of using GetBuffer() here, which is not the right way.
            var encryptedData = memoryStream.ToArray();

            return encryptedData;
        }

        /// <summary>
        /// Encrypt a file into another file.
        /// </summary>
        /// <param name="clearStreamIn">The clearStreamIn<see cref="Stream"/>.</param>
        /// <param name="encryptedFileOut">The encryptedFileOut<see cref="string"/>.</param>
        /// <param name="alg">The alg<see cref="RijndaelManaged"/>.</param>
        public static void Encrypt(Stream clearStreamIn, string encryptedFileOut, RijndaelManaged alg)
        {
            if (clearStreamIn == null) throw new ArgumentNullException(nameof(clearStreamIn));
            if (string.IsNullOrEmpty(encryptedFileOut)) throw new ArgumentNullException(nameof(encryptedFileOut));
            if (alg == null) throw new ArgumentNullException(nameof(alg));

            // First we are going to open the file streams
            using (var fsOut = new FileStream(encryptedFileOut, FileMode.OpenOrCreate, FileAccess.Write))
            {
                // Now create a crypto stream through which we are going to be pumping data.
                // Our encryptedFileOut is going to be receiving the encrypted bytes.
                using (var cs = new CryptoStream(fsOut, alg.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    // Now will will initialize a buffer and will be processing the input file in chunks.
                    // This is done to avoid reading the whole file (which can be huge) into memory.
                    var buffer = new byte[BufferLen];
                    int bytesRead;

                    do
                    {
                        bytesRead = clearStreamIn.Read(buffer, 0, BufferLen); // Read a chunk of data from the input file
                        if (bytesRead > 0)
                            cs.Write(buffer, 0, bytesRead); // Encrypt it
                    } while (bytesRead != 0);

                    // Close everything. This will also close the unrelying clearStreamOut stream
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                clearStreamIn.Close();
            }
        }

        /// <summary>
        /// Encrypt a file into another file.
        /// </summary>
        /// <param name="clearFileIn">The clearFileIn<see cref="string"/>.</param>
        /// <param name="encryptedFileOut">The encryptedFileOut<see cref="string"/>.</param>
        /// <param name="key">The key<see cref="byte[]"/>.</param>
        /// <param name="iv">The iv<see cref="byte[]"/>.</param>
        public static void Encrypt(string clearFileIn, string encryptedFileOut, byte[] key, byte[] iv)
        {
            Encrypt(clearFileIn, encryptedFileOut, key, iv, KeySize.Default, BlockSize.Default);
        }

        /// <summary>
        /// Encrypt a file into another file.
        /// </summary>
        /// <param name="clearFileIn">The clearFileIn<see cref="string"/>.</param>
        /// <param name="encryptedFileOut">The encryptedFileOut<see cref="string"/>.</param>
        /// <param name="key">The key<see cref="byte[]"/>.</param>
        /// <param name="iv">The iv<see cref="byte[]"/>.</param>
        /// <param name="keySize">The keySize<see cref="KeySize"/>.</param>
        /// <param name="blockSize">The blockSize<see cref="BlockSize"/>.</param>
        public static void Encrypt(string clearFileIn, string encryptedFileOut, byte[] key, byte[] iv, KeySize keySize, BlockSize blockSize)
        {
            if (string.IsNullOrEmpty(clearFileIn)) throw new ArgumentNullException(nameof(clearFileIn));
            if (string.IsNullOrEmpty(encryptedFileOut)) throw new ArgumentNullException(nameof(encryptedFileOut));

            if (key == null || key.Length <= 0) throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0) throw new ArgumentNullException(nameof(iv));

            using (var alg = GetRijndaelManaged(key, iv, keySize, blockSize))
            {
                using (var fsIn = new FileStream(clearFileIn, FileMode.Open, FileAccess.Read))
                {
                    Encrypt(fsIn, encryptedFileOut, alg);
                }
            }
        }

        /// <summary>
        /// Encrypt a stream into a file.
        /// </summary>
        /// <param name="clearStreamIn">The clearStreamIn<see cref="Stream"/>.</param>
        /// <param name="encryptedFileOut">The encryptedFileOut<see cref="string"/>.</param>
        /// <param name="key">The key<see cref="byte[]"/>.</param>
        /// <param name="iv">The iv<see cref="byte[]"/>.</param>
        public static void Encrypt(Stream clearStreamIn, string encryptedFileOut, byte[] key, byte[] iv)
        {
            Encrypt(clearStreamIn, encryptedFileOut, key, iv, KeySize.Default, BlockSize.Default);
        }

        /// <summary>
        /// Encrypt a stream into a file.
        /// </summary>
        /// <param name="clearStreamIn">The clearStreamIn<see cref="Stream"/>.</param>
        /// <param name="encryptedFileOut">The encryptedFileOut<see cref="string"/>.</param>
        /// <param name="key">The key<see cref="byte[]"/>.</param>
        /// <param name="iv">The iv<see cref="byte[]"/>.</param>
        /// <param name="keySize">The keySize<see cref="KeySize"/>.</param>
        /// <param name="blockSize">The blockSize<see cref="BlockSize"/>.</param>
        public static void Encrypt(Stream clearStreamIn, string encryptedFileOut, byte[] key, byte[] iv, KeySize keySize, BlockSize blockSize)
        {
            if (clearStreamIn == null) throw new ArgumentNullException(nameof(clearStreamIn));
            if (string.IsNullOrEmpty(encryptedFileOut)) throw new ArgumentNullException(nameof(encryptedFileOut));
            if (key == null || key.Length <= 0) throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0) throw new ArgumentNullException(nameof(iv));

            using (var alg = GetRijndaelManaged(key, iv, keySize, blockSize))
            {
                Encrypt(clearStreamIn, encryptedFileOut, alg);
            }
        }

        /// <summary>
        /// Encrypt a file into another file.
        ///     The Key and an IV are automatically generated. These will be required when Decrypting the data.
        /// </summary>
        /// <param name="clearFileIn">The clearFileIn<see cref="string"/>.</param>
        /// <param name="encryptedFileOut">The encryptedFileOut<see cref="string"/>.</param>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <param name="iv">The iv<see cref="string"/>.</param>
        public static void Encrypt(string clearFileIn, string encryptedFileOut, out string key, out string iv)
        {
            Encrypt(clearFileIn, encryptedFileOut, KeySize.Default, BlockSize.Default, out key, out iv);
        }

        /// <summary>
        /// Encrypt a file into another file.
        ///     The Key and an IV are automatically generated. These will be required when Decrypting the data.
        /// </summary>
        /// <param name="clearFileIn">The clearFileIn<see cref="string"/>.</param>
        /// <param name="encryptedFileOut">The encryptedFileOut<see cref="string"/>.</param>
        /// <param name="keySize">The keySize<see cref="KeySize"/>.</param>
        /// <param name="blockSize">The blockSize<see cref="BlockSize"/>.</param>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <param name="iv">The iv<see cref="string"/>.</param>
        public static void Encrypt(string clearFileIn, string encryptedFileOut, KeySize keySize, BlockSize blockSize, out string key, out string iv)
        {
            if (string.IsNullOrEmpty(clearFileIn)) throw new ArgumentNullException(nameof(clearFileIn));
            if (string.IsNullOrEmpty(encryptedFileOut)) throw new ArgumentNullException(nameof(encryptedFileOut));

            using (var alg = GetRijndaelManaged(null, null, keySize, blockSize))
            {
                alg.GenerateIV();
                alg.GenerateKey();

                key = Convert.ToBase64String(alg.Key);
                iv = Convert.ToBase64String(alg.IV);

                using (var fsIn = new FileStream(clearFileIn, FileMode.Open, FileAccess.Read))
                {
                    Encrypt(fsIn, encryptedFileOut, alg);
                }
            }
        }

        /// <summary>
        /// Encrypt a stream into a file.
        ///     The Key and an IV are automatically generated. These will be required when Decrypting the data.
        /// </summary>
        /// <param name="clearStreamIn">The clearStreamIn<see cref="Stream"/>.</param>
        /// <param name="encryptedFileOut">The encryptedFileOut<see cref="string"/>.</param>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <param name="iv">The iv<see cref="string"/>.</param>
        public static void Encrypt(Stream clearStreamIn, string encryptedFileOut, out string key, out string iv)
        {
            Encrypt(clearStreamIn, encryptedFileOut, KeySize.Default, BlockSize.Default, out key, out iv);
        }

        /// <summary>
        /// Encrypt a stream into a file.
        ///     The Key and an IV are automatically generated. These will be required when Decrypting the data.
        /// </summary>
        /// <param name="clearStreamIn">The clearStreamIn<see cref="Stream"/>.</param>
        /// <param name="encryptedFileOut">The encryptedFileOut<see cref="string"/>.</param>
        /// <param name="keySize">The keySize<see cref="KeySize"/>.</param>
        /// <param name="blockSize">The blockSize<see cref="BlockSize"/>.</param>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <param name="iv">The iv<see cref="string"/>.</param>
        public static void Encrypt(Stream clearStreamIn, string encryptedFileOut, KeySize keySize, BlockSize blockSize, out string key, out string iv)
        {
            if (clearStreamIn == null) throw new ArgumentNullException(nameof(clearStreamIn));
            if (string.IsNullOrEmpty(encryptedFileOut)) throw new ArgumentNullException(nameof(encryptedFileOut));

            using (var alg = GetRijndaelManaged(null, null, keySize, blockSize))
            {
                alg.GenerateIV();
                alg.GenerateKey();

                key = Convert.ToBase64String(alg.Key);
                iv = Convert.ToBase64String(alg.IV);

                Encrypt(clearStreamIn, encryptedFileOut, alg);
            }
        }

        /// <summary>
        /// Decrypt a byte array into a byte array using a Key and an IV.
        /// </summary>
        /// <param name="cipherData">The cipherData<see cref="byte[]"/>.</param>
        /// <param name="key">The key<see cref="byte[]"/>.</param>
        /// <param name="iv">The iv<see cref="byte[]"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public static byte[] Decrypt(byte[] cipherData, byte[] key, byte[] iv)
        {
            return Decrypt(cipherData, key, iv, KeySize.Default, BlockSize.Default);
        }

        /// <summary>
        /// Decrypt a byte array into a byte array using a Key and an IV.
        /// </summary>
        /// <param name="cipherData">The cipherData<see cref="byte[]"/>.</param>
        /// <param name="key">The key<see cref="byte[]"/>.</param>
        /// <param name="iv">The iv<see cref="byte[]"/>.</param>
        /// <param name="keySize">The keySize<see cref="KeySize"/>.</param>
        /// <param name="blockSize">The blockSize<see cref="BlockSize"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public static byte[] Decrypt(byte[] cipherData, byte[] key, byte[] iv, KeySize keySize, BlockSize blockSize)
        {
            if (cipherData == null) throw new ArgumentNullException(nameof(cipherData));
            if (key == null || key.Length <= 0) throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0) throw new ArgumentNullException(nameof(iv));

            if (cipherData.Length < 1) throw new ArgumentException("cipherData");

            // Create a MemoryStream that is going to accept the decrypted bytes
            using (var memoryStream = new MemoryStream())
            {
                // Create a symmetric algorithm.
                // We are going to use Rijndael because it is strong and available on all platforms.
                // You can use other algorithms, to do so substitute the next line with something like
                // TripleDES alg = TripleDES.Create();
                using (var alg = GetRijndaelManaged(key, iv, keySize, blockSize))
                {
                    // Create a CryptoStream through which we are going to be pumping our data.
                    // CryptoStreamMode.Write means that we are going to be writing data to the stream
                    // and the output will be written in the MemoryStream we have provided.
                    using (var cs = new CryptoStream(memoryStream, alg.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        // Write the data and make it do the decryption
                        cs.Write(cipherData, 0, cipherData.Length);

                        // Close the crypto stream (or do FlushFinalBlock).
                        // This will tell it that we have done our decryption and there is no more data coming in,
                        // and it is now a good time to remove the padding and finalize the decryption process.
                        cs.FlushFinalBlock();
                        cs.Close();
                    }
                }

                // Now get the decrypted data from the MemoryStream.
                // Some people make a mistake of using GetBuffer() here, which is not the right way.
                var decryptedData = memoryStream.ToArray();
                return decryptedData;
            }
        }

        /// <summary>
        /// Decrypt a file into another file.
        /// </summary>
        /// <param name="encryptedStreamIn">The encryptedStreamIn<see cref="Stream"/>.</param>
        /// <param name="clearStreamOut">The clearStreamOut<see cref="Stream"/>.</param>
        /// <param name="alg">The alg<see cref="RijndaelManaged"/>.</param>
        public static void Decrypt(Stream encryptedStreamIn, Stream clearStreamOut, RijndaelManaged alg)
        {
            if (encryptedStreamIn == null) throw new ArgumentNullException(nameof(encryptedStreamIn));
            if (clearStreamOut == null) throw new ArgumentNullException(nameof(clearStreamOut));
            if (alg == null) throw new ArgumentNullException(nameof(alg));

            // Now create a crypto stream through which we are going to be pumping data.
            // Our encryptedFileOut is going to be receiving the Decrypted bytes.
            var cs = new CryptoStream(clearStreamOut, alg.CreateDecryptor(), CryptoStreamMode.Write);

            // Now will will initialize a buffer and will be processing the input file in chunks.
            // This is done to avoid reading the whole file (which can be huge) into memory.
            var buffer = new byte[BufferLen];
            int bytesRead;

            do
            {
                bytesRead = encryptedStreamIn.Read(buffer, 0, BufferLen); // Read a chunk of data from the input file
                if (bytesRead > 0)
                    cs.Write(buffer, 0, bytesRead); // Decrypt it
            } while (bytesRead != 0);

            // Close everything
            cs.FlushFinalBlock();
            //cs.Close(); // Causes an exception when streaming to http
            encryptedStreamIn.Close();
        }

        /// <summary>
        /// Decrypt a file into another file.
        /// </summary>
        /// <param name="encryptedFileIn">The encryptedFileIn<see cref="string"/>.</param>
        /// <param name="clearFileOut">The clearFileOut<see cref="string"/>.</param>
        /// <param name="key">The key<see cref="byte[]"/>.</param>
        /// <param name="iv">The iv<see cref="byte[]"/>.</param>
        public static void Decrypt(string encryptedFileIn, string clearFileOut, byte[] key, byte[] iv)
        {
            Decrypt(encryptedFileIn, clearFileOut, key, iv, KeySize.Default, BlockSize.Default);
        }

        /// <summary>
        /// Decrypt a file into another file.
        /// </summary>
        /// <param name="encryptedFileIn">The encryptedFileIn<see cref="string"/>.</param>
        /// <param name="clearFileOut">The clearFileOut<see cref="string"/>.</param>
        /// <param name="key">The key<see cref="byte[]"/>.</param>
        /// <param name="iv">The iv<see cref="byte[]"/>.</param>
        /// <param name="keySize">The keySize<see cref="KeySize"/>.</param>
        /// <param name="blockSize">The blockSize<see cref="BlockSize"/>.</param>
        public static void Decrypt(string encryptedFileIn, string clearFileOut, byte[] key, byte[] iv, KeySize keySize, BlockSize blockSize)
        {
            if (string.IsNullOrEmpty(encryptedFileIn)) throw new ArgumentNullException(nameof(encryptedFileIn));
            if (string.IsNullOrEmpty(clearFileOut)) throw new ArgumentNullException(nameof(clearFileOut));
            if (key == null || key.Length <= 0) throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0) throw new ArgumentNullException(nameof(iv));

            // First we are going to open the file streams
            using (var fsIn = new FileStream(encryptedFileIn, FileMode.Open, FileAccess.Read))
            {
                using (var fsOut = new FileStream(clearFileOut, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (var alg = GetRijndaelManaged(key, iv, keySize, blockSize))
                    {
                        Decrypt(fsIn, fsOut, alg);
                    }
                }
            }
        }

        /// <summary>
        /// Decrypt a file into another file using a Key and an IV.
        /// </summary>
        /// <param name="encryptedFileIn">The encryptedFileIn<see cref="string"/>.</param>
        /// <param name="clearFileOut">The clearFileOut<see cref="string"/>.</param>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <param name="iv">The iv<see cref="string"/>.</param>
        public static void Decrypt(string encryptedFileIn, string clearFileOut, string key, string iv)
        {
            if (string.IsNullOrEmpty(encryptedFileIn)) throw new ArgumentNullException(nameof(encryptedFileIn));
            if (string.IsNullOrEmpty(clearFileOut)) throw new ArgumentNullException(nameof(clearFileOut));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrEmpty(iv)) throw new ArgumentNullException(nameof(iv));

            Decrypt(encryptedFileIn, clearFileOut, Convert.FromBase64String(key), Convert.FromBase64String(iv));
        }

        /// <summary>
        /// Decrypt a file into another file using a Key and an IV.
        /// </summary>
        /// <param name="encryptedFileIn">The encryptedFileIn<see cref="string"/>.</param>
        /// <param name="clearStreamOut">The clearStreamOut<see cref="Stream"/>.</param>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <param name="iv">The iv<see cref="string"/>.</param>
        public static void Decrypt(string encryptedFileIn, Stream clearStreamOut, string key, string iv)
        {
            Decrypt(encryptedFileIn, clearStreamOut, key, iv, KeySize.Default, BlockSize.Default);
        }

        /// <summary>
        /// Decrypt a file into another file using a Key and an IV.
        /// </summary>
        /// <param name="encryptedFileIn">The encryptedFileIn<see cref="string"/>.</param>
        /// <param name="clearStreamOut">The clearStreamOut<see cref="Stream"/>.</param>
        /// <param name="key">The key<see cref="string"/>.</param>
        /// <param name="iv">The iv<see cref="string"/>.</param>
        /// <param name="keySize">The keySize<see cref="KeySize"/>.</param>
        /// <param name="blockSize">The blockSize<see cref="BlockSize"/>.</param>
        public static void Decrypt(string encryptedFileIn, Stream clearStreamOut, string key, string iv, KeySize keySize, BlockSize blockSize)
        {
            if (encryptedFileIn == null) throw new ArgumentNullException(nameof(encryptedFileIn));
            if (clearStreamOut == null) throw new ArgumentNullException(nameof(clearStreamOut));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrEmpty(iv)) throw new ArgumentNullException(nameof(iv));

            using (var fsIn = new FileStream(encryptedFileIn, FileMode.Open, FileAccess.Read))
            {
                using (var alg = GetRijndaelManaged(Convert.FromBase64String(key), Convert.FromBase64String(iv), keySize, blockSize))
                {
                    Decrypt(fsIn, clearStreamOut, alg);
                }
            }
        }

        /// <summary>
        /// Converts HEX string to byte array.
        /// Opposite of ByteArrayToHex.
        /// </summary>
        /// <param name="hexString">The hexString<see cref="string"/>.</param>
        /// <returns>The <see cref="byte[]"/>.</returns>
        public static byte[] HexToByteArray(string hexString)
        {
            if (hexString == null) throw new ArgumentNullException(nameof(hexString));

            if (hexString.Length % 2 != 0)
                throw new ApplicationException("Hex string must be multiple of 2 in length");

            var byteCount = hexString.Length / 2;
            var byteValues = new byte[byteCount];
            for (var i = 0; i < byteCount; i++)
            {
                byteValues[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return byteValues;
        }

        /// <summary>
        /// Convert bytes to 2 hex characters per byte, "-" separators are removed.
        /// Opposite of HexToByteArray.
        /// </summary>
        /// <param name="data">The data<see cref="byte[]"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string ByteArrayToHex(byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            return BitConverter.ToString(data).Replace("-", "");
        }

        /// <summary>
        /// Use cryptographically strong random number generator to fill buffer with random data.
        /// </summary>
        /// <param name="buffer">The buffer<see cref="byte[]"/>.</param>
        public static void GetRandomBytes(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            Rng.GetBytes(buffer);
        }
    }
}
