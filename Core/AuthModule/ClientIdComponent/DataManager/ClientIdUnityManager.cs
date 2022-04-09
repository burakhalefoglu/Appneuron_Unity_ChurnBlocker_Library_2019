using System.Collections.Generic;
using System.Threading.Tasks;
using AppneuronUnity.Core.AuthModule.ClientIdComponent.DataModel;
using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
using UnityEngine.iOS;

namespace AppneuronUnity.Core.AuthModule.ClientIdComponent.DataManager
{
    using UnityEngine;
    using AppneuronUnity.Core.AuthModule.ClientIdComponent.DataAccess;
    using AppneuronUnity.Core.Adapters.CryptoAdapter.Absrtact;
    using Zenject;

    internal class ClientIdUnityManager : IClientIdUnityManager
    {
        private readonly IClientIdDal _clientIdDal;
        private readonly ICryptoServices _cryptoServices;

        [Inject]
        private readonly CoreHelper coreHelper;

        public ClientIdUnityManager(IClientIdDal clientIdDal, ICryptoServices cryptoServices)
        {
            _clientIdDal = clientIdDal;
            _cryptoServices = cryptoServices;
        }
        public async Task<long> GetPlayerIdAsync()
        {
            var folderNameList = coreHelper.GetSavedDataFilesNames<CustomerIdModel>();
            if (folderNameList.Count == 0)
            {
                var fileName = _cryptoServices.GenerateStringName(6);
                var clientId = _cryptoServices.GetRandomNumber();
                await _clientIdDal.InsertAsync(fileName, new CustomerIdModel
                {
                    Id = clientId
                });
                return clientId;
            }
            var clientIdModel = await _clientIdDal.SelectAsync(folderNameList[0]);
            return clientIdModel.Id;
        }
    }
}
