namespace AppneuronUnity.Core.Adapters.CryptoAdapter.Absrtact
{

    internal interface ICryptoServices
    {
        string GetRandomHexNumber(int digits);
        long GetRandomNumber();

        string EnCrypto(string encrypted);

        string DeCrypto(string value);

        string GenerateStringName(int longString);
    }
}
