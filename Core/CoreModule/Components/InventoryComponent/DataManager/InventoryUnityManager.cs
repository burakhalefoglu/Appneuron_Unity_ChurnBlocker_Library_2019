using AppneuronUnity.Core.BaseServices.WebsocketAdapter.WebsocketSharp;

namespace AppneuronUnity.Core.CoreModule.Components.InventoryComponent.DataManager
{
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
    using AppneuronUnity.Core.Adapters.CryptoAdapter.Absrtact;
    using AppneuronUnity.Core.CoreModule.Components.InventoryComponent.DataAccess;
    using AppneuronUnity.Core.CoreModule.Components.InventoryComponent.DataModel;
    using Zenject;
    using AppneuronUnity.Core.AuthModule.ClientIdComponent.DataManager;

    internal class InventoryUnityManager : IInventoryUnityManager
    {

        private readonly IInventoryDal _inventoryDal;

        private readonly IDataCreationClient _dataCreationClient;

        private readonly ICryptoServices _cryptoServices;

        private readonly IClientIdUnityManager _clientIdUnityManager;

        [Inject]
        private readonly CoreHelper coreHelper;

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
            var playerId =await _clientIdUnityManager.GetPlayerIdAsync();
            var projectId = coreHelper.GetProjectInfo().ProjectId;
            var customerId = coreHelper.GetProjectInfo().CustomerId;

            InventoryDataModel dataModel = ınventoryDataModel;
            dataModel.ClientId = playerId;
            dataModel.CustomerId = customerId;
            dataModel.ProjectId = projectId;

            await _dataCreationClient.PushAsync(await _clientIdUnityManager.GetPlayerIdAsync(),
            coreHelper.GetProjectInfo().ProjectId,
            dataModel, async (result) =>
            {
                if (!result)
                {
                    string fileName = _cryptoServices.GenerateStringName(6);
                    await _inventoryDal.InsertAsync(fileName, dataModel);
                }
            });
        }

        public async Task CheckFileExistAndSend()
        {
            List<string> FolderList = coreHelper.GetSavedDataFilesNames<InventoryDataModel>();
            if (FolderList.Count == 0)
                return;
            foreach (var fileName in FolderList)
            {
                var dataModel = await _inventoryDal.SelectAsync(fileName);
                await _dataCreationClient.PushAsync(await _clientIdUnityManager.GetPlayerIdAsync(),
                coreHelper.GetProjectInfo().ProjectId,
                dataModel, async (result) =>
                {
                    if (result)
                    {
                        await _inventoryDal.DeleteAsync(fileName);

                    }
                });
            }
        }
    }
}
