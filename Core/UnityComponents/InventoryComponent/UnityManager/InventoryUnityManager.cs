namespace AppneuronUnity.Core.UnityComponents.InventoryComponent.UnityManager
{
    using AppneuronUnity.Core.CoreServices.CryptoServices.Absrtact;
    using AppneuronUnity.Core.CoreServices.MessageBrokers;
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System.Collections.Generic;
    using System.Threading.Tasks;
using AppneuronUnity.Core.UnityComponents.InventoryComponent.DataAccess;
using AppneuronUnity.Core.UnityComponents.InventoryComponent.DataModel;
using AppneuronUnity.Core.UnityComponents.InventoryComponent.UnityManager;
using AppneuronUnity.Core.UnityWorkers.ClientIdWorker.UnityManager;

    /// <summary>
    /// Defines the <see cref="InventoryUnityManager" />.
    /// </summary>
    internal class InventoryUnityManager : IInventoryUnityManager
    {
        /// <summary>
        /// Defines the _inventoryDal.
        /// </summary>
        internal IInventoryDal _inventoryDal;

        /// <summary>
        /// Defines the _kafkaMessageBroker.
        /// </summary>
        internal IMessageBrokerService _kafkaMessageBroker;

        /// <summary>
        /// Defines the _cryptoServices.
        /// </summary>
        internal ICryptoServices _cryptoServices;

        /// <summary>
        /// Defines the _clientIdUnityManager.
        /// </summary>
        internal IClientIdUnityManager _clientIdUnityManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryUnityManager"/> class.
        /// </summary>
        /// <param name="inventoryDal">The inventoryDal<see cref="IInventoryDal"/>.</param>
        /// <param name="kafkaMessageBroker">The kafkaMessageBroker<see cref="IMessageBrokerService"/>.</param>
        /// <param name="cryptoServices">The cryptoServices<see cref="ICryptoServices"/>.</param>
        /// <param name="clientIdUnityManager">The clientIdUnityManager<see cref="IClientIdUnityManager"/>.</param>
        public InventoryUnityManager(IInventoryDal inventoryDal,
            IMessageBrokerService kafkaMessageBroker,
            ICryptoServices cryptoServices,
            IClientIdUnityManager clientIdUnityManager)
        {
            _inventoryDal = inventoryDal;
            _kafkaMessageBroker = kafkaMessageBroker;
            _cryptoServices = cryptoServices;
            _clientIdUnityManager = clientIdUnityManager;
        }

        /// <summary>
        /// The SendData.
        /// </summary>
        /// <param name="ınventoryDataModel">The ınventoryDataModel<see cref="InventoryDataModel"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SendData(InventoryDataModel ınventoryDataModel)
        {
            var playerId = _clientIdUnityManager.GetPlayerID();
            var projectId = ChurnBlockerSingletonConfigs.Instance.GetProjectID();
            var customerId = ChurnBlockerSingletonConfigs.Instance.GetCustomerID();

            InventoryDataModel dataModel = ınventoryDataModel;
            dataModel.ClientId = playerId;
            dataModel.CustomerId = customerId;
            dataModel.ProjectId = projectId;

            var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
            if (!result.Success)
            {
                string fileName = _cryptoServices.GenerateStringName(6);
                string filepath = ComponentsConfigs.InventoryDataPath;
                await _inventoryDal.InsertAsync(filepath + fileName, dataModel);
            }
        }

        /// <summary>
        /// The CheckFileExistAndSend.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task CheckFileExistAndSend()
        {
            List<string> FolderList = ComponentsConfigs.GetSavedDataFilesNames(
                ComponentsConfigs.SaveTypePath.InventoryDataModel);
            foreach (var fileName in FolderList)
            {
                var dataModel = await _inventoryDal.SelectAsync(ComponentsConfigs.InventoryDataPath + fileName);
                var result = await _kafkaMessageBroker.SendMessageAsync(dataModel);
                if (result.Success)
                {
                    await _inventoryDal.DeleteAsync(ComponentsConfigs.InventoryDataPath + fileName);
                }
            }
        }
    }
}
