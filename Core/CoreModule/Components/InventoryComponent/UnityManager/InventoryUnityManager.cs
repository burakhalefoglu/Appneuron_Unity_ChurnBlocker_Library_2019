namespace AppneuronUnity.Core.CoreModule.Components.InventoryComponent.UnityManager
{
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AppneuronUnity.Core.AuthModule.ClientIdComponent.UnityManager;
    using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
using AppneuronUnity.Core.Adapters.CryptoAdapter.Absrtact;
using AppneuronUnity.Core.CoreModule.Components.InventoryComponent.DataAccess;
using AppneuronUnity.Core.CoreModule.Components.InventoryComponent.DataModel;
using AppneuronUnity.Core.CoreModule.Components.InventoryComponent.UnityManager;

    internal class InventoryUnityManager : IInventoryUnityManager
    {

        internal IInventoryDal _inventoryDal;

        private readonly IDataCreationClient _dataCreationClient;

        internal ICryptoServices _cryptoServices;

        internal IClientIdUnityManager _clientIdUnityManager;

        public InventoryUnityManager(IInventoryDal inventoryDal,
            IDataCreationClient dataCreationClient,
            ICryptoServices cryptoServices,
            IClientIdUnityManager clientIdUnityManager)
        {
            _inventoryDal = inventoryDal;
            _dataCreationClient = dataCreationClient;
            _cryptoServices = cryptoServices;
            _clientIdUnityManager = clientIdUnityManager;
        }

        public async Task SendData(InventoryDataModel ınventoryDataModel)
        {
            var playerId = _clientIdUnityManager.GetPlayerID();
            var projectId = ChurnBlockerSingletonConfigs.Instance.GetProjectID();
            var customerId = ChurnBlockerSingletonConfigs.Instance.GetCustomerID();

            InventoryDataModel dataModel = ınventoryDataModel;
            dataModel.ClientId = playerId;
            dataModel.CustomerId = customerId;
            dataModel.ProjectId = projectId;

            await _dataCreationClient.PushAsync(_clientIdUnityManager.GetPlayerID(),
            dataModel, async (result) =>
            {
                if (!result)
                {
                    string fileName = _cryptoServices.GenerateStringName(6);
                    string filepath = ComponentsConfigs.InventoryDataPath;
                    await _inventoryDal.InsertAsync(filepath + fileName, dataModel);
                }
            });
        }

        public async Task CheckFileExistAndSend()
        {
            List<string> FolderList = ComponentsConfigs.GetSavedDataFilesNames(
                ComponentsConfigs.SaveTypePath.InventoryDataModel);
            foreach (var fileName in FolderList)
            {
                var dataModel = await _inventoryDal.SelectAsync(ComponentsConfigs.InventoryDataPath + fileName);
                await _dataCreationClient.PushAsync(_clientIdUnityManager.GetPlayerID(),
                dataModel, async (result) =>
                {
                    if (result)
                    {
                        await _inventoryDal.DeleteAsync(ComponentsConfigs.InventoryDataPath + fileName);

                    }
                });
            }
        }
    }
}
