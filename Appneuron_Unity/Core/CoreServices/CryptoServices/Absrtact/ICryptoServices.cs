using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Appneuron.Core.CoreServices.CryptoServices.Absrtact
{
    public interface ICryptoServices
    {

        string GetRandomHexNumber(int digits);
        string EnCrypto(int longString);
        string DeCrypto(string value);
        string GenerateStringName(int longString);

    }
}
