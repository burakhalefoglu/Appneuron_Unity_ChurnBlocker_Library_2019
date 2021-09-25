namespace AppneuronUnity.Core.CoreModule.Components.BuyingDataComponent.UnityManager
{
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AppneuronUnity.Core.AuthModule.ClientIdComponent.UnityManager;
    using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
using AppneuronUnity.Core.Adapters.CryptoAdapter.Absrtact;
using AppneuronUnity.Core.CoreModule.Components.BuyingDataComponent.DataAccess;
using AppneuronUnity.Core.CoreModule.Components.BuyingDataComponent.DataModel;
using AppneuronUnity.Core.CoreModule.Components.BuyingDataComponent.UnityManager;

    /// <summary>
    /// Defines the <see cref="BuyingEventManager" />.
    /// </summary>
    internal class BuyingEventManager : IBuyingEventManager
    {

        private readonly IBuyingEventDal _buyingEventDal;

        private readonly IClientIdUnityManager _clientIdUnityManager;

        private readonly IDataCreationClient _dataCreationClient;

        private readonly ICryptoServices _cryptoServices;

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
            List<string> FolderList = ComponentsConfigs.GetSavedDataFilesNames(ComponentsConfigs
                                                                                      .SaveTypePath
                                                                                      .BuyingEventDataModel);

            foreach (var fileName in FolderList)
            {
                var dataModel = await _buyingEventDal.SelectAsync(ComponentsConfigs.BuyingEventDataPath + fileName);

                await _dataCreationClient.PushAsync(_clientIdUnityManager.GetPlayerID(),
                dataModel, async (result) =>
                {
                    if (result)
                    {
                        await _buyingEventDal.DeleteAsync(ComponentsConfigs.AdvEventDataPath + fileName);

                    }
                });
            }
        }

        public async Task SendAdvEventData(string Tag,
            string levelName,
            int levelIndex,
            float GameSecond,
            string clientId)
        {
            BuyingEventDataModel dataModel = new BuyingEventDataModel
            {
                ClientId = clientId,
                ProjectId = ChurnBlockerSingletonConfigs.Instance.GetProjectID(),
                CustomerId = ChurnBlockerSingletonConfigs.Instance.GetCustomerID(),
                LevelName = levelName,
                LevelIndex = levelIndex,
                ProductType = Tag,
                InWhatMinutes = GameSecond,
                TrigerdTime = DateTime.Now
            };

            await _dataCreationClient.PushAsync(_clientIdUnityManager.GetPlayerID(),
            dataModel, async (result) =>
            {
                if (!result)
                {
                    string fileName = _cryptoServices.GenerateStringName(6);
                    string filepath = ComponentsConfigs.AdvEventDataPath + fileName;
                    await _buyingEventDal.InsertAsync(filepath, dataModel);
                }
            });

        }
    }
}
