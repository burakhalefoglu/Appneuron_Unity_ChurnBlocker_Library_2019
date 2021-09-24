namespace AppneuronUnity.Core.UnityWorkers.ClientIdWorker.UnityManager
{
    using AppneuronUnity.Core.CoreServices.CryptoServices.Absrtact;
    using System.IO;
    using System.Threading.Tasks;
    using UnityEngine;
    using static AppneuronUnity.Core.UnityWorkers.ClientIdWorker.Helper.ClientIdConfigServices;
using AppneuronUnity.Core.UnityWorkers.ClientIdWorker.DataAccess;
using AppneuronUnity.Core.UnityWorkers.ClientIdWorker.DataModel;
using AppneuronUnity.Core.UnityWorkers.ClientIdWorker.UnityManager;

    /// <summary>
    /// Defines the <see cref="ClientIdUnityManager" />.
    /// </summary>
    internal class ClientIdUnityManager : IClientIdUnityManager
    {
        /// <summary>
        /// Defines the _IidDal.
        /// </summary>
        private IClientIdDal _IidDal;

        /// <summary>
        /// Defines the _cryptoServices.
        /// </summary>
        private ICryptoServices _cryptoServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientIdUnityManager"/> class.
        /// </summary>
        /// <param name="ıidDal">The ıidDal<see cref="IClientIdDal"/>.</param>
        /// <param name="cryptoServices">The cryptoServices<see cref="ICryptoServices"/>.</param>
        public ClientIdUnityManager(IClientIdDal ıidDal, ICryptoServices cryptoServices)
        {
            _IidDal = ıidDal;
            _cryptoServices = cryptoServices;
        }

        /// <summary>
        /// The SaveIdOnLocalStorage.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SaveIdOnLocalStorage()
        {
            string savePath = CustomerIdPath + "_ID";

            if (!File.Exists(savePath))
            {
                string id = GenerateId();
                await _IidDal.InsertAsync(savePath, new CustomerIdModel
                {
                    _id = id
                });
            }
        }

        /// <summary>
        /// The GetPlayerID.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetPlayerID()
        {
            return SystemInfo.deviceUniqueIdentifier;
        }

        /// <summary>
        /// The GenerateId.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string GenerateId()
        {
            return _cryptoServices.GetRandomHexNumber(32);
        }
    }
}
