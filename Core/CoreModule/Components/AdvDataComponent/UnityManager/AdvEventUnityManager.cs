namespace AppneuronUnity.Core.CoreModule.Components.AdvDataComponent.UnityManager
{
    using AppneuronUnity.ProductModules.ChurnBlockerModule.Configs;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AppneuronUnity.Core.Adapters.WebsocketAdapter.WebsocketSharp;
    using AppneuronUnity.Core.AuthModule.ClientIdComponent.UnityManager;
using AppneuronUnity.Core.Adapters.CryptoAdapter.Absrtact;
using AppneuronUnity.Core.CoreModule.Components.AdvDataComponent.DataAccess;
using AppneuronUnity.Core.CoreModule.Components.AdvDataComponent.DataModel;
using AppneuronUnity.Core.CoreModule.Components.AdvDataComponent.UnityManager;

    /// <summary>
    /// Defines the <see cref="AdvEventUnityManager" />.
    /// </summary>
    internal class AdvEventUnityManager : IAdvEventUnityManager
    {
        private readonly ICryptoServices _cryptoServices;

        private readonly IAdvEventDal _advEventDal;

        private readonly IClientIdUnityManager _clientIdUnityManager;

        private readonly IDataCreationClient _dataCreationClient;


        public AdvEventUnityManager(ICryptoServices cryptoServices,
            IAdvEventDal advEventDal,
            IDataCreationClient dataCreationClient,
            IClientIdUnityManager clientIdUnityManager)
        {
            _cryptoServices = cryptoServices;
            _advEventDal = advEventDal;
            _dataCreationClient = dataCreationClient;
            _clientIdUnityManager = clientIdUnityManager;
        }

        public async Task CheckAdvFileAndSendData()
        {
            List<string> FolderNameList = ComponentsConfigs.GetSavedDataFilesNames(ComponentsConfigs
                                                                                          .SaveTypePath
                                                                                          .AdvEventDataModel);
            foreach (var fileName in FolderNameList)
            {
                var dataModel = await _advEventDal.SelectAsync(ComponentsConfigs.AdvEventDataPath + fileName);

                await _dataCreationClient.PushAsync(_clientIdUnityManager.GetPlayerID(),
                dataModel, async (result) =>
                {
                    if (result)
                    {
                        await _advEventDal.DeleteAsync(ComponentsConfigs.AdvEventDataPath + fileName);
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
            AdvEventDataModel advEventDataModel = new AdvEventDataModel
            {
                ClientId = clientId,
                ProjectId = ChurnBlockerSingletonConfigs.Instance.GetProjectID(),
                CustomerId = ChurnBlockerSingletonConfigs.Instance.GetCustomerID(),
                LevelName = levelName,
                LevelIndex = levelIndex,
                AdvType = Tag,
                InMinutes = GameSecond,
                TrigerdTime = DateTime.Now
            };

            await _dataCreationClient.PushAsync(_clientIdUnityManager.GetPlayerID(),
            advEventDataModel, async (result) =>
            {

                if (!result)
                {
                    string fileName = _cryptoServices.GenerateStringName(6);
                    string filepath = ComponentsConfigs.AdvEventDataPath + fileName;
                    await _advEventDal.InsertAsync(filepath, advEventDataModel);
                }


            });

        }
    }
}
