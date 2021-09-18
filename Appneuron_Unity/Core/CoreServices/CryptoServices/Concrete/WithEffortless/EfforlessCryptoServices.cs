using Appneuron.Core.CoreServices.CryptoServices.Concrete.WithEffortless.Library;
using Assets.Appneuron.Core.CoreServices.CryptoServices.Absrtact;
using System;
using System.Linq;

namespace Assets.Appneuron.Core.CoreServices.CryptoServices.Concrete.WithEffortless
{
    public class EfforlessCryptoServices : ICryptoServices
    {


        public string DeCrypto(string value)
        {
            return null;

        }

        public string EnCrypto(int longString)
        {
            return null;
        }

        public string GetRandomHexNumber(int digits)
        {
            Random random = new Random();
            byte[] buffer = new byte[digits / 2];
            random.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
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
