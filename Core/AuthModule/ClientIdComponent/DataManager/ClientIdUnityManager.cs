namespace AppneuronUnity.Core.AuthModule.ClientIdComponent.DataManager
{
    using UnityEngine;
    using AppneuronUnity.Core.AuthModule.ClientIdComponent.DataAccess;
    using AppneuronUnity.Core.Adapters.CryptoAdapter.Absrtact;
    using Zenject;

    internal class ClientIdUnityManager : IClientIdUnityManager
    {
        [Inject]
        private ICryptoServices _cryptoServices;

        public ClientIdUnityManager(ICryptoServices cryptoServices)
        {
            _cryptoServices = cryptoServices;
        }
        public string GetPlayerID()
        {
            return SystemInfo.deviceUniqueIdentifier;
        }
        public string GenerateId()
        {
            return _cryptoServices.GetRandomHexNumber(32);
        }
    }
}
