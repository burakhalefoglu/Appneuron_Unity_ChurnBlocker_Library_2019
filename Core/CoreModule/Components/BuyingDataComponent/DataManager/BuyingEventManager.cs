using AppneuronUnity.Core.BaseServices.WebsocketAdapter.WebsocketSharp;

namespace AppneuronUnity.Core.CoreModule.Components.BuyingDataComponent.DataManager
{
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
    using AppneuronUnity.Core.Adapters.CryptoAdapter.Absrtact;
    using AppneuronUnity.Core.CoreModule.Components.BuyingDataComponent.DataAccess;
    using AppneuronUnity.Core.CoreModule.Components.BuyingDataComponent.DataModel;
    using Zenject;
    using AppneuronUnity.Core.AuthModule.ClientIdComponent.DataManager;

    internal class BuyingEventManager : IBuyingEventManager
    {

        private readonly IBuyingEventDal _buyingEventDal;

        private readonly IClientIdUnityManager _clientIdUnityManager;

        private readonly IDataCreationClient _dataCreationClient;

        private readonly ICryptoServices _cryptoServices;

        [Inject]
        private readonly CoreHelper coreHelper;

        public BuyingEventManager(IBuyingEventDal buyingEventDal,
            ICryptoServices cryptoServices,
            IDataCreationClient dataCreationClient,
            IClientIdUnityManager clientIdUnityManager)
        {
            _buyingEventDal = buyingEventDal;
            _cryptoServices = cryptoServices;
            _dataCreationClient = dataCreationClient;
            _clientIdUnityManager = clientIdUnityManager;
        }

        public async Task CheckAdvFileAndSendData()
        {
            List<string> FolderList = coreHelper.GetSavedDataFilesNames<BuyingEventDataModel>();
            if (FolderList.Count == 0)
                return;
            foreach (var fileName in FolderList)
            {
                var dataModel = await _buyingEventDal.SelectAsync(fileName);

                await _dataCreationClient.PushAsync(await _clientIdUnityManager.GetPlayerIdAsync(),
                coreHelper.GetProjectInfo().ProjectId,
                dataModel, async (result) =>
                {
                    if (result)
                    {
                        await _buyingEventDal.DeleteAsync(fileName);

                    }
                });
            }
        }

        public async Task SendAdvEventData(string Tag,
            string levelName,
            int levelIndex,
            float GameSecond,
            long clientId)
        {
            BuyingEventDataModel dataModel = new BuyingEventDataModel
            {
                ClientId = clientId,
                ProjectId = coreHelper.GetProjectInfo().ProjectId,
                CustomerId = coreHelper.GetProjectInfo().CustomerId,
                LevelName = levelName,
                LevelIndex = levelIndex,
                ProductType = Tag,
                InWhatMinutes = GameSecond,
                TrigerdTime = DateTime.Now
            };

            await _dataCreationClient.PushAsync(await _clientIdUnityManager.GetPlayerIdAsync(),
            coreHelper.GetProjectInfo().ProjectId,
            dataModel, async (result) =>
            {
                if (!result)
                {
                    string fileName = _cryptoServices.GenerateStringName(6);
                    await _buyingEventDal.InsertAsync(fileName, dataModel);
                }
            });

        }
    }
}
