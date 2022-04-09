using System;
using System.Linq;
using AppneuronUnity.Core.Adapters.CryptoAdapter.Absrtact;
using AppneuronUnity.Core.Libraries.CryptoEffortless;
using static AppneuronUnity.Core.Extension.RandomExtension;
namespace AppneuronUnity.Core.BaseServices.CryptoAdapter.Concrete.Effortless
{
    internal class EfforlessCryptoServices : ICryptoServices
    {
        byte[] key = Bytes.GenerateKey();
        byte[] iv = Bytes.GenerateIV();

        public string DeCrypto(string value)
        {
            string encrypted = StringHash.Encrypt(value, key, iv);

            return encrypted;
        }

        public long GetRandomNumber()
        {
            var randomGenerator = new Random();
            return randomGenerator.NextLong(int.MaxValue, long.MaxValue);
        }

        public string EnCrypto(string encrypted)
        {
            string decrypted = StringHash.Decrypt(encrypted, key, iv);

            return decrypted;
        }

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

        public string GenerateStringName(int longString)
        {
            string salt = StringHash.CreateSalt(longString);
            return salt;
        }
    }
}
